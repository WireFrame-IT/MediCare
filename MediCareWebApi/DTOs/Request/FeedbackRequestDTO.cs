using System.ComponentModel.DataAnnotations;

namespace MediCare.DTOs.Request
{
	public class FeedbackRequestDTO
	{
		[Required(ErrorMessage = "Ocena jest wymagana.")]
		[Range(1, 5, ErrorMessage = "Ocena musi być między 1 a 5.")]
		public byte Rate { get; set; }

		[Required(ErrorMessage = "Opis jest wymagany.")]
		[MaxLength(256, ErrorMessage = "Opis nie może przekroczyć 256 znaków.")]
		public string Description { get; set; }

		[Required(ErrorMessage = "Id wizyty jest wymagane.")]
		public int AppointmentId { get; set; }
	}
}
