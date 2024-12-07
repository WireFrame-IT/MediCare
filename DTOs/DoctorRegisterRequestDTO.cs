using System.ComponentModel.DataAnnotations;

namespace MediCare.DTOs
{
	public class DoctorRegisterRequestDTO : RegisterRequestDTO
	{
		[Required(ErrorMessage = "Employment date is required")]
		[DataType(DataType.Date, ErrorMessage = "Invalid date format")]
		public DateTime EmploymentDate { get; set; }

		[Required(ErrorMessage = "Speciality is required")]
		public int SpecialityId { get; set; }
	}
}
