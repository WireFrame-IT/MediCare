using AutoMapper;
using MediCare.DTOs.Request;
using MediCare.DTOs.ViewModels;
using MediCare.Enums;
using MediCare.Models;
using MediCare.ServiceModels;
using MediCare.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MediCare.Controllers
{
    [Authorize]
	[ApiController]
	[Route("[controller]")]
	public class AppointmentsController : BaseController
	{
		private readonly AppointmentSettings _appointmentSettings;

		public AppointmentsController(MediCareDbContext context, IConfiguration configuration, IMapper mapper, IAccountsService accountsService, IOptions<AppointmentSettings> appointmentSettings)
			: base(context, configuration, mapper, accountsService)
		{
			_appointmentSettings = appointmentSettings.Value;
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetAppointmentsAsync()
		{
			var user = await GetCurrentUserAsync();

			switch (user.Role.RoleType)
			{
				case RoleType.Doctor:
					var doctor = await _context.Doctors.FirstOrDefaultAsync(x => x.UserId == user.Id);
					if (doctor == null)
						return NotFound("Lekarz nie został znaleziony.");

					var actionResult = await CheckPermission(PermissionType.ViewAllAppointments);
					var appointments = actionResult == null ? await GetAllAppointmentsAsync() : await GetAllAppointmentsAsync(doctor.UserId, true);
					return Ok(_mapper.Map<List<AppointmentDTO>>(appointments));

				case RoleType.Patient:
					var patient = await _context.Patients.FirstOrDefaultAsync(x => x.UserId == user.Id);
					if (patient == null)
						return NotFound("Pacjent nie został znaleziony.");

					return Ok(_mapper.Map<List<ReducedAppointmentDTO>>(await GetAllAppointmentsAsync(patient.UserId)));

				case RoleType.Admin:
					return Ok(_mapper.Map<List<AppointmentDTO>>(await GetAllAppointmentsAsync()));

				default:
					return NotFound();
			}
		}

		[HttpPost]
		[Authorize(Roles = "Admin, Doctor, Patient")]
		public async Task<IActionResult> SaveAppointmentAsync([FromBody] AppointmentRequestDTO appointmentRequestDTO)
		{
			var user = await GetCurrentUserAsync();
			var isNew = appointmentRequestDTO.Id == null || appointmentRequestDTO.Id == 0;
			var appointment = isNew
				? _mapper.Map<Appointment>(appointmentRequestDTO)
				: await _context.Appointments
					.Include(x => x.Service)
					.Include(x => x.Patient)
						.ThenInclude(x => x.User)
					.Include(x => x.Doctor)
						.ThenInclude(x => x.User)
					.Include(x => x.Doctor)
						.ThenInclude(x => x.Speciality)
					.FirstOrDefaultAsync(x => x.Id == appointmentRequestDTO.Id);

			if (isNew && user.Role.RoleType != RoleType.Patient)
				return BadRequest("Tylko pacjent może umówić wizytę.");

			switch (user.Role.RoleType)
			{
				case RoleType.Admin:
					appointment.Status = appointmentRequestDTO.Status.Value;
					break;
				case RoleType.Patient:
					appointment.Status = AppointmentStatus.New;
					appointment.PatientsUserId = user.Id;
					break;
				case RoleType.Doctor:
					appointment.Diagnosis = appointmentRequestDTO.Diagnosis;
					await _context.SaveChangesAsync();
					return Ok(_mapper.Map<AppointmentDTO>(appointment));
			}

			if (!isNew)
			{
				appointment.Time = appointmentRequestDTO.Time;
			}

			if (appointmentRequestDTO.ServiceId.HasValue)
			{
				appointment.Service = await _context.Services.FirstOrDefaultAsync(x => x.Id == appointmentRequestDTO.ServiceId.Value);
				if (appointment.Service == null)
					return NotFound("Usługa nie została znaleziona.");
			}
			else
			{
				appointment.Service = await _context.Services.FirstOrDefaultAsync(x => x.Name == _appointmentSettings.FamilyMedicineServiceName);
			}

			if (appointmentRequestDTO.DoctorsUserId.HasValue && appointmentRequestDTO.DoctorsUserId.Value > 0)
			{
				var actionResult = await CheckPermission(PermissionType.ChooseDoctor);
				if (actionResult != null)
					return actionResult;

				var doctor = await FindAvailableDoctorAsync(appointment.Time, appointment.Service, appointment.Id, appointment.DoctorsUserId);
				if (doctor == null)
					return NotFound("Wybrany lekarz nie jest dostępny o podanej godzinie. Jeśli nie wybierzesz lekarza, przypiszemy dostępnego automatycznie.");
				appointment.Doctor = doctor;
			}
			else
			{
				var doctor = await FindAvailableDoctorAsync(appointment.Time, appointment.Service, appointment.Id);
				if (doctor == null)
					return NotFound("Brak dostępnego lekarza w wybranym terminie.");
				appointment.Doctor = doctor;
			}

			var validationResult = ValidateAppointment(appointment, user.Role.RoleType);
			if (validationResult != null)
				return validationResult;

			if (appointment.Id == 0)
				_context.Appointments.Add(appointment);
			else if (user.Role.RoleType == RoleType.Patient)
				return BadRequest("Pacjent nie może edytować wizyty.");

			await _context.SaveChangesAsync();
			return Ok(_mapper.Map<AppointmentDTO>(appointment));
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("doctors")]
		public async Task<IActionResult> GetDoctorsAsync()
		{
			return Ok(_mapper.Map<List<DoctorDTO>>(await _context.Doctors
				.Include(x => x.User)
				.Include(x => x.Speciality)
				.OrderBy(x => x.User.Name)
				.ThenBy(x => x.User.Surname)
				.ToListAsync()));
		}

		[Authorize]
		[HttpGet("reduced-doctors")]
		public async Task<IActionResult> GetReducedDoctorsAsync()
		{
			var reducedDoctors = await _context.Doctors
				.Include(x => x.User)
				.Include(x => x.Speciality)
				.Include(x => x.DoctorsAvailabilities.Where(y => y.To == null || DateTime.Now <= y.To))
				.ToListAsync();

			var reducedDoctorsDTO = _mapper.Map<List<ReducedDoctorDTO>>(reducedDoctors);

			foreach (var doctor in reducedDoctorsDTO)
			{
				doctor.UnavailableTerms = await _context.Appointments
					.Where(x => x.DoctorsUserId == doctor.UserId && x.Time.Date >= DateTime.Now.Date && x.Status != AppointmentStatus.Canceled && x.Status != AppointmentStatus.Absent)
					.Select(x => new TimeRangeDTO { Start = x.Time, End = x.Time.AddMinutes(x.Service.DurationMinutes)})
					.ToListAsync();
			}

			return Ok(reducedDoctorsDTO);
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("patients")]
		public async Task<IActionResult> GetPatientsAsync()
		{
			return Ok(_mapper.Map<List<PatientDTO>>(await _context.Patients
				.Include(x => x.User)
				.OrderBy(x => x.User.Name)
				.ThenBy(x => x.User.Surname)
				.ToListAsync()));
		}

		[AllowAnonymous]
		[HttpGet("services")]
		public async Task<IActionResult> GetServicesAsync()
		{
			return Ok(_mapper.Map<List<ServiceDTO>>(await _context.Services.Include(x => x.Speciality).OrderBy(x => x.Name).ToListAsync()));
		}

		[AllowAnonymous]
		[HttpGet("medicaments")]
		public async Task<IActionResult> GetMedicamentsAsync()
		{
			return Ok(_mapper.Map<List<MedicamentDTO>>(await _context.Medicaments.OrderBy(x => x.Name).ToListAsync()));
		}

		[Authorize]
		[HttpGet("prescriptions")]
		public async Task<IActionResult> GetPrescriptionsAsync()
		{
			var user = await GetCurrentUserAsync();
			IQueryable<Prescription> query = _context.Prescriptions
				.Include(x => x.PrescriptionMedicaments)
				.ThenInclude(x => x.Medicament);

			switch (user.Role.RoleType)
			{
				case RoleType.Patient:
					query = query.Where(x => x.Appointment.PatientsUserId == user.Id);
					break;
				case RoleType.Doctor:
					var actionResult = await CheckPermission(PermissionType.ViewAllAppointments);
					if (actionResult != null)
						query = query.Where(x => x.Appointment.DoctorsUserId == user.Id);
					break;
				case RoleType.Admin:
					break;
				default:
					return Forbid();
			}

			return Ok(_mapper.Map<List<PrescriptionDTO>>(await query.ToListAsync()));
		}

		[Authorize(Roles = "Doctor")]
		[HttpPost("prescription")]
		public async Task<IActionResult> SavePrescriptionAsync([FromBody] PrescriptionRequestDTO prescriptionRequestDto)
		{
			var prescription = await _context.Prescriptions
				.Include(x => x.PrescriptionMedicaments)
					.ThenInclude(x => x.Medicament)
				.FirstOrDefaultAsync(x => x.AppointmentId == prescriptionRequestDto.AppointmentId);

			if (prescriptionRequestDto.ExpirationDate <= DateTime.Now.Date)
				return BadRequest("Nieprawidłowa data ważności.");

			if (prescription == null)
			{
				prescription = _mapper.Map<Prescription>(prescriptionRequestDto);
				prescription.IssueDate = DateTime.Now;
				await _context.AddAsync(prescription);
			}
			else
			{
				prescription.Description = prescriptionRequestDto.Description;
				prescription.ExpirationDate = prescriptionRequestDto.ExpirationDate;
			}

			await _context.SaveChangesAsync();
			return Ok(_mapper.Map<PrescriptionDTO>(prescription));
		}

		[Authorize(Roles = "Doctor")]
		[HttpPost("prescription-medicament")]
		public async Task<IActionResult> AddPrescriptionMedicamentAsync([FromBody] PrescriptionMedicamentRequestDTO prescriptionMedicamentRequestDTO)
		{
			var result = await ValidateDoctorAsync(prescriptionMedicamentRequestDTO.PrescriptionAppointmentId);
			if (result != null)
				return result;

			await _context.PrescriptionMedicaments.AddAsync(_mapper.Map<PrescriptionMedicament>(prescriptionMedicamentRequestDTO));
			await _context.SaveChangesAsync();

			var prescriptionMedicament = await _context.PrescriptionMedicaments
				.Include(x => x.Medicament)
				.FirstOrDefaultAsync(x => x.PrescriptionAppointmentId == prescriptionMedicamentRequestDTO.PrescriptionAppointmentId && x.MedicamentId == prescriptionMedicamentRequestDTO.MedicamentId);
			return Ok(_mapper.Map<PrescriptionMedicamentDTO>(prescriptionMedicament));
		}

		[Authorize(Roles = "Doctor")]
		[HttpDelete("prescription-medicament")]
		public async Task<IActionResult> DeletePrescriptionMedicamentAsync([FromQuery] int prescriptionAppointmentId, [FromQuery] int medicamentId)
		{
			var result = await ValidateDoctorAsync(prescriptionAppointmentId);
			if (result != null)
				return result;

			var prescriptionMedicament = await _context.PrescriptionMedicaments
				.FirstOrDefaultAsync(x => x.PrescriptionAppointmentId == prescriptionAppointmentId && x.MedicamentId == medicamentId);
			if (prescriptionMedicament == null)
				return BadRequest("Przypisany lek nie istnieje.");

			_context.PrescriptionMedicaments.Remove(prescriptionMedicament);
			await _context.SaveChangesAsync();
			return Ok();
		}

		[Authorize(Roles = "Doctor")]
		[HttpPost("accept")]
		public async Task<IActionResult> AcceptAppointmentAsync([FromQuery] int id)
		{
			var appointment = await GetAppointmentAsync(id);
			if (appointment == null)
				return NotFound("Wizyta nie istnieje lub nie masz uprawnień do jej zaakceptowania.");
			appointment.Status = AppointmentStatus.Accepted;
			await _context.SaveChangesAsync();
			return Ok();
		}

		[Authorize(Roles = "Doctor")]
		[HttpPost("confirm")]
		public async Task<IActionResult> ConfirmAppointmentAsync([FromQuery] int id)
		{
			var appointment = await GetAppointmentAsync(id);
			if (appointment == null)
				return NotFound("Wizyta nie istnieje lub nie masz uprawnień do jej potwierdzenia.");

			if (DateTime.Now < appointment.Time)
				return BadRequest("Zbyt wcześnie, aby potwierdzić wizytę.");

			appointment.Status = AppointmentStatus.Confirmed;
			await _context.SaveChangesAsync();
			return Ok();
		}

		[Authorize]
		[HttpPost("cancel")]
		public async Task<IActionResult> CancelAppointmentAsync([FromQuery] int id)
		{
			var actionResult = await CheckPermission(PermissionType.CancelAppointment);
			if (actionResult != null)
				return actionResult;

			var appointment = await GetAppointmentAsync(id);
			if (appointment == null)
				return NotFound("Wizyta nie istnieje lub nie masz uprawnień do jej anulowania.");
			appointment.Status = AppointmentStatus.Canceled;
			await _context.SaveChangesAsync();
			return Ok();
		}

		[HttpGet("feedbacks")]
		public async Task<IActionResult> GetFeedbacksAsync()
		{
			var user = await GetCurrentUserAsync();
			var query = _context.Feedbacks.AsQueryable();

			switch (user.Role.RoleType)
			{
				case RoleType.Patient:
					query = query.Where(x => x.Appointment.PatientsUserId == user.Id);
					break;
				case RoleType.Doctor:
					query = query.Where(x => x.Appointment.DoctorsUserId == user.Id);
					break;
				case RoleType.Admin:
					break;
				default:
					return Forbid();
			}

			return Ok(_mapper.Map<List<FeedbackDTO>>(await query.ToListAsync()));
		}

		[Authorize(Roles = "Patient")]
		[HttpPost("feedback")]
		public async Task<IActionResult> SaveFeedbackAsync([FromBody] FeedbackRequestDTO feedbackRequestDto)
		{
			var userId = GetCurrentUserId();
			var appointment = await _context.Appointments.FirstOrDefaultAsync(x => x.Id == feedbackRequestDto.AppointmentId);

			if (appointment == null)
				return NotFound("Wizyta nie została znaleziona.");

			if (appointment.PatientsUserId != userId)
				return Unauthorized("Brak odpowiednich uprawnień.");

			var feedback = await _context.Feedbacks.FirstOrDefaultAsync(x => x.Appointment.PatientsUserId == userId && x.AppointmentId == feedbackRequestDto.AppointmentId);
			if (feedback == null)
			{
				feedback = _mapper.Map<Feedback>(feedbackRequestDto);
				feedback.CreatedAt = DateTime.Now;
				await _context.Feedbacks.AddAsync(feedback);
			}
			else
			{
				feedback.Rate = feedbackRequestDto.Rate;
				feedback.Description = feedbackRequestDto.Description;
			}

			await _context.SaveChangesAsync();
			return Ok(_mapper.Map<FeedbackDTO>(feedback));
		}

		private async Task<IActionResult?> ValidateDoctorAsync(int appointmentId)
		{
			var userId = GetCurrentUserId();
			var appointment = await _context.Appointments.FirstOrDefaultAsync(x => x.Id == appointmentId);

			if (appointment == null)
				return NotFound("Wizyta nie została znaleziona.");

			if (appointment.DoctorsUserId != userId)
				return Unauthorized("Brak odpowiednich uprawnień.");

			return null;
		}

		private async Task<Appointment?> GetAppointmentAsync(int id)
		{
			var userId = GetCurrentUserId();
			return await _context.Appointments
				.Include(x => x.Doctor)
				.Include(x => x.Patient)
				.FirstOrDefaultAsync(x => x.Id == id && (x.Doctor.UserId == userId || x.Patient.UserId == userId));
		}

		private async Task<Doctor?> FindAvailableDoctorAsync(DateTime time, Service service, int ignoreAppointmentId = 0, int? doctorsUserId = null)
		{
			var appointmentEndTime = time.AddMinutes(service.DurationMinutes);
			return await _context.Doctors
				.Where(x => x.SpecialityId == service.SpecialityId && (doctorsUserId == null || x.UserId == doctorsUserId)
				    && x.DoctorsAvailabilities.Any(y => y.From <= time && appointmentEndTime <= y.To)
					&& !x.Appointments.Any(y => y.Id != ignoreAppointmentId && y.Status != AppointmentStatus.Absent && y.Status != AppointmentStatus.Canceled
					&& y.Time < appointmentEndTime && time < y.Time.AddMinutes(y.Service.DurationMinutes)))
				.FirstOrDefaultAsync();
		}

		private async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync(int? userId = null, bool doctor = false)
		{
			var query = _context.Appointments
				.Include(x => x.Doctor)
					.ThenInclude(x => x.User)
				.Include(x => x.Doctor)
					.ThenInclude(x => x.Speciality)
				.Include(x => x.Patient)
					.ThenInclude(x => x.User)
				.Include(x => x.Service)
				.AsQueryable();

			if (userId.HasValue)
			{
				query = query.Where(x => (doctor ? x.DoctorsUserId : x.PatientsUserId) == userId);
			}

			return await query.OrderByDescending(x => x.Time).ToListAsync();
		}

		private IActionResult? ValidateAppointment(Appointment appointment, RoleType roleType)
		{
			if (!_appointmentSettings.AvailableDays.Contains((int)appointment.Time.DayOfWeek))
				return BadRequest($"Wizyta nie jest dozwolona {GetPolishDayName(appointment.Time.DayOfWeek)}.");

			if (appointment.Time.Minute % 15 != 0 || appointment.Time < DateTime.Now && roleType != RoleType.Admin)
				return BadRequest("Czas wizyty jest nieprawidłowy.");

			if (appointment.Time.TimeOfDay < TimeSpan.FromHours(_appointmentSettings.StartHour) || appointment.Time.TimeOfDay >= TimeSpan.FromHours(_appointmentSettings.EndHour))
				return BadRequest($"Wizyty można umawiać tylko w godzinach od {_appointmentSettings.StartHour} do {_appointmentSettings.EndHour}.");

			if (appointment.Time.AddMinutes(appointment.Service.DurationMinutes) > appointment.Time.Date.AddHours(_appointmentSettings.EndHour))
				return BadRequest($"Czas trwania wizyty przekracza {TimeSpan.FromHours(_appointmentSettings.EndHour).ToString(@"hh\:mm")}.");

			return null;
		}

		private string GetPolishDayName(DayOfWeek dayOfWeek) => dayOfWeek switch
		{
			DayOfWeek.Monday => "w poniedziałek",
			DayOfWeek.Tuesday => "we wtorek",
			DayOfWeek.Wednesday => "w środę",
			DayOfWeek.Thursday => "w czwartek",
			DayOfWeek.Friday => "w piątek",
			DayOfWeek.Saturday => "w sobotę",
			DayOfWeek.Sunday => "w niedzielę"
		};
	}
}
