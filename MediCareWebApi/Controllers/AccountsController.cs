using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using MediCare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediCare.Controllers;
using MediCare.Enums;
using MediCare.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediCare.DTOs.Request;
using MediCare.DTOs.Response;
using MediCare.DTOs.ViewModels;

namespace MedicalFacility.Controllers
{
    [Authorize]
	[ApiController]
	[Route("[controller]")]
	public class AccountsController : BaseController
	{
		private static readonly AppointmentStatus[] _cannotCancelAppointmentStatuses = [ AppointmentStatus.Accepted, AppointmentStatus.Confirmed ];

		public AccountsController(MediCareDbContext context, IConfiguration configuration, IMapper mapper,
			IAccountsService accountsService)
			: base(context, configuration, mapper, accountsService)
		{ }

		[AllowAnonymous]
		[HttpPost("register")]
		public async Task<IActionResult> PatientRegisterAsync([FromBody] PatientRegisterRequestDTO registerRequest)
		{
			var user = await RegisterUserAsync(registerRequest, RoleType.Patient);
			var patient = new Patient()
			{
				RegisterDate = DateTime.Now,
				BirthDate = registerRequest.BirthDate.Date,
				UserId = user.Id,
			};

			if (patient.BirthDate > DateTime.Now)
				throw new InvalidOperationException("Wrong birth date.");

			await _context.Patients.AddAsync(patient);
			await _context.SaveChangesAsync();
			return Ok(new LoginResponseDTO()
			{
				UserId = user.Id,
				UserName = user.Name,
				UserSurname = user.Surname,
				AccessToken = _accountsService.GenerateAccessToken(user),
				RefreshToken = user.RefreshToken,
				RoleType = RoleType.Patient
			});
		}

		[Authorize(Roles = "Admin")]
		[HttpPost("register-user")]
		public async Task<IActionResult> UserRegisterAsync([FromBody] UserRegisterRequestDTO registerRequest)
		{
			var user = await RegisterUserAsync(registerRequest, registerRequest.RoleType);
			switch (registerRequest.RoleType)
			{
				case RoleType.Doctor:
					var doctor = new Doctor()
					{
						EmploymentDate = registerRequest.EmploymentDate.Value,
						SpecialityId = registerRequest.SpecialityId.Value,
						UserId = user.Id,
					};

					await _context.Doctors.AddAsync(doctor);
					break;

				case RoleType.Patient:
					var patient = new Patient()
					{
						BirthDate = registerRequest.BirthDate.Value,
						UserId = user.Id,
					};

					if (patient.BirthDate > DateTime.Now)
						throw new InvalidOperationException("Wrong birth date.");

					await _context.Patients.AddAsync(patient);
					break;
			}
			
			await _context.SaveChangesAsync();
			return Ok();
		}

		[AllowAnonymous]
		[HttpPost("login")]
		public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDTO loginRequest)
		{
			var user = await _context.Users
				.Include(x => x.Role)
				.FirstOrDefaultAsync(x => x.Email == loginRequest.Email);

			if (user == null || _accountsService.HashPassword(loginRequest.Password, user.Salt) != user.Password)
				return Unauthorized("Invalid email or password.");

			user.RefreshToken = _accountsService.GenerateSalt();
			user.RefreshTokenExpiration = DateTime.Now.AddMinutes(double.Parse(
				_configuration.GetSection("JwtSettings:RefreshTokenExpirationMinutes").Value));
			await _context.SaveChangesAsync();

			return Ok(new LoginResponseDTO
			{
				UserId = user.Id,
				UserName = user.Name,
				UserSurname = user.Surname,
				AccessToken = _accountsService.GenerateAccessToken(user),
				RefreshToken = user.RefreshToken,
				RoleType = user.Role.RoleType
			});
		}

