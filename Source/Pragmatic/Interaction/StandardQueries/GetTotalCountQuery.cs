using System;
using System.Linq.Expressions;
using SwissKnife;

namespace Pragmatic.Interaction.StandardQueries
{
    public sealed class GetTotalCountQuery<T> : IQuery<int> where T : class
    {
        public Option<Expression<Func<T, bool>>> Criteria { get; set; }
    }
}
