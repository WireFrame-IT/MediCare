using System.ComponentModel.DataAnnotations;
using MediCare.Enums;

namespace MediCare.DTOs.Request
{
    public class UserRegisterRequestDTO : RegisterRequestDTO
    {
        [Required(ErrorMessage = "RoleType is required.")]
        public RoleType RoleType { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime? EmploymentDate { get; set; }

        public int? SpecialityId { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime? BirthDate { get; set; }
	}
}
