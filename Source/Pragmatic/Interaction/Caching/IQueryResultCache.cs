using System;

namespace Pragmatic.Interaction.Caching
{
    public interface IQueryResultCache<TQuery, TResult> : IQueryResultCache<TResult> where TQuery : IQuery
    {
        bool TryGetCachedResultFor(TQuery query, out TResult result); 
        void CacheResultFor(TQuery query, TResult result);
        void InvalidateCacheFor(Func<TQuery, bool> queryShouldBeInvalidated);
        void InvalidateCacheForAllQueries();
    }

    // Technical interface. Added just to avoid reflection calls.
    public interface IQueryResultCache<TResult>
    {
        bool TryGetCachedResultFor(IQuery query, out TResult result);
        void CacheResultFor(IQuery query, TResult result);
    }
}