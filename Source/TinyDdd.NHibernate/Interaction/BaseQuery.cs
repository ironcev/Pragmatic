using NHibernate;
using SwissKnife.Diagnostics.Contracts;

namespace TinyDdd.NHibernate.Interaction
{
    public abstract class BaseQuery
    {
        protected ISession Session { get; private set; }

        protected BaseQuery(ISession session)
        {
            Argument.IsNotNull(session, "session");

            Session = session;
        }
    }
}
