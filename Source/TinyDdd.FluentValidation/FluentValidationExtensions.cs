using System;
using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.Results;
using SwissKnife.Collections;
using SwissKnife.Diagnostics.Contracts;
using TinyDdd.Interaction;

namespace TinyDdd.FluentValidation
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> WithLocalizedMessageAndKey<T, TProperty>(this IRuleBuilderOptions<T, TProperty> ruleBuilderOptions, Expression<Func<string>> resourceSelector)
        {
            Argument.IsNotNull(ruleBuilderOptions, "ruleBuilderOptions");
            Argument.IsNotNull(resourceSelector, "resourceSelector");

            return ruleBuilderOptions.WithLocalizedMessage(resourceSelector).WithState(x => MessageKeyBuilder.From(resourceSelector));
        }

        public static Response AddErrors(this Response response, ValidationResult validationResult)
        {
            Argument.IsNotNull(response, "response");
            Argument.IsNotNull(validationResult, "validationResult");

            validationResult.Errors.ForEach(error => response.AddError(error.ErrorMessage, error.CustomState is string ? (string)error.CustomState : string.Empty));

            return response;
        }
    }

}
