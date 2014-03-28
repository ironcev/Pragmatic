using System;
using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.Results;
using Pragmatic.Interaction;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.FluentValidation
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(this IRuleBuilderOptions<T, TProperty> ruleBuilderOptions, Expression<Func<string>> resourceSelector)
        {
            Argument.IsNotNull(ruleBuilderOptions, "ruleBuilderOptions");
            Argument.IsNotNull(resourceSelector, "resourceSelector");

            return ruleBuilderOptions.WithLocalizedMessage(resourceSelector).WithState(x => ResponseMessageKeyBuilder.From(resourceSelector));
        }

        public static Response AddErrors(this Response response, ValidationResult validationResult)
        {
            Argument.IsNotNull(response, "response");
            Argument.IsNotNull(validationResult, "validationResult");

            foreach (var error in validationResult.Errors)
                response.AddError(error.ErrorMessage, error.CustomState is string ? (string) error.CustomState : string.Empty);

            return response;
        }
    }

}
