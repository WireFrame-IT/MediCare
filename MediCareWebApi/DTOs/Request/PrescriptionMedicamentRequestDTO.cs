using MediCare.Enums;

namespace MediCare.DTOs.Request
{
	public class PrescriptionMedicamentRequestDTO
	{
		public string Dosage { get; set; }

		public int Quantity { get; set; }

		public MedicamentUnit MedicamentUnit { get; set; }

		public int PrescriptionAppointmentId { get; set; }

		public int MedicamentId { get; set; }	

		public string? Notes { get; set; }
	}
}
