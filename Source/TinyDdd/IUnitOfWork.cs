namespace TinyDdd
{
    /// <summary>
    /// Exposes methods which enable registration of entities that need to be saved or deleted from repositories in a single transaction.
    /// Unit of work supports nested commits i.e. multiple calls to <see cref="Commit"/> on single unit of work instance.
    /// Only the last <see cref="Commit"/> will be fully executed.
    /// We assume that there is only one unit of work instance running in a single transaction.
    /// For example, in a web application we will usually have single unit of work instance per HTTP request.
    /// In a desktop application we could have single unit of work per tread.
    /// Unit of work instance should perform its own lookup for concrete repositories involved in transaction.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Marks the starting point of unit of work.
        /// </summary>
        void Begin();

        /// <summary>
        /// Registers the <paramref name="entity"/> to be added or updated.
        /// </summary>
        void RegisterEntityToAddOrUpdate(IAggregateRoot entity);

        /// <summary>
        /// Registers the <paramref name="entity"/> to be deleted.
        /// </summary>
        void RegisterEntityToDelete(IAggregateRoot entity);
        
        /// <summary>
        /// Saves and deletes all registered entities on all repositories in a single transaction.
        /// </summary>
        void Commit();
    }
}