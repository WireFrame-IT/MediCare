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
				AccessToken = _accountsService.GenerateAccessToken(user),
				RefreshToken = user.RefreshToken,
				RoleType = user.Role.RoleType
			});
		}

		[Authorize]
		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
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
		public async Task<IActionResult> SaveUser([FromBody] UserRequestDTO userRequestDTO)
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

			var doctor = await _context.Doctors
				.Include(x => x.User)
				.Include(x => x.Speciality)
				.FirstOrDefaultAsync(x => x.UserId == user.Id);
			var patient = await _context.Patients
				.Include(x => x.User)
				.FirstOrDefaultAsync(x => x.UserId == user.Id);
			if (doctor == null && patient == null)
				throw new InvalidOperationException("The user is neither a patient nor a doctor.");

			if (doctor != null && userRequestDTO.SpecialityId.HasValue)
				doctor.SpecialityId = userRequestDTO.SpecialityId.Value;

			if (patient != null && userRequestDTO.BirthDate.HasValue)
				patient.BirthDate = userRequestDTO.BirthDate.Value;

			await _context.SaveChangesAsync();
			return Ok(doctor == null ? patient : doctor);
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("specialities")]
		public async Task<IActionResult> GetSpecialities()
		{
			return Ok(_mapper.Map<List<SpecialityDTO>>(await _context.Specialities.OrderBy(x => x.Name).ToListAsync()));
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("permissions")]
		public async Task<IActionResult> GetPermissions()
		{
			var permissions = await _context.Permissions
				.Include(x => x.PermissionRoles)
					.ThenInclude(x => x.Role)
				.OrderBy(x => x.Description)
				.ToListAsync();

			return Ok(_mapper.Map<List<PermissionDTO>>(permissions));
		}

		[Authorize(Roles = "Admin")]
		[HttpPost("role-permission")]
		public async Task<IActionResult> SaveRolePermission([FromBody] RolePermissionRequestDTO requestDTO)
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
		public async Task<IActionResult> DeleteRolePermission(RoleType roleType, int permissionId)
		{
			var rolePermission = await _context.RolePermissions.FirstOrDefaultAsync(x => x.Role.RoleType == roleType && x.PermissionId == permissionId);
			if (rolePermission == null)
				return BadRequest("Role permission does not exist.");

			_context.RolePermissions.Remove(rolePermission);
			await _context.SaveChangesAsync();
			return Ok(rolePermission);
		}
	}
}