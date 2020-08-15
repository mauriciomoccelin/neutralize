using BuildingBlocks.Core.Commands;

namespace BuildingBlocks.Core.Tests.Commands.Crud
{
    public class GetPagedPersonCommand : GetPageResultCommand<PersonDto, long>
    {
        public override bool Validate()
        {
            return true;
        }
    }
}