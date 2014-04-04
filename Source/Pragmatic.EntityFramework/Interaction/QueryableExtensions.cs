using System.Data.Entity;
using System.Linq;
using Pragmatic.Interaction;
using SwissKnife;

namespace Pragmatic.EntityFramework.Interaction
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, Option<OrderBy<T>> orderOption) where T:class
        {
            if (orderOption.IsSome)
            {
                foreach (var orderByItem in orderOption.Value.OrderByItems)
                {
                    source = orderByItem.Direction == OrderByDirection.Ascending
                        ? source.OrderBy(orderByItem.Criteria)
                        : source.OrderByDescending(orderByItem.Criteria);
                }
            }

            return source;
        }
    }
}
