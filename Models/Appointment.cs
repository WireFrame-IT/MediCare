using MediCare.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCare.Models
{
	public class Appointment : Entity
	{
		[Required]
		public DateTime Time { get; set; }

		[Required]
		public AppointmentStatus Status { get; set; }

		[Required]
		[MaxLength(256)]
		public string Diagnosis { get; set; }

		[Required]
		public int UserId { get; set; }

		[ForeignKey("UserId")]
		public User User { get; set; }

		[Required]
		public int DoctorId { get; set; }

		[ForeignKey("DoctorId")]
		public Doctor Doctor { get; set; }

		public int? ServiceId { get; set; }

		public Service? Service { get; set; }
	}
}
