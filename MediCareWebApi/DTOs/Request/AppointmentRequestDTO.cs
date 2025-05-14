using System.ComponentModel.DataAnnotations;
using MediCare.Enums;

namespace MediCare.DTOs.Request
{
    public class AppointmentRequestDTO
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Data wizyty jest wymagana.")]
        [DataType(DataType.DateTime, ErrorMessage = "Nieprawidłowy format daty i godziny.")]
        public DateTime Time { get; set; }

        public AppointmentStatus? Status { get; set; }

        public int? DoctorsUserId { get; set; }

        public int? ServiceId { get; set; }

        [MaxLength(256)]
        public string? Diagnosis { get; set; }
	}
}
