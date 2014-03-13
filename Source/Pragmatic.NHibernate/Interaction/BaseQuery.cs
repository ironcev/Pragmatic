using System.Linq;
using NHibernate;
using Pragmatic.Interaction;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.NHibernate.Interaction
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
