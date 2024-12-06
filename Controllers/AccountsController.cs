using AutoMapper;
using MediCare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using MediCare.Controllers;
using MediCare.DTOs;
using MediCare.Enums;
using MediCare.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalFacility.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class AccountsController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IAccountsService _accountsService;

		public AccountsController(MediCareDbContext context, IConfiguration configuration, IMapper mapper, IAccountsService accountsService)
			: base(context, configuration)
		{
			_mapper = mapper;
			_accountsService = accountsService;
		}

		[AllowAnonymous]
		[HttpPost("register")]
		public async Task<IActionResult> PatientRegister([FromBody] RegisterRequestDTO registerRequest)
		{
			var user = _mapper.Map<User>(registerRequest);
			user.Role = await _context.Roles.FirstAsync(x => x.RoleType == RoleType.Patient);
			user.Password = _accountsService.HashPassword(registerRequest.Password, user.Salt = _accountsService.GenerateSalt());
			await _context.Users.AddAsync(user);
			await _context.SaveChangesAsync();

			var patient = new Patient()
			{
				RegisterDate = DateTime.Now,
				BirthDate = registerRequest.BirthDate,
				PatientCard = _accountsService.GeneratePatientCard(registerRequest.Pesel),
				UserId = user.Id,

			};
			await _context.Patients.AddAsync(patient);
			await _context.SaveChangesAsync();
			return Ok(user.Id);
		}

		[AllowAnonymous]
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
		{
			var user = await _context.Users
				.Include(x => x.Role)
				.FirstOrDefaultAsync(x => x.Email == loginRequest.Email);

			if (user == null || _accountsService.HashPassword(loginRequest.Password, user.Salt) != user.Password)
				return Unauthorized("Invalid email or password.");

			user.RefreshToken = _accountsService.GenerateSalt();
			user.RefreshTokenExpiration = DateTime.Now.AddHours(1);
			await _context.SaveChangesAsync();

			return Ok(new
			{
				accessToken = new JwtSecurityTokenHandler().WriteToken(_accountsService.GenerateAccessToken(user)),
				user.RefreshToken
			});
		}

		[AllowAnonymous]
		[HttpPost("refresh")]
		public async Task<IActionResult> RefreshToken([FromBody] string refreshTokenRequest)
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
	}
}