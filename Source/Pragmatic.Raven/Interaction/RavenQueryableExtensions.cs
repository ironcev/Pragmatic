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

            Paging pagingValue = paging.ValueOr(Paging.None);

            RavenQueryStatistics stats;
            var results = ((IRavenQueryable<T>)queryable)
                .Statistics(out stats)
                .Skip(pagingValue.Skip)
                .Take(pagingValue.PageSize)
                .ToArray();

            return new PagedList<T>(results, pagingValue.Page, pagingValue.PageSize, stats.TotalResults);
        }
    }
}
