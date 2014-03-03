using FluentValidation;
using TinyDdd.Example.Model.Localization;
using TinyDdd.FluentValidation;

namespace TinyDdd.Example.Model.Users
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithError(() => UserResources.FirstNameMustNotBeEmpty);
            RuleFor(x => x.LastName).NotEmpty().WithError(() => UserResources.LastNameMustNotBeEmpty);
            RuleFor(x => x.Email).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithError(() => UserResources.EmailMustNotBeEmpty)
                .EmailAddress().WithError(() => UserResources.EmailMustBeAValidEmailAddress);
        }
    }
}
