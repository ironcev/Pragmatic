using Raven.Client;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Raven
{
    public class UnitOfWork : Pragmatic.UnitOfWork
    {
        private readonly IDocumentSession _documentSession;

        public UnitOfWork(IDocumentSession documentSession)
        {
            Argument.IsNotNull(documentSession, "documentSession");

            _documentSession = documentSession;
        }

        protected override void MarkEntityAsAdded(Entity entity)
        {
            _documentSession.Store(entity);
        }

        protected override void MarkEntityAsUpdated(Entity entity)
        {
            _documentSession.Store(entity);
        }

        protected override void MarkEntityAsDeleted(Entity entity)
        {
            _documentSession.Delete(entity);
        }

        protected override void SaveMarkedChanges()
        {
            _documentSession.SaveChanges();
        }
    }
}
