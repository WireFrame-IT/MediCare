using System.ComponentModel.DataAnnotations;
using MediCare.Enums;

namespace MediCare.Models
{
	public class Medicament : Entity
	{
		[Required]
		[MaxLength(256)]
		public string Name { get; set; }

		[Required]
		[MaxLength(256)]
		public string Description { get; set; }

		[Required]
		public MedicamentType MedicamentType { get; set; }

		[Required]
		public bool PrescriptionRequired { get; set; }

		public ICollection<PrescriptionMedicament> MedicamentPrescriptions { get; set; } = new List<PrescriptionMedicament>();
	}
}
