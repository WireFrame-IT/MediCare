using System.ComponentModel.DataAnnotations;
using MediCare.Enums;

namespace MediCare.DTOs.Request
{
    public class UserRegisterRequestDTO : RegisterRequestDTO
    {
		[Required(ErrorMessage = "Typ roli jest wymagany.")]
		public RoleType RoleType { get; set; }

		[DataType(DataType.Date, ErrorMessage = "Nieprawidłowy format daty.")]
		public DateTime? EmploymentDate { get; set; }

		public int? SpecialityId { get; set; }

		[DataType(DataType.Date, ErrorMessage = "Nieprawidłowy format daty.")]
		public DateTime? BirthDate { get; set; }
	}
}
