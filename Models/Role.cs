using System.ComponentModel.DataAnnotations;
using MediCare.Enums;

namespace MediCare.Models
{
	public class Role : Entity
	{
		[Required]
		public RoleType RoleType { get; set; }

		public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
	}
}
