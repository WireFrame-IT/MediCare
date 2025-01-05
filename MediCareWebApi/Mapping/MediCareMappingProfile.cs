using AutoMapper;
using MediCare.DTOs.Request;
using MediCare.DTOs.ViewModels;
using MediCare.Models;

namespace MediCare.Mapping
{
    public class MediCareMappingProfile : Profile
	{
		public MediCareMappingProfile()
		{
			CreateMap<RegisterRequestDTO, User>()
				.ForMember(x => x.Password, y => y.Ignore())
				.ForMember(x => x.Salt, y => y.Ignore())
				.ForMember(x => x.RefreshToken, y => y.Ignore())
				.ForMember(x => x.RefreshTokenExpiration, y => y.Ignore())
				.ForMember(x => x.RoleId, y => y.Ignore())
				.ForMember(x => x.Role, y => y.Ignore())
				.ForMember(x => x.Logs, y => y.Ignore());

			CreateMap<AppointmentRequestDTO, Appointment>()
				.ForMember(x => x.Status, y => y.Ignore())
				.ForMember(x => x.Diagnosis, y => y.Ignore())
				.ForMember(x => x.PatientId, y => y.Ignore())
				.ForMember(x => x.Patient, y => y.Ignore())
				.ForMember(x => x.Doctor, y => y.Ignore())
				.ForMember(x => x.Service, y => y.Ignore())
				.ForMember(x => x.Feedbacks, y => y.Ignore());

			CreateMap<PatientRegisterRequestDTO, User>();
			CreateMap<DoctorRegisterRequestDTO, User>();

			CreateMap<Appointment, AppointmentDTO>();
			CreateMap<Doctor, DoctorDTO>();
			CreateMap<Feedback, FeedbackDTO>();
			CreateMap<Patient, PatientDTO>();
			CreateMap<Permission, PermissionDTO>();
			CreateMap<Role, RoleDTO>();
			CreateMap<RolePermission, RolePermissionDTO>();
			CreateMap<Service, ServiceDTO>();
			CreateMap<Speciality, SpecialityDTO>();
			CreateMap<User, UserDTO>();
		}
	}
}
