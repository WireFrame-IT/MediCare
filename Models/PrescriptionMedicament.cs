using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCare.Models
{
	public class PrescriptionMedicament : Entity
	{
		[Required]
		[MaxLength(256)]
		public string Dosage { get; set; }

		[Required]
		public int Quantity { get; set; }

		[Required]
		public string Notes { get; set; }

		[Required]
		public int PrescriptionId { get; set; }

		[ForeignKey("PrescriptionId")]
		public Prescription Prescription { get; set; }

		[Required]
		public int MedicamentId { get; set; }

		[ForeignKey("MedicamentId")]
		public Medicament Medicament { get; set; }
	}
}
