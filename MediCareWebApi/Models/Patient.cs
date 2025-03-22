using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCare.Models
{
	public class Patient
	{
		[Key]
		public int UserId { get; set; }

		[ForeignKey("UserId")]
		public User User { get; set; }

		[Required]
		public DateTime RegisterDate { get; set; }

		[Required]
		public DateTime BirthDate { get; set; }

		[MaxLength(20)]
		public string? PatientCard { get; set; }

		public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
		public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
	}
}
