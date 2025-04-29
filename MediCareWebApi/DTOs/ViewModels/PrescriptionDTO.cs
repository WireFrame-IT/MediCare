namespace MediCare.DTOs.ViewModels
{
	public class PrescriptionDTO
	{
		public DateTime IssueDate { get; set; }

		public DateTime ExpirationDate { get; set; }

		public string Description { get; set; }

		public int AppointmentId { get; set; }
	}
}
