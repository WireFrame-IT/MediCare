using System.ComponentModel.DataAnnotations;

namespace MediCare.DTOs.Request
{
    public abstract class RegisterRequestDTO : UserBasicDataDTO
    {
		[Required(ErrorMessage = "Hasło jest wymagane.")]
		[MaxLength(256, ErrorMessage = "Hasło nie może przekroczyć 256 znaków.")]
		public required string Password { get; set; }
    }
}
