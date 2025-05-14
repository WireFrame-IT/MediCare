using System.ComponentModel.DataAnnotations;

namespace MediCare.DTOs.Request
{
	public class DoctorsAvailabilityRequestDTO
	{
		public int? Id { get; set; }

		[Required(ErrorMessage = "Wymagana jest data 'Od'.")]
		[DataType(DataType.DateTime, ErrorMessage = "Nieprawidłowy format daty i godziny.")]
		public DateTime From { get; set; }

		[DataType(DataType.DateTime, ErrorMessage = "Nieprawidłowy format daty i godziny.")]
		public DateTime? To { get; set; }
	}
}
