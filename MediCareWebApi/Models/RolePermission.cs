using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediCare.Models
{
	public class RolePermission : Entity
	{
		[Required]
		public int RoleId { get; set; }

		[ForeignKey("RoleId")]
		public Role Role { get; set; }

		[Required]
		public int PermissionId { get; set; }

		[ForeignKey("PermissionId")]
		public Permission Permission { get; set; }
	}
}
