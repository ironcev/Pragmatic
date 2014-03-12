using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SwissKnife;

namespace Pragmatic.Interaction.StandardQueries
{
    public sealed class GetAllQuery<T> : IQuery<IEnumerable<T>> where T : class
    {
        public Option<Expression<Func<T, bool>>> Criteria { get; set; }
    }
}
