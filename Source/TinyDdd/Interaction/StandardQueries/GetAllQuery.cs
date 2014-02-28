using System.Collections.Generic;

namespace TinyDdd.Interaction.StandardQueries
{
    public class GetAllQuery<TEntity> : IQuery<IEnumerable<TEntity>> where TEntity : Entity, IAggregateRoot
    {
    }
}
