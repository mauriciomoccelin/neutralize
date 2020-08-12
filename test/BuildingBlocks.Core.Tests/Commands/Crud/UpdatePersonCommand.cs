using BuildingBlocks.Core.Commands;

namespace BuildingBlocks.Core.Tests.Commands.Crud
{
    public class UpdatePersonCommand : UpdateCommand
    {
        public string Name { get; set; }

        public UpdatePersonCommand(long id, string name)
        {
            Id = id;
            Name = name;
        }
        
        public override bool Validate()
        {
            return true;
        }
    }
}