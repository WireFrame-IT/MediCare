using System.ComponentModel.DataAnnotations;
using MediCare.Enums;

namespace MediCare.Models
{
	public class Permission : Entity
	{
		[Required]
		[MaxLength(256)]
		public string Description { get; set; }

		[Required]
		public bool DoctorOnly { get; set; }

		[Required]
		public bool PatientOnly { get; set; }

		[Required]
		public PermissionType PermissionType { get; set; }

		public ICollection<RolePermission> PermissionRoles { get; set; } = new List<RolePermission>();
	}
}
