using MediCare.Models;
using MediCare.Services.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MediCare.Services
{
	public class AccountsService : IAccountsService
	{
		private readonly IConfiguration _configuration;

		public AccountsService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string GenerateSalt()
		{
			using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
			{
				byte[] salt = new byte[32];
				rng.GetBytes(salt);
				return Convert.ToBase64String(salt);
			}
		}

		public string HashPassword(string password, string salt)
		{
			return Convert.ToBase64String(KeyDerivation.Pbkdf2(
				password: password,
				salt: Encoding.UTF8.GetBytes(salt),
				prf: KeyDerivationPrf.HMACSHA1,
				iterationCount: 10000,
				numBytesRequested: 32));
		}

		public JwtSecurityToken GenerateAccessToken(User user)
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

		public string GetAccessToken(User user)
		{
			return new JwtSecurityTokenHandler().WriteToken(GenerateAccessToken(user));
		}

		public CookieOptions GetExpiredCookieOptions()
		{
			return new CookieOptions()
			{
				Expires = DateTime.Now.AddDays(-1),
				HttpOnly = true,
				Secure = true,
				SameSite = SameSiteMode.Strict
			};
		}
	}
}
