using System;

namespace Pragmatic.Interaction.Caching
{
    public interface IQueryResultCache<TQuery, TResult> : IQueryResultCache<TResult> where TQuery : IQuery
    {
        bool HasCachedResultFor(TQuery query);
        TResult GetCachedResultFor(TQuery query);
        void CacheResultFor(TQuery query, TResult result);
        void InvalidateCacheFor(Func<TQuery, bool> queryShouldBeInvalidated);
        void InvalidatCacheForAllQueries();
    }

    // Technical interface. Added just to avoid reflection calls.
    public interface IQueryResultCache<TResult>
    {
        bool HasCachedResultFor(IQuery query);
        TResult GetCachedResultFor(IQuery query);
        void CacheResultFor(IQuery query, TResult result);
    }
}