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
			await _context.Patients.AddAsync(patient);
			await _context.SaveChangesAsync();
			return Ok(new
			{
				accessToken = _accountsService.GetAccessToken(user),
				user.RefreshToken
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
				IsAvailable = true,
				UserId = user.Id,
			};
			await _context.Doctors.AddAsync(doctor);
			await _context.SaveChangesAsync();
			return Ok(new
			{
				accessToken = _accountsService.GetAccessToken(user),
				user.RefreshToken
			});
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
				AccessToken = _accountsService.GetAccessToken(user),
				RefreshToken = user.RefreshToken,
				RoleType = user.Role.RoleType
			});
		}

		[AllowAnonymous]
		[HttpPost("refresh")]
		public async Task<IActionResult> RefreshTokenAsync([FromBody] string refreshTokenRequest)
		{
			var user = await _context.Users
				.Include(x => x.Role)
				.SingleOrDefaultAsync(x => x.RefreshToken == refreshTokenRequest);

			if (user == null)
				return Unauthorized("Invalid refresh token.");

			if (user.RefreshTokenExpiration.Value < DateTime.Now)
				return Unauthorized("Refresh token expired.");

			user.RefreshToken = _accountsService.GenerateSalt();
			await _context.SaveChangesAsync();

			return Ok(new
			{
				accessToken = _accountsService.GenerateAccessToken(user),
				refreshToken = user.RefreshToken
			});
		}

		[Authorize]
		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			Response.Cookies.Append("refreshToken", string.Empty, _accountsService.GetExpiredCookieOptions());
			Response.Cookies.Append("accessToken", string.Empty, _accountsService.GetExpiredCookieOptions());
			Response.Cookies.Append("roleType", string.Empty, _accountsService.GetExpiredCookieOptions());
			return Ok();
		}
	}
}