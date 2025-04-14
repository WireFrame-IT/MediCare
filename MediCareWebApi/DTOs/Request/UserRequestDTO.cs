using System.ComponentModel.DataAnnotations;

namespace MediCare.DTOs.Request
{
	public class UserRequestDTO : UserBasicDataDTO
	{
		public int Id { get; set; }

		[StringLength(256, ErrorMessage = "Password must not exceed 256 characters.")]
		public string? Password { get; set; }

		public int? SpecialityId { get; set; }

		[DataType(DataType.Date, ErrorMessage = "Invalid date format")]
		public DateTime? BirthDate { get; set; }
	}
}
