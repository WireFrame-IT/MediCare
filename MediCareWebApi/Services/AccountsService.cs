using MediCare.Models;
using MediCare.Services.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Caching.Memory;
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
		private readonly IMemoryCache _memoryCache;

		public AccountsService(IConfiguration configuration, IMemoryCache memoryCache)
		{
			_configuration = configuration;
			_memoryCache = memoryCache;
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

		public string GenerateAccessToken(User user)
		{
			var secretKey = _configuration.GetSection("JwtSettings:SecretKey").Value;
			if (string.IsNullOrEmpty(secretKey))
			{
				throw new InvalidOperationException("Klucz tajny nie jest ustawiony.");
			}

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
			var claims = new List<Claim>
			{
				new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new(ClaimTypes.Role, user.Role.RoleType.ToString()),
				new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
				issuer: _configuration.GetSection("JwtSettings:ValidIssuer").Value,
				audience: _configuration.GetSection("JwtSettings:ValidAudience").Value,
				claims: claims,
				expires: DateTime.Now.AddMinutes(double.Parse(_configuration.GetSection("JwtSettings:AccessTokenExpirationMinutes").Value)),
				signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
			));
		}

		public void BlacklistToken(string jti)
		{
			var expirationMinutes = TimeSpan.FromMinutes(double.Parse(_configuration.GetSection("JwtSettings:AccessTokenExpirationMinutes").Value));
			_memoryCache.Set(jti, true, expirationMinutes);
		}

		public bool IsTokenBlacklisted(string jti)
		{
			return _memoryCache.TryGetValue(jti, out _);
		}
	}
}
