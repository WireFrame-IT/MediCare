using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediCare.Enums;

namespace MediCare.Models
{
	public class PrescriptionMedicament
	{
		[Required]
		[MaxLength(256)]
		public string Dosage { get; set; }

		[Required]
		public int Quantity { get; set; }

		[Required]
		public MedicamentUnit MedicamentUnit { get; set; }

		public string? Notes { get; set; }

		[Key]
		[Column(Order = 1)]
		public int PrescriptionAppointmentId { get; set; }

		[ForeignKey("PrescriptionAppointmentId")]
		public Prescription Prescription { get; set; }

		[Key]
		[Column(Order = 2)]
		public int MedicamentId { get; set; }

		[ForeignKey("MedicamentId")]
		public Medicament Medicament { get; set; }
	}
}
