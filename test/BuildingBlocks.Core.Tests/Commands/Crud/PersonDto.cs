using BuildingBlocks.Core.Application;

namespace BuildingBlocks.Core.Tests.Commands.Crud
{
    public class PersonDto : EntityDto<long>
    {
        public string Name { get; set; }
    }
}