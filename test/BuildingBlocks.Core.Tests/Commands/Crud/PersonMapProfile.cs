using AutoMapper;

namespace BuildingBlocks.Core.Tests.Commands.Crud
{
    public class PersonMapProfile : Profile
    {
        public PersonMapProfile()
        {
            CreateMap<PersonDto, Person>();
            CreateMap<CreatePersonCommand, Person>();
            CreateMap<UpdatePersonCommand, Person>();
            CreateMap<DeletePersonCommand, Person>();
            
            CreateMap<Person, PersonDto>();
        }
    }
}