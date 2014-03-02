using Raven.Client;
using SwissKnife.Diagnostics.Contracts;

namespace TinyDdd.Raven
{
    public class UnitOfWork : TinyDdd.UnitOfWork
    {
        private readonly IDocumentSession _documentSession;

        public UnitOfWork(IDocumentSession documentSession)
        {
            Argument.IsNotNull(documentSession, "documentSession");

            _documentSession = documentSession;
        }

        public override void RegisterEntityToAddOrUpdateCore(IAggregateRoot entity)
        {
            _documentSession.Store(entity);
        }

        public override void RegisterEntityToDeleteCore(IAggregateRoot entity)
        {
            _documentSession.Delete(entity);
        }

        public override void CommitCore()
        {
            _documentSession.SaveChanges();
        }
    }
}
