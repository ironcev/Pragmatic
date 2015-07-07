using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Interaction.Caching
{
    public class InMemoryEquatableQueryResultCache<TQuery, TResult> : IQueryResultCache<TQuery, TResult>
        where TQuery : class, IEquatableQuery<TQuery, TResult>
    {
        private readonly Dictionary<IEquatableQuery<TQuery, TResult>, TResult> _cache = new Dictionary<IEquatableQuery<TQuery, TResult>, TResult>(); 

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool HasCachedResultFor(TQuery query)
        {
            Argument.IsNotNull(query, "query");

            return _cache.Keys.Any(existingQuery => AreQueriesEquivalent(existingQuery, query));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public TResult GetCachedResultFor(TQuery query)
        {
            Argument.IsNotNull(query, "query");

            var equivalentQuery = _cache.Keys.FirstOrDefault(existingQuery => AreQueriesEquivalent(existingQuery, query));
            if (equivalentQuery == null)
                throw new ArgumentException(string.Format("There is no cashed result for the concrete query of type {0}.", typeof(TQuery)), "query");

            return _cache[equivalentQuery];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CacheResultFor(TQuery query, TResult result)
        {
            Argument.IsNotNull(query, "query");
            Argument.IsNotNull(result, "result");

            var equivalentQuery = _cache.Keys.FirstOrDefault(existingQuery => AreQueriesEquivalent(existingQuery, query)) ?? query;
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
        public void InvalidatCacheForAllQueries()
        {
            _cache.Clear();
        }

        private static bool AreQueriesEquivalent(IEquatableQuery<TQuery, TResult> first, TQuery second)
        {
            return ReferenceEquals(first, second) || first.IsEquivalentTo(second);
        }

        bool IQueryResultCache<TResult>.HasCachedResultFor(IQuery query)
        {
            Argument.Is<TQuery>(Option<object>.From(query), "query");

            return HasCachedResultFor((TQuery)query);
        }

        TResult IQueryResultCache<TResult>.GetCachedResultFor(IQuery query)
        {
            Argument.Is<TQuery>(Option<object>.From(query), "query");

            return GetCachedResultFor((TQuery)query);
        }

        void IQueryResultCache<TResult>.CacheResultFor(IQuery query, TResult result)
        {
            Argument.Is<TQuery>(Option<object>.From(query), "query");

            CacheResultFor((TQuery)query, result);
        }
    }
}
