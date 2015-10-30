using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.CompilerServices;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;
using SwissKnife.Time;

namespace Pragmatic.Interaction.Caching
{
    public class AutoInvalidatingInMemoryResultCache<TQuery, TResult> : IQueryResultCache<TQuery, TResult>
        where TQuery : class, IEquatableQuery<TQuery, TResult>
    {
        // How the implementation works?
        // We want to reuse the System.Runtime.Caching.MemoryCache
        // because we don't want to implement our own time-based invalidation logic right now.
        // But the MemoryCache accepts only strings as keys and in our case
        // the keys are equatable queries that have to be compared by using the
        // IsEquivalentTo() method.
        // So, to join these two worlds we will do the following:
        // - At storing create a dummy unique string key that represents the query (a class of equivalent queries, actually).
        // - At retrieving, go through the all queries in the cache to see if the equivalent one is there.
        // - If the equivalent is there, get its string key and return the cached result stored under that key.
        // So, in the MemoryCache, we have unique strings for keys and instances of the private
        // class QueryResultCacheItem as stored objects.
        // Note that we will have a performance penalty in locking the whole MemoryCache while
        // iterating through all the stored query results :-(
        // Plus the constant casting and the heavyweight MemoryCache implementation
        // built for far more advanced scenarios.
        // In other words, this implementation is a good candidate for performance improvement.

        public TimeSpan TimeSpan { get; private set; }
        // MSDN advice against creating MemoryCache instances but to use the default instance.
        // See the Caution section at the end of this MSDN help article:
        // https://msdn.microsoft.com/en-us/library/system.runtime.caching.memorycache.memorycache(v=vs.110).aspx
        // Still, we don't want to have a single huge cache.
        // This would severely affect performance of all of the operations.
        // That's why we are creating a separate MemoryCache instance per cache.
        private readonly MemoryCache _cache = new MemoryCache(Guid.NewGuid().ToString());

        public AutoInvalidatingInMemoryResultCache(TimeSpan timeSpan)
        {
            Argument.IsValid(timeSpan > TimeSpan.Zero && timeSpan < TimeSpan.FromDays(365),
                             string.Format("The invalidation time span must be greater then zero and less than one year. The provided time span was: {0}", timeSpan),
                             "timeSpan");

            TimeSpan = timeSpan;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool TryGetCachedResultFor(TQuery query, out TResult result)
        {
            Argument.IsNotNull(query, "query");

            var existingCachedItem = FindExistingCachedItem(query);
            if (default(KeyValuePair<string, object>).Equals(existingCachedItem))
            {
                result = default(TResult);
                return false;
            }

            var cachedValue = (QueryResultCacheItem)_cache[existingCachedItem.Key];
            // We have to check if the value exists because it could expire immediately
            // after we finished the above enumerating ;-)
            if (cachedValue == null)
            {
                result = default(TResult);
                return false;
            }

            result = cachedValue.Result;
            return true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CacheResultFor(TQuery query, TResult result)
        {
            Argument.IsNotNull(query, "query");
            Argument.IsNotNull(result, "result");

            var existingCachedItem = FindExistingCachedItem(query);
            // If the query does not exist create new key, otherwise, take the existing one.
            string key = default(KeyValuePair<string, object>).Equals(existingCachedItem)
                ? Guid.NewGuid().ToString()
                : existingCachedItem.Key;

            _cache.Set(key, new QueryResultCacheItem(query, result), TimeGenerator.GetUtcNow().Add(TimeSpan));
        }

        private KeyValuePair<string, object> FindExistingCachedItem(TQuery query)
        {
            // This line causes performance problems: blocking the cache and casting. For blocking, see:
            // https://msdn.microsoft.com/en-us/library/system.runtime.caching.memorycache.getenumerator(v=vs.110).aspx
            return _cache.FirstOrDefault(item => ((QueryResultCacheItem)item.Value).Query.IsEquivalentTo(query));
        }
            
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void InvalidateCacheFor(Func<TQuery, bool> queryShouldBeInvalidated)
        {
            Argument.IsNotNull(queryShouldBeInvalidated, "queryShouldBeInvalidated");

            var keysToInvalidate = _cache
                .Where(item => queryShouldBeInvalidated(((QueryResultCacheItem)item.Value).Query))
                .Select(item => item.Key)
                .ToArray();
            foreach (var key in keysToInvalidate)
                _cache.Remove(key);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void InvalidateCacheForAllQueries()
        {
            // The original implementation tried to simply call the Trim(100)
            // method on the MemoryCache object, but it turned out that
            // the objects were actually still in cache.
            // It looks like Trim() is not deterministic, which is fine for Trim()
            // but not in our case, where we need deterministic behavior.
            // That's why this "manual" removal.
            var keysToInvalidate = _cache
                .Select(item => item.Key)
                .ToArray();
            foreach (var key in keysToInvalidate)
                _cache.Remove(key);
        }

        bool IQueryResultCache<TResult>.TryGetCachedResultFor(IQuery query, out TResult result)
        {
            Argument.Is<TQuery>(Option<object>.From(query), "query");

            return TryGetCachedResultFor((TQuery)query, out result);
        }

        void IQueryResultCache<TResult>.CacheResultFor(IQuery query, TResult result)
        {
            Argument.Is<TQuery>(Option<object>.From(query), "query");

            CacheResultFor((TQuery)query, result);
        }

        private class QueryResultCacheItem
        {
            public TQuery Query { get; private set; }
            public TResult Result { get; private set; }

            public QueryResultCacheItem(TQuery query, TResult result)
            {
                Query = query;
                Result = result;
            }
        }
    }
}
