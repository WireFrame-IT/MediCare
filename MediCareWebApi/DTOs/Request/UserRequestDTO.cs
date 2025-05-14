using System.ComponentModel.DataAnnotations;

namespace MediCare.DTOs.Request
{
	public class UserRequestDTO : UserBasicDataDTO
	{
		public int Id { get; set; }

		[MaxLength(256, ErrorMessage = "Hasło nie może przekroczyć 256 znaków.")]
		public string? Password { get; set; }

		public int? SpecialityId { get; set; }

		[DataType(DataType.Date, ErrorMessage = "Nieprawidłowy format daty.")]
		public DateTime? BirthDate { get; set; }
	}
}
