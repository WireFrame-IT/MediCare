namespace MediCare.DTOs.ViewModels
{
	public class PermissionDTO
	{
		public int Id { get; set; }
		public string Description { get; set; }
		public bool DoctorOnly { get; set; }
		public bool PatientOnly { get; set; }
		public IEnumerable<RolePermissionDTO> PermissionRoles { get; set; } = new List<RolePermissionDTO>();
	}
}
