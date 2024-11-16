using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCare.Models
{
	public class Doctor : Entity
	{
		[Required]
		public DateTime EmploymentDate { get; set; }

		[Required]
		public bool IsAvailable { get; set; }

		[Required]
		public int SpecialtyId { get; set; }

		[ForeignKey("SpecialtyId")]
		public Speciality Speciality { get; set; }

		[Required]
		public int UserId { get; set; }

		[ForeignKey("UserId")]
		public User User { get; set; }

		public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
	}
}
