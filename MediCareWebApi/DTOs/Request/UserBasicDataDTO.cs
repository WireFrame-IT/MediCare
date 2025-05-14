using System.ComponentModel.DataAnnotations;

namespace MediCare.DTOs.Request
{
	public class UserBasicDataDTO
	{
		[Required(ErrorMessage = "Imię jest wymagane.")]
		[MaxLength(50, ErrorMessage = "Imię nie może przekroczyć 50 znaków.")]
		public required string Name { get; set; }

		[Required(ErrorMessage = "Nazwisko jest wymagane.")]
		[MaxLength(50, ErrorMessage = "Nazwisko nie może przekroczyć 50 znaków.")]
		public required string Surname { get; set; }

		[Required(ErrorMessage = "Adres e-mail jest wymagany.")]
		[EmailAddress(ErrorMessage = "Nieprawidłowy format adresu e-mail.")]
		[MaxLength(50, ErrorMessage = "Adres e-mail nie może przekroczyć 50 znaków.")]
		public required string Email { get; set; }

		[Required(ErrorMessage = "Pesel jest wymagany.")]
		[StringLength(11, MinimumLength = 11, ErrorMessage = "Pesel musi mieć dokładnie 11 znaków.")]
		public required string Pesel { get; set; }

		[Required(ErrorMessage = "Numer telefonu jest wymagany.")]
		[Phone(ErrorMessage = "Nieprawidłowy numer telefonu.")]
		[MaxLength(15, ErrorMessage = "Numer telefonu nie może przekroczyć 15 znaków.")]
		public required string PhoneNumber { get; set; }
	}
}
