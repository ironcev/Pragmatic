using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SwissKnife;

namespace TinyDdd
{
    /// <summary>
    /// Represents a read only repository of entities of the type <typeparam name="T"/>.
    /// </summary>
    public interface IReadOnlyRepository<T> where T : Entity, IAggregateRoot
    {
        /// <summary>
        /// Returns single <see cref="Entity"/> based on entity id.
        /// </summary>
        Option<T> GetById(Guid id);

        /// <summary>
        /// Returns single <see cref="Entity"/> that satisfies the <paramref name="criteria"/>.
        /// </summary>
        Option<T> GetOne(Expression<Func<T, bool>> criteria);

        /// <summary>
        /// Returns all entities.
        /// </summary>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Returns all entities that satisfy the <paramref name="criteria"/>.
        /// </summary>
        IEnumerable<T> GetAll(Expression<Func<T, bool>> criteria);
    }
}
