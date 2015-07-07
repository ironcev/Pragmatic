using System;
using System.Collections.Generic;
using System.Linq;
using Pragmatic.Interaction;
using Pragmatic.Interaction.EntityDeletion;
using StructureMap;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.StructureMap
{
    public class StructureMapInteractionObjectResolver : IInteractionHandlerResolver, IEntityDeleterResolver, IQueryResultCacheResolver
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

        public IEnumerable<object> ResolveQueryResultCache(Type queryHandlerType)
        {
            Argument.IsNotNull(queryHandlerType, "queryHandlerType");

            return ObjectFactory.GetAllInstances(queryHandlerType).Cast<object>();
        }
    }
}
