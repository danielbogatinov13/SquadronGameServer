using AutoMapper;
using GameServer.Models;
using GameServer.Services.Dtos.Account;

namespace GameServer.Services.MappingConfiguration
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ReverseMap();

            CreateMap<RegisterDto, User>()
                .ForMember(x => x.SecurityStamp, cfg => cfg.MapFrom(y => Guid.NewGuid().ToString()));
        }
    }
}
