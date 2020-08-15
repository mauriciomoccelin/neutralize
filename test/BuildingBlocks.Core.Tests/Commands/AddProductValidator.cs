using FluentValidation;

namespace BuildingBlocks.Core.Tests.Commands.OnDemand
{
    public class AddProductValidator : AbstractValidator<AddProductCommand>
    {
        public AddProductValidator() { RuleFor(x => x.Name).NotEmpty().WithMessage("Required"); }
    }
}