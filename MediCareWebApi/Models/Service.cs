using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCare.Models
{
	public class Service : Entity
	{
		[Required]
		[MaxLength(50)]
		public string Name { get; set; }

		[Required]
		[MaxLength(256)]
		public string Description { get; set; }

		[Required]
		public decimal Price { get; set; }

		[Required]
		public int DurationMinutes { get; set; }

		[Required]
		public int SpecialityId { get; set; }

		[ForeignKey("SpecialityId")]
		public Speciality Speciality { get; set; }
	}
}
