using MediCare.Enums;
using System.ComponentModel.DataAnnotations;

namespace MediCare.DTOs.Request
{
	public class PrescriptionMedicamentRequestDTO
	{
		[Required(ErrorMessage = "Dawkowanie jest wymagane.")]
		[MaxLength(256, ErrorMessage = "Dawkowanie nie może przekroczyć 256 znaków.")]
		public string Dosage { get; set; }

		[Required(ErrorMessage = "Ilość jest wymagana.")]
		public int Quantity { get; set; }

		[Required(ErrorMessage = "Jednostka miary jest wymagana.")]
		public MedicamentUnit MedicamentUnit { get; set; }

		[Required(ErrorMessage = "Id wizyty jest wymagane.")]
		public int PrescriptionAppointmentId { get; set; }

		[Required(ErrorMessage = "Id leku jest wymagane.")]
		public int MedicamentId { get; set; }

		[MaxLength(256, ErrorMessage = "Uwagi nie mogą przekroczyć 256 znaków.")]
		public string? Notes { get; set; }
	}
}
