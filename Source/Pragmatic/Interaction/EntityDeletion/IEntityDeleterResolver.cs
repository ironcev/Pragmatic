using System;
using System.Collections.Generic;

namespace Pragmatic.Interaction.EntityDeletion
{
    public interface IEntityDeleterResolver
    {
        IEnumerable<object> ResolveEntityDeleter(Type entityDeleterType);
    }
}
