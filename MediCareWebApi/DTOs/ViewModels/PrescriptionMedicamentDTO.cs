using MediCare.Enums;

namespace MediCare.DTOs.ViewModels
{
	public class PrescriptionMedicamentDTO
	{
		public string Dosage { get; set; }

		public int Quantity { get; set; }

		public MedicamentUnit MedicamentUnit { get; set; }

		public string? Notes { get; set; }

		public int PrescriptionAppointmentId { get; set; }

		public PrescriptionDTO Prescription { get; set; }

		public int MedicamentId { get; set; }

		public MedicamentDTO Medicament { get; set; }
	}
}
