using FluentValidation;

namespace BuildingBlocks.Core.Tests.Commands.OnDemand
{
    public class AddPeopleValidator : AbstractValidator<AddPeopleCommand>
    {
        public AddPeopleValidator() { RuleFor(x => x.Name).NotEmpty().WithMessage("Required"); }
    }
}