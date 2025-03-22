using MediCare.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCare.Models
{
	public class Admin
	{
		[Key]
		public int UserId { get; set; }

		[ForeignKey("UserId")]
		public User User { get; set; }

		[Required]
		public DateTime LastLogin { get; set; }

		[Required]
		public AdminStatus Status { get; set; }
	}
}
