using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCare.Models
{
	public class Feedback
	{
		[Required]
		public DateTime CreatedAt { get; set; }

		[Required]
		[MaxLength(256)]
		public string Description { get; set; }

		[Range(1, 5)]
		public byte Rate { get; set; }

		[Key]
		public int AppointmentId { get; set; }

		[ForeignKey("AppointmentId")]
		public Appointment Appointment { get; set; }
	}
}
