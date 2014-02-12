using Raven.Client;
using SwissKnife.Diagnostics.Contracts;

namespace TinyDdd.Raven
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDocumentSession _documentSession;

        /// <summary>
        /// Counts how many times the <see cref="Begin"/> method is called.
        /// </summary>
        private int _counter;

        public UnitOfWork(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public void Begin()
        {
            _counter++;
        }

        public void RegisterEntityToAddOrUpdate(IAggregateRoot entity)
        {
            Argument.IsNotNull(entity, "entity");
            Argument.Is<Entity>((object)entity, "entity" );
            CheckUnitOfWorkHasBegun();
            
            _documentSession.Store(entity);
        }

        public void RegisterEntityToDelete(IAggregateRoot entity)
        {
            Argument.IsNotNull(entity, "entity");
            Argument.Is<Entity>((object)entity, "entity");
            CheckUnitOfWorkHasBegun();
            
            _documentSession.Delete(entity);
        }

        public void Commit()
        {
            CheckUnitOfWorkHasBegun();

            if ( --_counter == 0 )
                _documentSession.SaveChanges();    
        }

        private void CheckUnitOfWorkHasBegun()
        {
            Operation.IsValid(_counter > 0, string.Format( "Unit of work has not begun. Unit of work must begin before any of its methods are called."));
        }
    }
}
