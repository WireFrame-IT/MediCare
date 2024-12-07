using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCare.Models
{
	public class User : Entity
	{
		[Required]
		[MaxLength(50)]
		public string Email { get; set; }

		[Required]
		[MaxLength(256)]
		public string Password { get; set; }

		[Required]
		[MaxLength(256)]
		public string Salt { get; set; }

		[Required]
		[MaxLength(50)]
		public string Name { get; set; }

		[Required]
		[MaxLength(50)]
		public string Surname { get; set; }

		[Required]
		[MaxLength(11)]
		[MinLength(11)]
		public string Pesel { get; set; }

		[Required]
		[MaxLength(15)]
		public string PhoneNumber { get; set; }

		[MaxLength(256)]
		public string? RefreshToken { get; set; }

		public DateTime? RefreshTokenExpiration { get; set; }

		[Required]
		public int RoleId { get; set; }

		[ForeignKey("RoleId")]
		public Role Role { get; set; }

		public ICollection<Log> Logs { get; set; } = new List<Log>();
	}
}
