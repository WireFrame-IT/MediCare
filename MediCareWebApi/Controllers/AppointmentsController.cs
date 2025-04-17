using System.ComponentModel.DataAnnotations;
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

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetAppointmentsAsync()
		{
			var user = await GetCurrentUserAsync();
			switch (user.Role.RoleType)
			{
				case RoleType.Doctor:
					var doctor = await _context.Doctors.FirstOrDefaultAsync(x => x.UserId == user.Id);
					if (doctor == null)
						return NotFound("Doctor not found.");
					return Ok(_mapper.Map<List<AppointmentDTO>>(await GetAppointments(doctor.UserId, true)));

				case RoleType.Patient:
					var patient = await _context.Patients.FirstOrDefaultAsync(x => x.UserId == user.Id);
					if (patient == null)
						return NotFound("Patient not found.");
					return Ok(_mapper.Map<List<AppointmentDTO>>(await GetAppointments(patient.UserId)));

				case RoleType.Admin:
					return Ok(_mapper.Map<List<AppointmentDTO>>(await GetAppointments()));
			}

			return NotFound();
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
				return BadRequest("Only patient can make an appointment.");

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
					return NotFound("Service not found.");
			}
			else
			{
				appointment.Service = await _context.Services.FirstOrDefaultAsync(x => x.Name == _appointmentSettings.FamilyMedicineServiceName);
			}

			if (appointmentRequestDTO.DoctorsUserId.HasValue && appointmentRequestDTO.DoctorsUserId.Value > 0)
			{
				var doctor = await FindAvailableDoctorAsync(appointment.Time, appointment.Service, appointment.Id, appointment.DoctorsUserId);
				if (doctor == null)
					return NotFound("The selected doctor is not available at the provided time. If you don't choose a specific doctor, we'll automatically assign an available one for you.");
				appointment.Doctor = doctor;
			}
			else
			{
				var doctor = await FindAvailableDoctorAsync(appointment.Time, appointment.Service, appointment.Id);
				if (doctor == null)
					return NotFound("No available doctor for the specified term.");
				appointment.Doctor = doctor;
			}

			ValidateAppointment(appointment);

			if (appointment.Id == 0)
				_context.Appointments.Add(appointment);
			else if (user.Role.RoleType == RoleType.Patient)
				return BadRequest("Patient cannot edit appointment.");

			await _context.SaveChangesAsync();
			return Ok(_mapper.Map<AppointmentDTO>(appointment));
		}

		[Authorize]
		[HttpGet("doctors")]
		public async Task<IActionResult> GetDoctors()
		{
			return Ok(_mapper.Map<List<DoctorDTO>>(await _context.Doctors
				.Include(x => x.User)
				.Include(x => x.Speciality)
				.ToListAsync()));
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("patients")]
		public async Task<IActionResult> GetPatients()
		{
			return Ok(_mapper.Map<List<PatientDTO>>(await _context.Patients
				.Include(x => x.User)
				.ToListAsync()));
		}

		[AllowAnonymous]
		[HttpGet("services")]
		public async Task<IActionResult> GetServices()
		{
			return Ok(_mapper.Map<List<ServiceDTO>>(await _context.Services.ToListAsync()));
		}

		[HttpPost("accept")]
		public async Task<IActionResult> AcceptAppointment([FromQuery] int id)
		{
			var appointment = await GetAppointmentAsync(id);
			if (appointment == null)
				return NotFound();
			appointment.Status = AppointmentStatus.Accepted;
			await _context.SaveChangesAsync();
			return Ok();
		} 

		[HttpPost("cancel")]
		public async Task<IActionResult> CancelAppointment([FromQuery] int id)
		{
			var appointment = await GetAppointmentAsync(id);
			if (appointment == null)
				return NotFound();
			appointment.Status = AppointmentStatus.Canceled;
			await _context.SaveChangesAsync();
			return Ok();
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

		private async Task<IEnumerable<Appointment>> GetAppointments(int? userId = null, bool doctor = false)
		{
			var baseQuery = _context.Appointments
				.Include(x => x.Doctor)
					.ThenInclude(x => x.User)
				.Include(x => x.Doctor)
					.ThenInclude(x => x.Speciality)
				.Include(x => x.Patient)
					.ThenInclude(x => x.User)
				.Include(x => x.Service);

			IQueryable<Appointment> query = baseQuery;

			if (userId.HasValue)
			{
				query = query.Where(x => (doctor ? x.DoctorsUserId : x.PatientsUserId) == userId);
			}

			return await query.ToListAsync();
		}

		private void ValidateAppointment(Appointment appointment)
		{
			var dayOfWeek = appointment.Time.DayOfWeek.ToString();
			if (!_appointmentSettings.AvailableDays.Contains(dayOfWeek))
				throw new ValidationException($"Appointment is not allowed on {dayOfWeek}.");

			if (appointment.Time.Minute % 15 != 0 || appointment.Time < DateTime.Now)
				throw new ValidationException("Appointment time is not correct.");

			if (appointment.Time.TimeOfDay < TimeSpan.FromHours(_appointmentSettings.StartHour) || appointment.Time.TimeOfDay >= TimeSpan.FromHours(_appointmentSettings.EndHour))
				throw new ValidationException($"Appointments can only be made within the time range of {_appointmentSettings.StartHour} to {_appointmentSettings.EndHour}.");

			if (appointment.Time.AddMinutes(appointment.Service.DurationMinutes) > appointment.Time.Date.AddHours(_appointmentSettings.EndHour))
				throw new ValidationException($"Appointment duration exceeds {TimeSpan.FromHours(_appointmentSettings.EndHour).ToString(@"hh\:mm")}.");
		}
	}
}
