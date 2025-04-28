using MediCare.Enums;

namespace MediCare.DTOs.Response
{
	public class LoginResponseDTO
	{
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string UserSurname { get; set; }
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
		public RoleType RoleType { get; set; }
	}
}
