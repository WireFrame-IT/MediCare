namespace MediCare.DTOs.ViewModels
{
	public class UserDTO
	{
		public int Id { get; set; }
		public string Email { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Pesel { get; set; }
		public string PhoneNumber { get; set; }
		public int RoleId { get; set; }
		public RoleDTO Role { get; set; }
	}
}
