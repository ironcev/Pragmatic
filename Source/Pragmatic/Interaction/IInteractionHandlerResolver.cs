using System;
using System.Collections.Generic;

namespace Pragmatic.Interaction
{
    public interface IInteractionHandlerResolver // TODO-IG: Explain decision that we don't want to have single IInteractionObjectResolver.
    {
        IEnumerable<object> ResolveInteractionHandler(Type interactionHandlerType);
    }
}
