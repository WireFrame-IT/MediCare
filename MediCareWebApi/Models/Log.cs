using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCare.Models
{
	public class Log : Entity
	{
		[Required]
		[MaxLength(256)]
		public string Action { get; set; }

		[Required]
		public DateTime CreatedAt { get; set; }

		[Required]
		[MaxLength(45)]
		public string IpAddress { get; set; }

		[Required]
		public string UserAgent { get; set; }

		[Required]
		public bool Success { get; set; }

		[Required]
		public int UserId { get; set; }

		[ForeignKey("UserId")]
		public User User { get; set; }
	}
}
