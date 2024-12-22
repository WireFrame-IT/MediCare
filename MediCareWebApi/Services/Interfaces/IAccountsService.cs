using System.IdentityModel.Tokens.Jwt;
using MediCare.Models;

namespace MediCare.Services.Interfaces
{
    public interface IAccountsService
    {
	    string GenerateSalt();
	    string HashPassword(string password, string salt);
	    string GenerateAccessToken(User user);
	    void BlacklistToken(string jti);
	    bool IsTokenBlacklisted(string jti);
	}
}
