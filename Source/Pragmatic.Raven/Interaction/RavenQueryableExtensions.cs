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
        public static IRavenQueryable<T> OrderBy<T>(this IRavenQueryable<T> queryable, Option<OrderBy<T>> orderBy) where T : class
        {
            return (IRavenQueryable<T>)((IQueryable<T>)queryable).OrderBy(orderBy);
        }


        public static IPagedEnumerable<T> ToPagedEnumerable<T>(this IRavenQueryable<T> queryable, Option<Paging> paging) where T : class
        {
            Argument.IsNotNull(queryable, "queryable");

            Paging pagingValue = paging.ValueOr(Paging.None);

            RavenQueryStatistics stats;
            var results = queryable
                .Statistics(out stats)
                .Skip(pagingValue.Skip)
                .Take(pagingValue.PageSize)
                .ToArray();

            return new PagedList<T>(results, pagingValue.Page, pagingValue.PageSize, stats.TotalResults);
        }
    }
}
