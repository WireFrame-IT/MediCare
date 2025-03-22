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

		[MaxLength(256)]
		public string? Diagnosis { get; set; }

		[Required]
		public int PatientsUserId { get; set; }

		[ForeignKey("PatientsUserId")]
		public Patient Patient { get; set; }

		[Required]
		public int DoctorsUserId { get; set; }

		[ForeignKey("DoctorsUserId")]
		public Doctor Doctor { get; set; }

		[Required]
		public int ServiceId { get; set; }

		[ForeignKey("ServiceId")]
		public Service Service { get; set; }

		public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
	}
}
