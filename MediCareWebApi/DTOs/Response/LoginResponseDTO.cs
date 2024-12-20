using MediCare.Enums;

namespace MediCare.DTOs.Response
{
	public class LoginResponseDTO
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
		public RoleType RoleType { get; set; }
	}
}
