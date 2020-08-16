using FluentValidation;

namespace Neutralize.Tests.Commands
{
    public class AddPeopleValidator : AbstractValidator<AddPeopleCommand>
    {
        public AddPeopleValidator() { RuleFor(x => x.Name).NotEmpty().WithMessage("Required"); }
    }
}
