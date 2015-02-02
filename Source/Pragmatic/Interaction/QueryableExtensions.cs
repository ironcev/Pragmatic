using System;
using System.Linq;
using System.Linq.Expressions;
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
            IOrderedQueryable<T> orderedQueryable = firstOrderBy.Direction == OrderByDirection.Ascending ?
                                                    queryable.OrderBy(firstOrderBy.Criteria) : 
                                                    queryable.OrderByDescending(firstOrderBy.Criteria);

            if (orderBy.Value.OrderByItems.Count() > 1)
            {
                orderedQueryable = orderBy.Value.OrderByItems.Skip(1)
                                          .Aggregate(orderedQueryable, (current, orderByItem) => 
                                              orderByItem.Direction == OrderByDirection.Ascending ? 
                                                                       current.ThenBy(orderByItem.Criteria) : 
                                                                       current.ThenByDescending(orderByItem.Criteria));
            }

            return orderedQueryable;
        }

        // Entity Framework throws exception of order is done by a value type.
        // In that case the following exception will be thrown:
        //  Unable to cast the type 'System.Guid' to type 'System.Object'. LINQ to Entities only supports casting EDM primitive or enumeration types.
        // This method transforms the <T, object> expressions into <T, ConcreteValueType> expressions.
        // For more details, see: http://stackoverflow.com/questions/1145847/entity-framework-linq-to-entities-only-supports-casting-entity-data-model-primi
        public static IQueryable<T> OrderByWithExpressionTransform<T>(this IQueryable<T> queryable, Option<OrderBy<T>> orderBy) where T : class
        {
            Argument.IsNotNull(queryable, "queryable");

            if (orderBy.IsNone || !orderBy.Value.OrderByItems.Any()) return queryable;

            var firstOrderBy = orderBy.Value.OrderByItems.First();
            IOrderedQueryable<T> orderedQueryable = GetOrderedQueryableWithTransformedExpression(queryable, firstOrderBy, false);

            if (orderBy.Value.OrderByItems.Count() > 1)
            {
                orderedQueryable = orderBy.Value.OrderByItems.Skip(1)
                                          .Aggregate(orderedQueryable, (current, orderByItem) => GetOrderedQueryableWithTransformedExpression(orderedQueryable, orderByItem, true));
            }

            return orderedQueryable;
        }

        private static IOrderedQueryable<T> GetOrderedQueryableWithTransformedExpression<T>(IQueryable<T> queryable, OrderByItem<T> orderByItem, bool useThenByExpression) where T: class
        {
            System.Diagnostics.Debug.Assert((useThenByExpression && queryable is IOrderedQueryable) || !useThenByExpression, "If useThenByExpression is true, the queryable must already be an ordered queryable.");
            IOrderedQueryable<T> orderedQueryable = useThenByExpression ? (IOrderedQueryable<T>)queryable : null;

            // Well, ReSharper is not smart enough to see that orderedQueryable will never be null.
            // ReSharper disable AssignNullToNotNullAttribute
            var unaryExpression = orderByItem.Criteria.Body as UnaryExpression;
            if (unaryExpression == null)
                return useThenByExpression
                    ? orderByItem.Direction == OrderByDirection.Ascending ? orderedQueryable.ThenBy(orderByItem.Criteria) : orderedQueryable.ThenByDescending(orderByItem.Criteria)
                    : orderByItem.Direction == OrderByDirection.Ascending ? queryable.OrderBy(orderByItem.Criteria) : queryable.OrderByDescending(orderByItem.Criteria);

            var propertyExpression = (MemberExpression)unaryExpression.Operand;
            var parameters = orderByItem.Criteria.Parameters;

            if (propertyExpression.Type == typeof(Guid))
            {
                var newExpression = Expression.Lambda<Func<T, Guid>>(propertyExpression, parameters);
                return useThenByExpression
                        ? orderByItem.Direction == OrderByDirection.Ascending ? orderedQueryable.ThenBy(newExpression) : orderedQueryable.ThenByDescending(newExpression)
                        : orderByItem.Direction == OrderByDirection.Ascending ? queryable.OrderBy(newExpression) : queryable.OrderByDescending(newExpression);
            }

            if (propertyExpression.Type == typeof(int))
            {
                var newExpression = Expression.Lambda<Func<T, int>>(propertyExpression, parameters);
                return useThenByExpression
                        ? orderByItem.Direction == OrderByDirection.Ascending ? orderedQueryable.ThenBy(newExpression) : orderedQueryable.ThenByDescending(newExpression)
                        : orderByItem.Direction == OrderByDirection.Ascending ? queryable.OrderBy(newExpression) : queryable.OrderByDescending(newExpression);
            }

            if (propertyExpression.Type == typeof(long))
            {
                var newExpression = Expression.Lambda<Func<T, long>>(propertyExpression, parameters);
                return useThenByExpression
                        ? orderByItem.Direction == OrderByDirection.Ascending ? orderedQueryable.ThenBy(newExpression) : orderedQueryable.ThenByDescending(newExpression)
                        : orderByItem.Direction == OrderByDirection.Ascending ? queryable.OrderBy(newExpression) : queryable.OrderByDescending(newExpression);
            }

            if (propertyExpression.Type == typeof(double))
            {
                var newExpression = Expression.Lambda<Func<T, double>>(propertyExpression, parameters);
                return useThenByExpression
                        ? orderByItem.Direction == OrderByDirection.Ascending ? orderedQueryable.ThenBy(newExpression) : orderedQueryable.ThenByDescending(newExpression)
                        : orderByItem.Direction == OrderByDirection.Ascending ? queryable.OrderBy(newExpression) : queryable.OrderByDescending(newExpression);
            }

            if (propertyExpression.Type == typeof(float))
            {
                var newExpression = Expression.Lambda<Func<T, float>>(propertyExpression, parameters);
                return useThenByExpression
                        ? orderByItem.Direction == OrderByDirection.Ascending ? orderedQueryable.ThenBy(newExpression) : orderedQueryable.ThenByDescending(newExpression)
                        : orderByItem.Direction == OrderByDirection.Ascending ? queryable.OrderBy(newExpression) : queryable.OrderByDescending(newExpression);
            }

            if (propertyExpression.Type == typeof(decimal))
            {
                var newExpression = Expression.Lambda<Func<T, decimal>>(propertyExpression, parameters);
                return useThenByExpression
                        ? orderByItem.Direction == OrderByDirection.Ascending ? orderedQueryable.ThenBy(newExpression) : orderedQueryable.ThenByDescending(newExpression)
                        : orderByItem.Direction == OrderByDirection.Ascending ? queryable.OrderBy(newExpression) : queryable.OrderByDescending(newExpression);
            }

            if (propertyExpression.Type == typeof(DateTime))
            {
                var newExpression = Expression.Lambda<Func<T, DateTime>>(propertyExpression, parameters);
                return useThenByExpression
                        ? orderByItem.Direction == OrderByDirection.Ascending ? orderedQueryable.ThenBy(newExpression) : orderedQueryable.ThenByDescending(newExpression)
                        : orderByItem.Direction == OrderByDirection.Ascending ? queryable.OrderBy(newExpression) : queryable.OrderByDescending(newExpression);
            }

            if (propertyExpression.Type == typeof(DateTimeOffset))
            {
                var newExpression = Expression.Lambda<Func<T, DateTimeOffset>>(propertyExpression, parameters);
                return useThenByExpression
                        ? orderByItem.Direction == OrderByDirection.Ascending ? orderedQueryable.ThenBy(newExpression) : orderedQueryable.ThenByDescending(newExpression)
                        : orderByItem.Direction == OrderByDirection.Ascending ? queryable.OrderBy(newExpression) : queryable.OrderByDescending(newExpression);
            }
            // ReSharper restore AssignNullToNotNullAttribute

            throw new NotSupportedException(string.Format("Expression of the type 'Expression<Func<{0}, System.Object>>' cannot be transformed to " +
                                                          "an expression of the type 'Expression<Func<{0}, {1}>>'. " +
                                                          "Expression transformation for the type '{1}' is currently not supported.", typeof(T), propertyExpression.Type));
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