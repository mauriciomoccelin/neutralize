using BuildingBlocks.Core.Commands;

namespace BuildingBlocks.Core.Tests.Commands.Crud
{
    public class GetPersonCommand : GetResultCommand<PersonDto, long>
    {
        public GetPersonCommand(long id)
        {
            Id = id;
        }
        
        public override bool Validate()
        {
            return true;
        }
    }
}