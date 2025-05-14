using System.ComponentModel.DataAnnotations;
using MediCare.Enums;

namespace MediCare.DTOs.Request
{
	public class RolePermissionRequestDTO
	{
		[Required(ErrorMessage = "Typ roli jest wymagany.")]
		public RoleType RoleType { get; set; }

		[Required(ErrorMessage = "Id uprawnienia jest wymagane.")]
		public int PermissionId { get; set; }
	}
}
