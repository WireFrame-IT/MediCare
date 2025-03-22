namespace MediCare.DTOs.ViewModels
{
	public class DoctorDTO
	{
		public int UserId { get; set; }
		public UserDTO User { get; set; }
		public int SpecialityId { get; set; }
		public SpecialityDTO Speciality { get; set; }
		public DateTime EmploymentDate { get; set; }
	}
}
