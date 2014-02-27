using System;
using SwissKnife;

namespace TinyDdd.Interaction
{
    public class GetByIdQuery<TEntity> : IQuery<Option<TEntity>> where TEntity : Entity, IAggregateRoot
    {
        public Guid Id { get; set; }
    }
}