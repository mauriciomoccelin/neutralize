using BuildingBlocks.Core.Commands;

namespace BuildingBlocks.Core.Tests.Commands.OnDemand
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