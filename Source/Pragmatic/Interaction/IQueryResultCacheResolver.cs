using System;
using System.Collections.Generic;

namespace Pragmatic.Interaction
{
    public interface IQueryResultCacheResolver
    {
        IEnumerable<object> ResolveQueryResultCache(Type queryHandlerType);
    }
}
