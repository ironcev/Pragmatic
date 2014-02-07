namespace TinyDdd
{
    /// <summary>
    /// Represents repository of entities of the type <typeparam name="T"/>.
    /// </summary>
    public interface IRepository<T> : IRepository, IReadOnlyRepository<T> where T : Entity, IAggregateRoot
    {
        /// <summary>
        /// Adds new <paramref name="entity"/> to the repository or updates an existing one.
        /// </summary>
        void AddOrUpdate(T entity);

        /// <summary>
        /// Deletes the <paramref name="entity"/> from the repository.
        /// </summary>
        void Delete(T entity);
    }

    /// <summary>
    /// Represents repository of <see cref="IAggregateRoot"/>s.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Adds new <paramref name="aggregateRoot"/> to the repository or updates an existing one.
        /// </summary>
        void AddOrUpdate(IAggregateRoot aggregateRoot);

        /// <summary>
        /// Deletes the <paramref name="aggregateRoot"/> from the repository.
        /// </summary>
        void Delete(IAggregateRoot aggregateRoot);
    }
}
