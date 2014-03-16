using NHibernate;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.NHibernate.Interaction
{
    public abstract class BaseQueryHandler
    {
        protected ISession Session { get; private set; }

        protected BaseQueryHandler(ISession session)
        {
            Argument.IsNotNull(session, "session");

            Session = session;
        }
    }
}
