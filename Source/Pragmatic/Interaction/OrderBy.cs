using SwissKnife.Diagnostics.Contracts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Pragmatic.Interaction
{
    public class OrderBy<T> where T : class
    {
        private readonly List<OrderByItem<T>> _orderByItems;

        public OrderBy(Expression<Func<T, object>> criteria, OrderByDirection direction = OrderByDirection.Ascending)
        {
            _orderByItems = new List<OrderByItem<T>>();
            ThenBy(criteria, direction);
        }

        public OrderBy<T> ThenBy(Expression<Func<T, object>> criteria, OrderByDirection direction = OrderByDirection.Ascending)
        {
            _orderByItems.Add(new OrderByItem<T>(criteria, direction));

            return this;
        }

        public IEnumerable<OrderByItem<T>> OrderByItems { get { return _orderByItems; } }
    }

    public struct OrderByItem<T> where T : class
    {
        internal OrderByItem(Expression<Func<T, object>> criteria, OrderByDirection direction = OrderByDirection.Ascending)
            : this()
        {
            Argument.IsNotNull(criteria, "criteria");

            Criteria = criteria;
            Direction = direction;
        }

        public Expression<Func<T, object>> Criteria { get; private set; }

        public OrderByDirection Direction { get; private set; }
    }
}
