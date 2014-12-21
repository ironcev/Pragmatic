using System.Linq;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Interaction
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, Option<OrderBy<T>> orderBy) where T : class
        {
            Argument.IsNotNull(queryable, "queryable");

            if (orderBy.IsNone || !orderBy.Value.OrderByItems.Any()) return queryable;

            var firstOrderBy = orderBy.Value.OrderByItems.First();
            IOrderedQueryable<T> orderedQueryable = (firstOrderBy.Direction == OrderByDirection.Ascending ? queryable.OrderBy(firstOrderBy.Criteria) : queryable.OrderByDescending(firstOrderBy.Criteria));

            if (orderBy.Value.OrderByItems.Count() > 1)
            {
                orderedQueryable = orderBy.Value.OrderByItems.Skip(1).Aggregate(orderedQueryable, (current, orderByItem) => orderByItem.Direction == OrderByDirection.Ascending ? current.ThenBy(orderByItem.Criteria) : current.ThenByDescending(orderByItem.Criteria));
            }

            return orderedQueryable;
        }

        public static IPagedEnumerable<T> ToPagedEnumerable<T>(this IQueryable<T> queryable, Option<Paging> paging)
        {
            Paging pagingValue = paging.ValueOr(Paging.None);

            var result = paging.IsNone ? queryable : queryable.Skip(pagingValue.Skip).Take(pagingValue.PageSize);
            var totalResults = queryable.Count();

            return new PagedList<T>(result, pagingValue.Page, pagingValue.PageSize, totalResults);
        }
    }
}