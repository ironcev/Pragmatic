using System;
using System.Linq.Expressions;
using SwissKnife;
using SwissKnife.IdentifierConversion;

namespace TinyDdd.Interaction
{
    public static class ResponseMessageKeyBuilder
    {
        private static readonly ConversionOptions _conversionOptions = new ConversionOptions
        {
            StaticMemberConversion = StaticMemberConversion.ParentTypeName
        };

        public static string From(Expression<Func<string>> resourceSelector)
        {
            System.Diagnostics.Debug.Assert(resourceSelector != null);

            return Identifier.ToString(resourceSelector, _conversionOptions);
        }
    }
}
