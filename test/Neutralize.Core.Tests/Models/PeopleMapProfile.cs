using AutoMapper;

namespace Neutralize.Tests.Models
{
    public class PeopleMapProfile : Profile
    {
        public PeopleMapProfile()
        {
            CreateMap<PeopleDto, People>();
            CreateMap<People, PeopleDto>();
        }
    }
}