using AutoMapper;
using MediCare.DTOs;
using MediCare.Models;

namespace MediCare.Mapping
{
	public class MediCareMappingProfile : Profile
	{
		public MediCareMappingProfile()
		{
			CreateMap<RegisterRequestDTO, User>()
				.ForMember(x => x.Password, y => y.Ignore())
				.ForMember(x => x.RefreshToken, y => y.Ignore())
				.ForMember(x => x.RefreshTokenExpiration, y => y.Ignore())
				.ForMember(x => x.RoleId, y => y.Ignore())
				.ForMember(x => x.Role, y => y.Ignore())
				.ForMember(x => x.Logs, y => y.Ignore())
				.ForMember(x => x.Salt, y => y.Ignore());
		}
	}
}
