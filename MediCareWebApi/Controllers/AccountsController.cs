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
				BirthDate = registerRequest.BirthDate,
				UserId = user.Id,
			};

			if (patient.BirthDate > DateTime.Now)
				throw new InvalidOperationException("Wrong birth date.");

			await _context.Patients.AddAsync(patient);
			await _context.SaveChangesAsync();
			return Ok(new RegisterResponseDTO()
			{
				AccessToken = _accountsService.GenerateAccessToken(user),
				RefreshToken = user.RefreshToken
			});
		}

		[Authorize(Roles = "Admin")]
		[HttpPost("register-doctor")]
		public async Task<IActionResult> DoctorRegisterAsync([FromBody] DoctorRegisterRequestDTO registerRequest)
		{
			var user = await RegisterUserAsync(registerRequest, RoleType.Doctor);
			var doctor = new Doctor()
			{
				EmploymentDate = registerRequest.EmploymentDate,
				SpecialityId = registerRequest.SpecialityId,
				UserId = user.Id,
			};
			await _context.Doctors.AddAsync(doctor);
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

			return Ok(new RegisterResponseDTO()
			{
				AccessToken = _accountsService.GenerateAccessToken(user),
				RefreshToken = user.RefreshToken
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
		[HttpGet("specialities")]
		public async Task<IActionResult> GetSpecialities()
		{
			return Ok(_mapper.Map<List<SpecialityDTO>>(await _context.Specialities.ToListAsync()));
		}
	}
}