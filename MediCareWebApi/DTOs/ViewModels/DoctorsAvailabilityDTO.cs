namespace MediCare.DTOs.ViewModels
{
	public class DoctorsAvailabilityDTO
	{
		public DateTime From { get; set; }

		public DateTime? To { get; set; }

		public int DoctorsUserId { get; set; }

		public DoctorDTO Doctor { get; set; }
	}
}
