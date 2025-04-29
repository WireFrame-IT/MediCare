using MediCare.Enums;

namespace MediCare.DTOs.ViewModels
{
	public class MedicamentDTO
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public MedicamentType MedicamentType { get; set; }

		public bool PrescriptionRequired { get; set; }
	}
}
