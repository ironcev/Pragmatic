using NHibernate;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.NHibernate
{
    public class UnitOfWork : Pragmatic.UnitOfWork
    {
        private readonly ISession _session;

        public UnitOfWork(ISession session)
        {
            Argument.IsNotNull( session, "session" );

            _session = session;
        }

        protected override void MarkEntityAsAdded(Entity entity)
        {
            _session.Save( entity );
        }

        protected override void MarkEntityAsUpdated(Entity entity)
        {
            _session.Update( entity );
        }

        protected override void MarkEntityAsDeleted(Entity entity)
        {
            _session.Delete( entity );
        }

        protected override void SaveMarkedChanges()
        {
            using ( var transaction = _session.BeginTransaction() )
            {
                try
                {
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
