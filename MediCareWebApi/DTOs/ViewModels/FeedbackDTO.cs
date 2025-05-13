namespace MediCare.DTOs.ViewModels
{
	public class FeedbackDTO
	{
		public int Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public string Description { get; set; }
		public byte Rate { get; set; }
		public int PatientsUserId { get; set; }
		public PatientDTO Patient { get; set; }
		public int AppointmentId { get; set; }
		public AppointmentDTO Appointment { get; set; }
	}
}
