using System;
using System.Linq.Expressions;
using SwissKnife;

namespace TinyDdd.Interaction.StandardQueries
{
    public class GetOneQuery<TEntity> : IQuery<Option<TEntity>> where TEntity : Entity, IAggregateRoot
    {
        public Expression<Func<TEntity, bool>> Criteria { get; set; }
    }
}