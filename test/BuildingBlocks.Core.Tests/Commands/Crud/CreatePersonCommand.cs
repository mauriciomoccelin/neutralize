using BuildingBlocks.Core.Commands;

namespace BuildingBlocks.Core.Tests.Commands.Crud
{
    public class CreatePersonCommand : CreateCommand<long>
    {
        public string Name { get; set; }

        public CreatePersonCommand(string name)
        {
            Name = name;
        }
        
        public override bool Validate()
        {
            return true;
        }
    }
}