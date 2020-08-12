using BuildingBlocks.Core.Commands;

namespace BuildingBlocks.Core.Tests.Commands.Crud
{
    public class DeletePersonCommand : DeleteCommand
    {
        public DeletePersonCommand(long id)
        {
            Id = id;
        }
        
        public override bool Validate()
        {
            return true;
        }
    }
}