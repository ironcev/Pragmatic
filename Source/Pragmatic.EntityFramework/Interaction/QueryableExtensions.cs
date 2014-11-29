using System.Linq;
using Pragmatic.Interaction;
using SwissKnife;

namespace Pragmatic.EntityFramework.Interaction
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, Option<OrderBy<T>> orderBy) where T:class
        {
            if (orderBy.IsNone || !orderBy.Value.OrderByItems.Any()) return source;

            foreach (var orderByItem in orderBy.Value.OrderByItems)
            {
                source = orderByItem.Direction == OrderByDirection.Ascending
                    ? source.OrderBy(orderByItem.Criteria)
                    : source.OrderByDescending(orderByItem.Criteria);
            }

            return source;
        }
    }
}
