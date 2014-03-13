using System;
using System.Linq.Expressions;
using SwissKnife;

namespace Pragmatic.Interaction.StandardQueries
{
    public sealed class GetAllQuery<T> : IQuery<IPagedEnumerable<T>> where T : class
    {
        public Option<Expression<Func<T, bool>>> Criteria { get; set; }
        public Option<Paging> Paging { get; set; }
        public Option<OrderBy<T>> OrderBy { get; set; }
    }
}
