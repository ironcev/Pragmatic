using System;
using System.Linq;
using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.TestHelper;
using Pragmatic.Interaction;
using SwissKnife;

namespace Pragmatic.FluentValidation
{
    public static class FluentValidationTestExtensions
    {
        public static void ShouldHaveValidationErrorWithKeyFor<T, TValue>(this IValidator<T> validator, Expression<Func<T, TValue>> expression, T objectToTest, Expression<Func<string>> resourceSelector)
        {
            ShouldHaveValidationErrorFor(validator, expression, objectToTest, ResponseMessageKeyBuilder.From(resourceSelector));
        }

        public static void ShouldHaveValidationErrorFor<T, TValue>(this IValidator<T> validator, Expression<Func<T, TValue>> expression, T objectToTest, object customState)
        {
            string memberName = Identifier.ToString(expression);
            var count = validator.Validate(objectToTest).Errors.Count(x => x.PropertyName == memberName && Equals(x.CustomState, customState));

            if (count == 0)
            {
                throw new ValidationTestException(string.Format("Expected a validation error for property {0} with custom state {1}.", memberName, customState));
            }
        }

        public static void ShouldNotHaveValidationErrorWithKeyFor<T, TValue>(this IValidator<T> validator, Expression<Func<T, TValue>> expression, T objectToTest, Expression<Func<object>> resourceSelector)
        {
            ShouldNotHaveValidationErrorFor(validator, expression, objectToTest, Identifier.ToString(resourceSelector));
        }

        public static void ShouldNotHaveValidationErrorFor<T, TValue>(this IValidator<T> validator, Expression<Func<T, TValue>> expression, T objectToTest, object customState)
        {
            string memberName = Identifier.ToString(expression);
            var count = validator.Validate(objectToTest).Errors.Count(x => x.PropertyName == memberName && Equals(x.CustomState, customState));

            if (count > 0)
            {
                throw new ValidationTestException(string.Format("Expected no validation errors for property {0} with custom state {1}.", memberName, customState));
            }
        }
    }
}
