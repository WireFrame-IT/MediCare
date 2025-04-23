using System.ComponentModel.DataAnnotations;

namespace MediCare.DTOs.Request
{
	public class DoctorsAvailabilityRequestDTO
	{
		public int? Id { get; set; }

		[Required(ErrorMessage = "Date 'From' is required.")]
		[DataType(DataType.DateTime, ErrorMessage = "Invalid date-time format.")]
		public DateTime From { get; set; }

		[DataType(DataType.DateTime, ErrorMessage = "Invalid date-time format.")]
		public DateTime? To { get; set; }
	}
}
