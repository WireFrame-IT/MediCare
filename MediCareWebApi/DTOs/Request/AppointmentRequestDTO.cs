using System.ComponentModel.DataAnnotations;
using MediCare.Enums;

namespace MediCare.DTOs.Request
{
    public class AppointmentRequestDTO
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Appointment date is required")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date-time format")]
        public DateTime Time { get; set; }

        public AppointmentStatus? Status { get; set; }

        public int? DoctorsUserId { get; set; }

        public int? ServiceId { get; set; }

        [MaxLength(256)]
        public string? Diagnosis { get; set; }
	}
}
