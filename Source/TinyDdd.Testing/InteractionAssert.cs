using System.Reflection;
using SwissKnife.Diagnostics.Contracts;

namespace TinyDdd.Testing
{
    public static class InteractionAssert
    {
        public static void ThatCommandsAreSealed(Assembly assembly)
        {
            Argument.IsNotNull(assembly, "assembly");

            // TODO-IG: Add support to SwissKnife to get the concrete implementations of the abstract base classes.
        }

        public static void ThatQueriesAreSealed(Assembly assembly)
        {
            Argument.IsNotNull(assembly, "assembly");

            // TODO-IG: Add support to SwissKnife to get the concrete implementations of the abstract base classes.
        }
    }
}
/* DECISIONS:
 * Because of performance reasons it would be better to have a single method called ThatCommandsAndQueriesAreSealed().
 * Having two methods gives better separation and allows separate unit tests and is a good treadoff.
 * The good 
 */
