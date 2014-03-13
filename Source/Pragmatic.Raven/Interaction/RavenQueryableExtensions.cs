using System;
using System.Linq;
using Pragmatic.Interaction;
using Raven.Client.Linq;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;
using Raven.Client;

namespace Pragmatic.Raven.Interaction
{
    public static class RavenQueryableExtensions
    {
        public static IPagedEnumerable<T> ToPagedEnumerable<T>(this IQueryable<T> queryable, Option<Paging> paging) where T : class
        {
            Argument.IsNotNull(queryable, "queryable");
            Argument.Is<IRavenQueryable<T>>((object)queryable, "queryable");

            var take = paging.IsSome ? paging.Value.PageSize : int.MaxValue;
            var skip = paging.IsSome ? paging.Value.Skip : 0;

            RavenQueryStatistics stats;
            var results = ((IRavenQueryable<T>)queryable)
                .Statistics(out stats)
                .Skip(skip)
                .Take(take)
                .ToArray();

            return new PagedList<T>(results, paging.Value.Page, paging.Value.PageSize, stats.TotalResults);
        }
    }
}
