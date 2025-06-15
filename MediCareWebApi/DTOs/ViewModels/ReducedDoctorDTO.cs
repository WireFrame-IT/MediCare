namespace MediCare.DTOs.ViewModels
{
	public class ReducedDoctorDTO
	{
		public int UserId { get; set; }
		public ReducedUserDTO User { get; set; }
		public int SpecialityId { get; set; }
		public SpecialityDTO Speciality { get; set; }
		public List<DoctorsAvailabilityDTO> DoctorsAvailabilities { get; set; } = new List<DoctorsAvailabilityDTO>();
		public List<TimeRangeDTO> UnavailableTerms { get; set; } = new List<TimeRangeDTO>();
	}
}
