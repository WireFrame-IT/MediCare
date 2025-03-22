using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCare.Models
{
	public class Feedback
	{
		[Required]
		public DateTime CreatedAt { get; set; }

		[Required]
		public string Description { get; set; }

		[Range(1, 5, ErrorMessage = "Rate must be between 1 and 5.")]
		public byte Rate { get; set; }

		[Key]
		public int PatientsUserId { get; set; }

		[ForeignKey("PatientsUserId")]
		public Patient Patient { get; set; }

		[Key]
		public int AppointmentId { get; set; }

		[ForeignKey("AppointmentId")]
		public Appointment Appointment { get; set; }
	}
}
