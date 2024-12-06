using System.IdentityModel.Tokens.Jwt;
using MediCare.Models;

namespace MediCare.Services.Interfaces
{
    public interface IAccountsService
    {
	    string GeneratePatientCard(string pesel);
	    string GenerateSalt();
	    string HashPassword(string password, string salt);
	    JwtSecurityToken GenerateAccessToken(User user);
	}
}
