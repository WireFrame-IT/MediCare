using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediCare.Models
{
	public class RolePermission
	{
		[Key]
		[Column(Order = 1)]
		public int RoleId { get; set; }

		[ForeignKey("RoleId")]
		public Role Role { get; set; }

		[Key]
		[Column(Order = 2)]
		public int PermissionId { get; set; }

		[ForeignKey("PermissionId")]
		public Permission Permission { get; set; }
	}
}
