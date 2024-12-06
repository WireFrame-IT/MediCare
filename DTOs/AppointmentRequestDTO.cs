using System.ComponentModel.DataAnnotations;

namespace MediCare.DTOs
{
	public class AppointmentRequestDTO
	{
		[Required(ErrorMessage = "Appointment date is required")]
		[DataType(DataType.DateTime, ErrorMessage = "Invalid date-time format")]
		public DateTime Time { get; set; }

		public int? DoctorId { get; set; }

		public int? ServiceId { get; set; }
	}
}
