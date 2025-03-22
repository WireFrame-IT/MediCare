namespace MediCare.DTOs.ViewModels
{
	public class PatientDTO
	{
		public int UserId { get; set; }
		public UserDTO User { get; set; }
		public DateTime RegisterDate { get; set; }
		public DateTime BirthDate { get; set; }
		public string PatientCard { get; set; }
	}
}
