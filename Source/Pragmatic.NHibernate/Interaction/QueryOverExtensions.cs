using System.Linq;
using NHibernate;
using Pragmatic.Interaction;
using SwissKnife;

namespace Pragmatic.NHibernate.Interaction
{
    public static class QueryOverExtensions
    {
        public static IQueryOver<T, T> OrderBy<T>(this IQueryOver<T, T> query, Option<OrderBy<T>> orderBy) where T : class
        {
            if (orderBy.IsNone || !orderBy.Value.OrderByItems.Any()) return query;

            return orderBy.Value.OrderByItems.Aggregate(query, (current, orderByItem) => orderByItem.Direction == OrderByDirection.Ascending ? current.OrderBy(orderByItem.Criteria).Asc : current.OrderBy(orderByItem.Criteria).Desc);
        }

        public static IPagedEnumerable<T> ToPagedEnumerable<T>(this IQueryOver<T, T> queryOver, Option<Paging> paging)
        {
            var take = paging.IsSome ? paging.Value.PageSize : int.MaxValue;
            var skip = paging.IsSome ? paging.Value.Skip : 0;
            
            var rowCountQuery = queryOver.ToRowCountQuery();
            var result = queryOver.Skip(skip).Take(take).Future();
            var totalResults = rowCountQuery.FutureValue<int>().Value;

            return new PagedList<T>(result, paging.Value.Page, paging.Value.PageSize, totalResults);
        }
    }
}
