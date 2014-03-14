using System;
using System.Collections.Generic;
using System.Linq;
using Pragmatic.Interaction;
using StructureMap;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.StructureMap
{
    public class StructureMapInteractionHandlerResolver : IInteractionHandlerResolver
    {
        public IEnumerable<object> ResolveInteractionHandler(Type interactionHandlerType)
        {
            Argument.IsNotNull(interactionHandlerType, "interactionHandlerType");

            return ObjectFactory.GetAllInstances(interactionHandlerType).Cast<object>();
        }
    }
}
