using System.Linq;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Interaction
{
    public static class OrderedQueryableExtensions
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IOrderedQueryable<T> queryable, Option<OrderBy<T>> orderBy) where T : class
        {
            Argument.IsNotNull(queryable, "queryable");

            if (orderBy.IsNone || !orderBy.Value.OrderByItems.Any()) return queryable;

            var firstOrderBy = orderBy.Value.OrderByItems.First();
            queryable = firstOrderBy.Direction == OrderByDirection.Ascending ? queryable.OrderBy(firstOrderBy.Criteria) : queryable.OrderByDescending(firstOrderBy.Criteria);

            if (orderBy.Value.OrderByItems.Count() > 1)
            {
                queryable = orderBy.Value.OrderByItems.Skip(1).Aggregate(queryable, (current, orderByItem) => orderByItem.Direction == OrderByDirection.Ascending ? current.ThenBy(orderByItem.Criteria) : current.ThenByDescending(orderByItem.Criteria));
            }

            return queryable;
        }
    }
}
