using System;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic
{
    internal class ArgumentCheck
    {
        internal static void EntityTypeRepresentsEntityType(Type entityType, Option<string> parameterName) // TODO-IG: Does the Entity type has to be concrete type or it can be abstract base type?
        {
            Argument.IsNotNull(entityType, parameterName);
            Argument.IsValid(typeof(Entity) != entityType,
                             string.Format("Entity type must not be the '{0}' type itself. Entity type must derive from '{0}'.", typeof(Entity)),
                             "entityType");
            Argument.IsValid(typeof(Entity).IsAssignableFrom(entityType),
                             string.Format("Entity type does not derive from '{0}'. Entity type must derive from '{0}'. The entity type is: '{1}'.", typeof(Entity), entityType),
                             "entityType");
        }
    }
}
