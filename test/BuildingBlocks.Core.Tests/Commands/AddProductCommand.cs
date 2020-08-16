using BuildingBlocks.Commands;
using MediatR;

namespace BuildingBlocks.Tests.Commands
{
    public class AddProductCommand : Command, IRequest<string>
    {
        public string Name { get; private set; }

        public AddProductCommand(string name) { Name = name; }

        public override bool Validate()
        {
            ValidationResult = new AddProductValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}