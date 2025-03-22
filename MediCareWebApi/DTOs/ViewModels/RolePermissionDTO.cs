namespace MediCare.DTOs.ViewModels
{
	public class RolePermissionDTO
	{
		public int RoleId { get; set; }
		public RoleDTO Role { get; set; }
		public int PermissionId { get; set; }
		public PermissionDTO Permission { get; set; }
	}
}
