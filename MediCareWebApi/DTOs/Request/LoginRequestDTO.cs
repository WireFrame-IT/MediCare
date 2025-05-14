using System.ComponentModel.DataAnnotations;

namespace MediCare.DTOs.Request
{
    public class LoginRequestDTO
    {
		[Required(ErrorMessage = "Adres e-mail jest wymagany.")]
		[EmailAddress(ErrorMessage = "Nieprawidłowy format adresu e-mail.")]
		[MaxLength(50, ErrorMessage = "Adres e-mail nie może przekroczyć 50 znaków.")]
		public required string Email { get; set; }

		[Required(ErrorMessage = "Hasło jest wymagane.")]
		[MaxLength(256, ErrorMessage = "Hasło nie może przekroczyć 256 znaków.")]
		public required string Password { get; set; }
    }
}
