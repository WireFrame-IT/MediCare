using System.ComponentModel.DataAnnotations;

namespace MediCare.DTOs
{
	public class RegisterRequestDTO
	{
		[Required(ErrorMessage = "Name is required")]
		[StringLength(50, ErrorMessage = "Name must not exceed 50 characters.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Surname is required")]
		[StringLength(50, ErrorMessage = "Surname must not exceed 50 characters.")]
		public string Surname { get; set; }

		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid email format")]
		[StringLength(50, ErrorMessage = "Email must not exceed 50 characters.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[StringLength(256, ErrorMessage = "Password must not exceed 256 characters.")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Pesel is required")]
		[StringLength(11, MinimumLength = 11, ErrorMessage = "Pesel must be exactly 11 characters.")]
		public string Pesel { get; set; }

		[Required(ErrorMessage = "Phone number is required")]
		[Phone(ErrorMessage = "Invalid phone number")]
		[StringLength(15, ErrorMessage = "Phone number must not exceed 15 characters.")]
		public string PhoneNumber { get; set; }

		[Required(ErrorMessage = "Date of birth is required")]
		[DataType(DataType.Date)]
		public DateTime BirthDate { get; set; }
	}
}
