using System;
using System.Collections.Generic;

namespace Pragmatic.Interaction
{
    public interface IInteractionHandlerResolver
    {
        IEnumerable<object> ResolveInteractionHandler(Type interactionHandlerType);
    }
}
