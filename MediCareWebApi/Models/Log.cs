using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCare.Models
{
	public class Log : Entity
	{
		[Required]
		[MaxLength(20)]
		public string Method { get; set; }

		[Required]
		[MaxLength(256)]
		public string Path { get; set; }

		[Required]
		public int StatusCode { get; set; }

		[Required]
		public DateTime CreatedAt { get; set; }

		[Required]
		[MaxLength(45)]
		public string IpAddress { get; set; }

		[Required]
		[MaxLength(512)]
		public string UserAgent { get; set; }

		[Required]
		public bool Success { get; set; }

		[MaxLength(512)]
		public string QueryString { get; set; }

		[MaxLength(1024)]
		public string? Payload { get; set; }

		[MaxLength(512)]
		public string? ErrorMessage { get; set; }

		public int? UserId { get; set; }

		[ForeignKey("UserId")]
		public User? User { get; set; }
	}
}
