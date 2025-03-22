using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCare.Models
{
	public class Doctor
	{
		[Key]
		public int UserId { get; set; }

		[ForeignKey("UserId")]
		public User User { get; set; }

		[Required]
		public DateTime EmploymentDate { get; set; }

		[Required]
		public int SpecialityId { get; set; }

		[ForeignKey("SpecialityId")]
		public Speciality Speciality { get; set; }

		public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

		public ICollection<DoctorsAvailability> DoctorsAvailabilities { get; set; } = new List<DoctorsAvailability>();
	}
}
