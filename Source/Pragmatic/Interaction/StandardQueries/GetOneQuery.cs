using System;
using System.Linq.Expressions;
using SwissKnife;

namespace Pragmatic.Interaction.StandardQueries
{
    public sealed class GetOneQuery<T> : IQuery<Option<T>> where T : class
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
    }
}