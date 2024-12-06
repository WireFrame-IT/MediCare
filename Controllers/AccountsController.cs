using AutoMapper;
using MediCare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MediCare.DTOs;
using MediCare.Enums;
using Microsoft.EntityFrameworkCore;

namespace MedicalFacility.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class AccountsController : Controller
	{
		private readonly MediCareDbContext _context;
		private readonly IConfiguration _configuration;
		private readonly IMapper _mapper;

		public AccountsController(MediCareDbContext context, IConfiguration configuration, IMapper mapper)
		{
			_context = context;
			_configuration = configuration;
			_mapper = mapper;
		}

		[AllowAnonymous]
		[HttpPost("register")]
		public async Task<IActionResult> PatientRegister([FromBody] RegisterRequestDTO registerRequest)
		{
			var user = _mapper.Map<User>(registerRequest);
			user.Role = await _context.Roles.FirstAsync(x => x.RoleType == RoleType.Patient);
			user.Password = HashPassword(registerRequest.Password, user.Salt = GenerateSalt());
			await _context.Users.AddAsync(user);
			await _context.SaveChangesAsync();

			var patient = new Patient()
			{
				RegisterDate = DateTime.Now,
				BirthDate = registerRequest.BirthDate,
				PatientCard = GeneratePatientCard(registerRequest.Pesel),
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

			if (user == null || HashPassword(loginRequest.Password, user.Salt) != user.Password)
				return Unauthorized("Invalid email or password.");

			user.RefreshToken = GenerateSalt();
			user.RefreshTokenExpiration = DateTime.Now.AddHours(1);
			await _context.SaveChangesAsync();

			return Ok(new
			{
				accessToken = new JwtSecurityTokenHandler().WriteToken(GenerateAccessToken(user)),
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

			user.RefreshToken = GenerateSalt();
			await _context.SaveChangesAsync();

			return Ok(new
			{
				accessToken = GenerateAccessToken(user),
				refreshToken = user.RefreshToken
			});
		}

		private string GeneratePatientCard(string pesel)
		{
			var cardBase = pesel.Substring(0, 6);
			var uniqueSuffix = DateTime.Now.Ticks.ToString().Substring(0, 14);
			return $"{cardBase}{uniqueSuffix}";
		}

		private JwtSecurityToken GenerateAccessToken(User user)
		{
			var secretKey = _configuration.GetSection("JwtSettings:SecretKey").Value;
			if (string.IsNullOrEmpty(secretKey))
			{
				throw new InvalidOperationException("SecretKey is not set.");
			}

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
			var claims = new List<Claim>
			{
				new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new(ClaimTypes.Role, user.Role.RoleType.ToString())
			};

			return new JwtSecurityToken(
				issuer: _configuration.GetSection("JwtSettings:ValidIssuer").Value,
				audience: _configuration.GetSection("JwtSettings:ValidAudience").Value,
				claims: claims,
				expires: DateTime.Now.AddMinutes(5),
				signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
			);
		}

		private static string GenerateSalt()
		{
			using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
			{
				byte[] salt = new byte[32];
				rng.GetBytes(salt);
				return Convert.ToBase64String(salt);
			}
		}

		private static string HashPassword(string password, string salt)
		{
			return Convert.ToBase64String(KeyDerivation.Pbkdf2(
				password: password,
				salt: Encoding.UTF8.GetBytes(salt),
				prf: KeyDerivationPrf.HMACSHA1,
				iterationCount: 10000,
				numBytesRequested: 32));
		}
	}
}