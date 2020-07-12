using MediatR;
using Optional;

namespace BuildingBlocks.Domain.Tests.Commands
{
    public class AddPeopleCommand : Command
    {
        public string Name { get; private set; }

        public AddPeopleCommand(string name) { Name = name; }

        public override bool IsValid()
        {
            ValidationResult = new AddPeopleValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}