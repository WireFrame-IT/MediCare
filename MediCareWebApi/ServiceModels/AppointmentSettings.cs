namespace MediCare.ServiceModels
{
	public class AppointmentSettings
	{
		public int StartHour { get; set; }
		public int EndHour { get; set; }
		public List<string> AvailableDays { get; set; }
		public string FamilyMedicineServiceName { get; set; }
		public string FamilyMedicineServiceDescription { get; set; }
	}
}
