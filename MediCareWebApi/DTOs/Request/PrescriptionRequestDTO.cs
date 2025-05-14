using System.ComponentModel.DataAnnotations;

namespace MediCare.DTOs.Request
{
	public class PrescriptionRequestDTO
	{
		[Required(ErrorMessage = "Id wizyty jest wymagane.")]
		public int AppointmentId { get; set; }

		[Required(ErrorMessage = "Opis jest wymagany.")]
		[MaxLength(256, ErrorMessage = "Opis nie może przekroczyć 256 znaków.")]
		public required string Description { get; set; }

		[Required(ErrorMessage = "Data ważności jest wymagana.")]
		[DataType(DataType.Date, ErrorMessage = "Nieprawidłowy format daty.")]
		public DateTime ExpirationDate { get; set; }
	}
}
