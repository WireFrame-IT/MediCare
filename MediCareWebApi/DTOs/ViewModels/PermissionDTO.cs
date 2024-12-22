namespace MediCare.DTOs.ViewModels
{
	public class PermissionDTO
	{
		public int Id { get; set; }
		public string Description { get; set; }
		public IEnumerable<RolePermissionDTO> RolePermissions = new List<RolePermissionDTO>();
	}
}
