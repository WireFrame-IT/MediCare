using System.ComponentModel.DataAnnotations;

namespace MediCare.Models
{
	public class Permission : Entity
	{
		[Required]
		[MaxLength(256)]
		public string Description { get; set; }

		public ICollection<RolePermission> PermissionRoles { get; set; } = new List<RolePermission>();
	}
}
