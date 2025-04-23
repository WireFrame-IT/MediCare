using MediCare.Enums;

namespace MediCare.DTOs.ViewModels
{
	public class ReducedAppointmentDTO
	{
		public int Id { get; set; }
		public DateTime Time { get; set; }
		public AppointmentStatus Status { get; set; }
		public string Diagnosis { get; set; }
		public int PatientsUserId { get; set; }
		public ReducedPatientDTO Patient { get; set; }
		public int DoctorsUserId { get; set; }
		public ReducedDoctorDTO Doctor { get; set; }
		public int ServiceId { get; set; }
		public ServiceDTO Service { get; set; }
	}
}
