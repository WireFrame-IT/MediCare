using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCare.Models
{
	public class Prescription : Entity
	{
		[Required]
		public DateTime IssueDate { get; set; }

		[Required]
		public string Description { get; set; }

		[Required]
		public int AppointmentId { get; set; }

		[ForeignKey("AppointmentId")]
		public Appointment Appointment { get; set; }

		public ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; } = new List<PrescriptionMedicament>();
	}
}
