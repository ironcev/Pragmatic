using Raven.Client;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Raven.Interaction
{
    public abstract class BaseQuery
    {
        protected IDocumentSession DocumentSession { get; private set; }

        protected BaseQuery(IDocumentSession documentSession)
        {
            Argument.IsNotNull(documentSession, "documentSession");

            DocumentSession = documentSession;
        }
    }
}
