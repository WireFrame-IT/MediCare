using System.ComponentModel.DataAnnotations;

namespace MediCare.DTOs.Request
{
    public abstract class RegisterRequestDTO : UserBasicDataDTO
    {
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(256, ErrorMessage = "Password must not exceed 256 characters.")]
        public required string Password { get; set; }
    }
}
