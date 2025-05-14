using System.ComponentModel.DataAnnotations;

namespace MediCare.Models
{
	public class Speciality : Entity
	{
		[Required]
		[MaxLength(50)]
		public string Name { get; set; }

		[Required]
		[MaxLength(256)]
		public string Description { get; set; }
	}
}
