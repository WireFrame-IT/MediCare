using MediCare.Enums;

namespace MediCare.DTOs.ViewModels
{
	public class RoleDTO
	{
		public int Id { get; set; }
		public RoleType RoleType { get; set; }
		public IEnumerable<RolePermissionDTO> RolePermissions { get; set; } = new List<RolePermissionDTO>();
	}
}
