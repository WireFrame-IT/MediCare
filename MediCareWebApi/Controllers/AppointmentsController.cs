using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediCare.DTOs.Request;
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
		[Authorize(Roles = "Doctor, Patient")]
		public async Task<IActionResult> GetAppointmentsAsync()
		{
			var user = await GetCurrentUserAsync();
			switch (user.Role.RoleType)
			{
				case RoleType.Doctor:
					var doctor = await _context.Doctors.FirstOrDefaultAsync(x => x.UserId == user.Id);
					if (doctor == null)
						return NotFound("Doctor not found.");
					return Ok(await _context.Appointments.Where(x => x.DoctorId == doctor.Id).ToListAsync());

				case RoleType.Patient:
					var patient = await _context.Patients.FirstOrDefaultAsync(x => x.UserId == user.Id);
					if (patient == null)
						return NotFound("Patient not found.");
					return Ok(await _context.Appointments.Where(x => x.PatientId == patient.Id).ToListAsync());
			}

			return NotFound();
		}

		[HttpPost]
		[Authorize(Roles = "Patient")]
		public async Task<IActionResult> SaveAppointmentAsync([FromBody] AppointmentRequestDTO appointmentDTO)
		{
			var appointment = _mapper.Map<Appointment>(appointmentDTO);
			var userId = GetCurrentUserId();
			var patient = await _context.Patients.FirstOrDefaultAsync(x => x.UserId == userId);

			if (patient == null)
				return BadRequest("Only patient can make the appointment.");

			appointment.Status = AppointmentStatus.New;
			appointment.PatientId = patient.Id;

			if (appointmentDTO.ServiceId.HasValue)
			{
				appointment.Service = await _context.Services.FirstOrDefaultAsync(x => x.Id == appointmentDTO.ServiceId.Value);
				if (appointment.Service == null)
					return NotFound("Service not found.");
			}
			else
			{
				appointment.Service = await _context.Services.FirstOrDefaultAsync(x => x.Name == _appointmentSettings.FamilyMedicineServiceName
					&& x.Description == _appointmentSettings.FamilyMedicineServiceDescription);
			}

			if (appointmentDTO.DoctorId.HasValue)
			{
				var doctor = await _context.Doctors.FirstOrDefaultAsync(x => x.Id == appointmentDTO.DoctorId.Value);
				if (doctor == null)
					return NotFound("Doctor not found.");
				appointment.Doctor = doctor;
			}
			else
			{
				var doctor = await FindAvailableDoctorAsync(appointment.Time, appointment.Service);
				if (doctor == null)
					return NotFound("No available doctor for the specified term.");
				appointment.Doctor = doctor;
			}

			ValidateAppointment(appointment);
			_context.Appointments.Add(appointment);
			await _context.SaveChangesAsync();
			return Ok(appointment);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> GetServices()
		{
			return Ok(await _context.Services.ToListAsync());
		}

		private async Task<Doctor?> FindAvailableDoctorAsync(DateTime time, Service service)
		{
			var doctors = await _context.Doctors
				.Include(x => x.Appointments)
				.Where(x => x.SpecialityId == service.SpecialityId)
				.ToListAsync();

			foreach (var doctor in doctors)
			{
				if (!doctor.Appointments
					    .Where(x => x.Status != AppointmentStatus.Absent && x.Status != AppointmentStatus.Canceled)
					    .Any(x => x.Time < time.AddMinutes(service.DurationMinutes) && time < x.Time.AddMinutes(x.Service.DurationMinutes)))
					return doctor;
			}
			return null;
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
