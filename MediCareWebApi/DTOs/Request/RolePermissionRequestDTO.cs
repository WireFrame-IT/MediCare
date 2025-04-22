using System.ComponentModel.DataAnnotations;
using MediCare.Enums;

namespace MediCare.DTOs.Request
{
	public class RolePermissionRequestDTO
	{
		[Required(ErrorMessage = "RoleType is required.")]
		public RoleType RoleType { get; set; }

		[Required(ErrorMessage = "Permission id is required.")]
		public int PermissionId { get; set; }
	}
}
