using System;
using System.Collections.Generic;
using System.Linq;
using Pragmatic.Interaction;
using Pragmatic.Interaction.EntityDeletion;
using StructureMap;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.StructureMap
{
    public class StructureMapInteractionObjectResolver : IInteractionHandlerResolver, IEntityDeleterResolver
    {
        public IEnumerable<object> ResolveInteractionHandler(Type interactionHandlerType)
        {
            Argument.IsNotNull(interactionHandlerType, "interactionHandlerType");

            return ObjectFactory.GetAllInstances(interactionHandlerType).Cast<object>();
        }

        public IEnumerable<object> ResolveEntityDeleter(Type entityDeleterType)
        {
            Argument.IsNotNull(entityDeleterType, "entityDeleterType");

            return ObjectFactory.GetAllInstances(entityDeleterType).Cast<object>();
        }
    }
}
