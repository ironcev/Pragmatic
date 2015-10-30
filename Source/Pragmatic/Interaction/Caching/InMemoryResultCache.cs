using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Interaction.Caching
{
    public class InMemoryResultCache<TQuery, TResult> : IQueryResultCache<TQuery, TResult>
        where TQuery : class, IEquatableQuery<TQuery, TResult>
    {
        private readonly Dictionary<IEquatableQuery<TQuery, TResult>, TResult> _cache = new Dictionary<IEquatableQuery<TQuery, TResult>, TResult>(); 

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool TryGetCachedResultFor(TQuery query, out TResult result)
        {
            Argument.IsNotNull(query, "query");

            var equivalentQuery = _cache.Keys.FirstOrDefault(existingQuery => existingQuery.IsEquivalentTo(query));
            if (equivalentQuery == null)
            {
                result = default(TResult);
                return false;
            }

            result = _cache[equivalentQuery];
            return true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CacheResultFor(TQuery query, TResult result)
        {
            Argument.IsNotNull(query, "query");
            Argument.IsNotNull(result, "result");

            var equivalentQuery = _cache.Keys.FirstOrDefault(existingQuery => existingQuery.IsEquivalentTo(query)) ?? query;
            _cache[equivalentQuery] = result;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void InvalidateCacheFor(Func<TQuery, bool> queryShouldBeInvalidated)
        {
            Argument.IsNotNull(queryShouldBeInvalidated, "queryShouldBeInvalidated");

            var queriesToInvalidate = _cache.Keys.Cast<TQuery>().Where(queryShouldBeInvalidated).ToArray();
            foreach (var query in queriesToInvalidate)
                _cache.Remove(query);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void InvalidateCacheForAllQueries()
        {
            _cache.Clear();
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
    }
}
