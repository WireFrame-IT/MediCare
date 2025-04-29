namespace MediCare.DTOs.Request
{
	public class PrescriptionRequestDTO
	{
		public int AppointmentId { get; set; }
		public string Description { get; set; }
		public DateTime ExpirationDate { get; set; }
	}
}
