using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCare.Models
{
	public class Prescription
	{
		[Required]
		public DateTime IssueDate { get; set; }

		[Required]
		public DateTime ExpirationDate { get; set; }

		[Required]
		[MaxLength(256)]
		public string Description { get; set; }

		[Key]
		[Required]
		public int AppointmentId { get; set; }

		[ForeignKey("AppointmentId")]
		public Appointment Appointment { get; set; }

		public ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; } = new List<PrescriptionMedicament>();
	}
}
