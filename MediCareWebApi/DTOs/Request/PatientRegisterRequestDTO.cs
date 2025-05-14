using System.ComponentModel.DataAnnotations;

namespace MediCare.DTOs.Request
{
    public class PatientRegisterRequestDTO : RegisterRequestDTO
    {
		[Required(ErrorMessage = "Data urodzenia jest wymagana.")]
		[DataType(DataType.Date, ErrorMessage = "Nieprawidłowy format daty.")]
		public DateTime BirthDate { get; set; }
    }
}
