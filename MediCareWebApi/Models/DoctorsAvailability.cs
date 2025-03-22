using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCare.Models
{
	public class DoctorsAvailability : Entity
	{
		[Required]
		public DateTime From { get; set; }

		public DateTime? To { get; set; }

		[Required]
		public int DoctorsUserId { get; set; }

		[ForeignKey("DoctorsUserId")]
		public Doctor Doctor { get; set; }
	}
}
