using AutoMapper;
using GameServer.Models;
using GameServer.Services.Dtos.Round;

namespace GameServer.Services.MappingConfiguration
{
    public class RoundProfile : Profile
    {
        public RoundProfile()
        {
            CreateMap<Round, RoundDto>()
                .ForMember(x => x.QuestionBody, cfg => cfg.MapFrom(y => y.Question.QuestionBody))
                .ForMember(x => x.QuestionId, cfg => cfg.MapFrom(y => y.Question.Id))
                ;
        }
    }
}
