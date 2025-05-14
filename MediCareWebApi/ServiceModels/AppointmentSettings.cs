namespace MediCare.ServiceModels
{
	public class AppointmentSettings
	{
		public int StartHour { get; set; }
		public int EndHour { get; set; }
		public List<int> AvailableDays { get; set; }
		public string FamilyMedicineServiceName { get; set; }
	}
}
