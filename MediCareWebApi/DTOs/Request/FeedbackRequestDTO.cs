namespace MediCare.DTOs.Request
{
	public class FeedbackRequestDTO
	{
		public byte Rate { get; set; }
		public string Description { get; set; }
		public int AppointmentId { get; set; }
	}
}
