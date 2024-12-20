using System.ComponentModel.DataAnnotations;

namespace MediCare.DTOs.Request
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(50, ErrorMessage = "Email must not exceed 50 characters.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(256, ErrorMessage = "Password must not exceed 256 characters.")]
        public required string Password { get; set; }
    }
}
