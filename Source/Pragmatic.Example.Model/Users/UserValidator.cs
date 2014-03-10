using FluentValidation;
using Pragmatic.Example.Model.Localization;
using Pragmatic.FluentValidation;

namespace Pragmatic.Example.Model.Users
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
