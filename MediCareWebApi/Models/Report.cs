using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCare.Models
{
	public class Report : Entity
	{
		[Required]
		[MaxLength(50)]
		public string Name { get; set; }

		[Required]
		public DateTime CreatedAt { get; set; }

		[Required]
		public int AdminId { get; set; }

		[Required]
		public byte[] Content { get; set; }

		[ForeignKey("AdminId")]
		public Admin Admin { get; set; }
	}
}
