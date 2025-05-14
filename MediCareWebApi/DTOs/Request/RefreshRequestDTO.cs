using System.ComponentModel.DataAnnotations;

namespace MediCare.DTOs.Request
{
	public class RefreshRequestDTO
	{
		[Required(ErrorMessage = "Token odświeżania jest wymagany.")]
		[MaxLength(256, ErrorMessage = "Token odświeżania nie może przekroczyć 256 znaków.")]
		public required string RefreshToken { get; set; }
	}
}
