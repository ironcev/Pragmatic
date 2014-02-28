using FluentValidation;
using TinyDdd.Example.Model.Localization;
using TinyDdd.FluentValidation;

namespace TinyDdd.Example.Model.Users
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithLocalizedMessageAndKey(() => UserResources.FirstNameMustNotBeEmpty);
            RuleFor(x => x.LastName).NotEmpty().WithLocalizedMessageAndKey(() => UserResources.LastNameMustNotBeEmpty);
            RuleFor(x => x.Email).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithLocalizedMessageAndKey(() => UserResources.EmailMustNotBeEmpty)
                .EmailAddress().WithLocalizedMessageAndKey(() => UserResources.EmailMustBeAValidEmailAddress);
        }
    }
}
