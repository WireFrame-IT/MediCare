using MediCare.Enums;

namespace MediCare.DTOs.ViewModels
{
	public class AppointmentDTO
	{
		public int Id { get; set; }
		public DateTime Time { get; set; }
		public AppointmentStatus Status { get; set; }
		public string Diagnosis { get; set; }
		public int PatientId { get; set; }
		public PatientDTO Patient { get; set; }
		public int DoctorId { get; set; }
		public DoctorDTO Doctor { get; set; }
		public int ServiceId { get; set; }
		public ServiceDTO Service { get; set; }
	}
}
