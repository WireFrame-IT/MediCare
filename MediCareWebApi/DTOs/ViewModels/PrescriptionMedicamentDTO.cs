namespace MediCare.DTOs.ViewModels
{
	public class PrescriptionMedicamentDTO
	{
		public string Dosage { get; set; }

		public int Quantity { get; set; }

		public string? Notes { get; set; }

		public int PrescriptionId { get; set; }

		public int MedicamentId { get; set; }
	}
}
