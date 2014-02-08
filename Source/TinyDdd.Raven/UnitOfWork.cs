using Raven.Client;

namespace TinyDdd.Raven
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDocumentSession _documentSession;

        public UnitOfWork(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public void Begin()
        {
            
        }

        public void RegisterEntityToAddOrUpdate(IAggregateRoot entity)
        {
            _documentSession.Store(entity);
        }

        public void RegisterEntityToDelete(IAggregateRoot entity)
        {
            _documentSession.Delete(entity);
        }

        public void Commit()
        {
            _documentSession.SaveChanges();
        }
    }
}
