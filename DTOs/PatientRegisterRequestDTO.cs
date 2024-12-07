using System.ComponentModel.DataAnnotations;

namespace MediCare.DTOs
{
	public class PatientRegisterRequestDTO : RegisterRequestDTO
	{
		[Required(ErrorMessage = "Date of birth is required")]
		[DataType(DataType.Date, ErrorMessage = "Invalid date format")]
		public DateTime BirthDate { get; set; }
	}
}
