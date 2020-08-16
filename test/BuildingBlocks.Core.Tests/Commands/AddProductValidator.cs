using FluentValidation;

namespace BuildingBlocks.Tests.Commands
{
    public class AddProductValidator : AbstractValidator<AddProductCommand>
    {
        public AddProductValidator() { RuleFor(x => x.Name).NotEmpty().WithMessage("Required"); }
    }
}