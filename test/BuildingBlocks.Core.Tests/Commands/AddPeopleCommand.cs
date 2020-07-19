using BuildingBlocks.Core.Commands;
using MediatR;
using Optional;

namespace BuildingBlocks.Core.Tests.Commands
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