		[AllowAnonymous]
		[HttpPost("refresh")]
		public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshRequestDTO refreshRequest)
		{
			var user = await _context.Users
				.Include(x => x.Role)
				.SingleOrDefaultAsync(x => x.RefreshToken == refreshRequest.RefreshToken);

			if (user == null)
				return Unauthorized("Invalid refresh token.");

			if (user.RefreshTokenExpiration.Value < DateTime.Now)
				return Unauthorized("Refresh token expired.");

			user.RefreshToken = _accountsService.GenerateSalt();
			await _context.SaveChangesAsync();

			return Ok(new LoginResponseDTO()
			{
				UserId = user.Id,
				UserName = user.Name,
				UserSurname = user.Surname,
				AccessToken = _accountsService.GenerateAccessToken(user),
				RefreshToken = user.RefreshToken,
				RoleType = user.Role.RoleType
			});
		}

		[Authorize]
		[HttpPost("logout")]
		public async Task<IActionResult> LogoutAsync()
		{
			var user = await GetCurrentUserAsync();
			user.RefreshToken = null;
			user.RefreshTokenExpiration = null;
			await _context.SaveChangesAsync();

			var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
			if (!string.IsNullOrEmpty(accessToken))
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				var jwtToken = tokenHandler.ReadJwtToken(accessToken);
				var jti = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;

				if (!string.IsNullOrEmpty(jti))
					_accountsService.BlacklistToken(jti);
			}
			return Ok();
		}

		[Authorize(Roles = "Admin")]
		[HttpPost("user")]
		public async Task<IActionResult> SaveUserAsync([FromBody] UserRequestDTO userRequestDTO)
		{
			var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userRequestDTO.Id);
			if (user == null)
				return NotFound("User not found.");

			user.Name = userRequestDTO.Name;
			user.Surname = userRequestDTO.Surname;
			user.Email = userRequestDTO.Email;
			user.Pesel = userRequestDTO.Pesel;
			user.PhoneNumber = userRequestDTO.PhoneNumber;

			if (!string.IsNullOrWhiteSpace(userRequestDTO.Password))
				user.Password = _accountsService.HashPassword(userRequestDTO.Password, user.Salt = _accountsService.GenerateSalt());

			var doctor = await _context.Doctors.FirstOrDefaultAsync(x => x.UserId == user.Id);
			var patient = await _context.Patients.FirstOrDefaultAsync(x => x.UserId == user.Id);
			if (doctor == null && patient == null)
				throw new InvalidOperationException("The user is neither a patient nor a doctor.");

			if (doctor != null && userRequestDTO.SpecialityId.HasValue)
				doctor.SpecialityId = userRequestDTO.SpecialityId.Value;

			if (patient != null && userRequestDTO.BirthDate.HasValue)
				patient.BirthDate = userRequestDTO.BirthDate.Value;

			await _context.SaveChangesAsync();

			return Ok(doctor == null
				? await _context.Patients.Include(x => x.User).FirstAsync(x => x.UserId == patient.UserId)
				: await _context.Doctors.Include(x => x.User).Include(x => x.Speciality).FirstAsync(x => x.UserId == doctor.UserId));
		}

		[AllowAnonymous]
		[HttpGet("specialities")]
		public async Task<IActionResult> GetSpecialitiesAsync()
		{
			return Ok(_mapper.Map<List<SpecialityDTO>>(await _context.Specialities.OrderBy(x => x.Name).ToListAsync()));
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("permissions")]
		public async Task<IActionResult> GetPermissionsAsync()
		{
			var permissions = await _context.Permissions
				.Include(x => x.PermissionRoles)
					.ThenInclude(x => x.Role)
				.OrderBy(x => x.Description)
				.ToListAsync();

			return Ok(_mapper.Map<List<PermissionDTO>>(permissions));
		}

		[Authorize]
		[HttpGet("user-permissions")]
		public async Task<IActionResult> GetUserPermissionsAsync()
		{
			var user = await GetCurrentUserAsync();
			var permissions = await _context.Permissions
				.Where(x => x.PermissionRoles.Any(x => x.Role.Id == user.Role.Id))
				.Select(x => x.PermissionType)
				.ToListAsync();

			return Ok(permissions);
		}

		[Authorize(Roles = "Admin")]
		[HttpPost("role-permission")]
		public async Task<IActionResult> SaveRolePermissionAsync([FromBody] RolePermissionRequestDTO requestDTO)
		{
			if (await _context.RolePermissions.AnyAsync(x => x.Role.RoleType == requestDTO.RoleType && x.PermissionId == requestDTO.PermissionId))
				return BadRequest("Role permission already exists.");

			var roleId = await _context.Roles.Where(x => x.RoleType == requestDTO.RoleType).Select(x => x.Id).FirstOrDefaultAsync();
			var rolePermission = new RolePermission()
			{
				PermissionId = requestDTO.PermissionId,
				RoleId = roleId
			};

			await _context.RolePermissions.AddAsync(rolePermission);
			await _context.SaveChangesAsync();

			return Ok(await _context.RolePermissions
				.Include(x => x.Role)
				.Include(x => x.Permission)
				.FirstOrDefaultAsync(x => x.RoleId == rolePermission.RoleId && x.PermissionId == rolePermission.PermissionId));
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete("role-permission")]
		public async Task<IActionResult> DeleteRolePermissionAsync([FromQuery] RoleType roleType, [FromQuery] int permissionId)
		{
			var rolePermission = await _context.RolePermissions.FirstOrDefaultAsync(x => x.Role.RoleType == roleType && x.PermissionId == permissionId);
			if (rolePermission == null)
				return BadRequest("Role permission does not exist.");

			_context.RolePermissions.Remove(rolePermission);
			await _context.SaveChangesAsync();
			return Ok();
		}

		[Authorize(Roles = "Doctor")]
		[HttpGet("availabilities")]
		public async Task<IActionResult> GetDoctorsAvailabilitiesAsync()
		{
			var user = await GetCurrentUserAsync();
			var availabilities = await _context.DoctorsAvailabilities
				.Where(x => x.DoctorsUserId == user.Id)
				.OrderByDescending(x => x.From)
				.ToListAsync();

			return Ok(availabilities);
		}

		[Authorize(Roles = "Doctor")]
		[HttpDelete("availability")]
		public async Task<IActionResult> DeleteDoctorsAvailabilityAsync([FromQuery] int id)
		{
			var user = await GetCurrentUserAsync();
			var availability = await _context.DoctorsAvailabilities.FirstOrDefaultAsync(x => x.Id == id && x.DoctorsUserId == user.Id);
			if (availability == null)
				return BadRequest("Availability does not exist.");

			if (await _context.Appointments.AnyAsync(x => x.DoctorsUserId == user.Id && _cannotCancelAppointmentStatuses.Contains(x.Status) && x.Time < availability.To && availability.From < x.Time.AddMinutes(x.Service.DurationMinutes)))
				return BadRequest("Cancel accepted and confirmed appointments without availability first.");

			_context.DoctorsAvailabilities.Remove(availability);
			await _context.SaveChangesAsync();
			return Ok();
		}

		[Authorize(Roles = "Doctor")]
		[HttpPost("availability")]
		public async Task<IActionResult> SaveDoctorsAvailabilityAsync([FromBody] DoctorsAvailabilityRequestDTO availabilityDTO)
		{
			if (availabilityDTO.From < DateTime.Now || availabilityDTO.From >= availabilityDTO.To)
				return BadRequest("Wrong availability period.");

			var user = await GetCurrentUserAsync();
			var availabilities = await _context.DoctorsAvailabilities
				.Where(x => x.DoctorsUserId == user.Id && availabilityDTO.From < x.To && x.From < availabilityDTO.To && (!availabilityDTO.Id.HasValue || x.Id != availabilityDTO.Id.Value))
				.ToListAsync();

			if (availabilities.Any())
				return BadRequest($"Availabilities cannot overlap.");

			if (availabilityDTO.Id.HasValue)
			{
				var availability = await _context.DoctorsAvailabilities.FirstOrDefaultAsync(x => x.Id == availabilityDTO.Id.Value);

				if (await _context.Appointments.AnyAsync(x => x.DoctorsUserId == user.Id && _cannotCancelAppointmentStatuses.Contains(x.Status) && availability.From <= x.Time && x.Time < availability.To && !(availabilityDTO.From <= x.Time && x.Time.AddMinutes(x.Service.DurationMinutes) <= availabilityDTO.To)))
					return BadRequest("Cancel accepted and confirmed appointments without availability first.");

				availability.From = availabilityDTO.From;
				availability.To = availabilityDTO.To;
			}
			else
			{
				var availability = _mapper.Map<DoctorsAvailability>(availabilityDTO);
				availability.DoctorsUserId = user.Id;
				await _context.DoctorsAvailabilities.AddAsync(availability);
			}

			await _context.SaveChangesAsync();
			return Ok();
		}
	}
}