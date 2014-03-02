using SwissKnife.Diagnostics.Contracts;

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
    public abstract class UnitOfWork
    {
        /// <summary>
        /// Counts how many times the <see cref="Begin"/> method is called.
        /// </summary>
        private int _counter;

        public void Begin()
        {
            _counter++;
        }

        public void RegisterEntityToAddOrUpdate(IAggregateRoot entity)
        {
            Argument.IsNotNull(entity, "entity");
            Argument.Is<Entity>((object)entity, "entity");
            CheckUnitOfWorkHasBegun();

            RegisterEntityToAddOrUpdateCore(entity);
        }

        public abstract void RegisterEntityToAddOrUpdateCore(IAggregateRoot entity);

        public void RegisterEntityToDelete(IAggregateRoot entity)
        {
            Argument.IsNotNull(entity, "entity");
            Argument.Is<Entity>((object)entity, "entity");
            CheckUnitOfWorkHasBegun();

            RegisterEntityToDeleteCore(entity);
        }

        public abstract void RegisterEntityToDeleteCore(IAggregateRoot entity);

        public void Commit()
        {
            CheckUnitOfWorkHasBegun();

            if (--_counter == 0)
                CommitCore();
        }

        public abstract void CommitCore();

        private void CheckUnitOfWorkHasBegun()
        {
            Operation.IsValid(_counter > 0, string.Format("Unit of work has not begun. Unit of work must begin before any of its methods are called.")); // TODO-IG: Add to the message that the Begin() method has to be called after the Identifier supports method names with brackets.
        }
    }
}
