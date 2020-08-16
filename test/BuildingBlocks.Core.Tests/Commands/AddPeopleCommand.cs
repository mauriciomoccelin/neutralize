using BuildingBlocks.Commands;

namespace BuildingBlocks.Tests.Commands
{
    public class AddPeopleCommand : Command
    {
        public string Name { get; private set; }

        public AddPeopleCommand(string name) { Name = name; }

        public override bool Validate()
        {
            ValidationResult = new AddPeopleValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}