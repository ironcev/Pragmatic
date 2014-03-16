using Raven.Client;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Raven.Interaction
{
    public abstract class BaseQueryHandler
    {
        protected IDocumentSession DocumentSession { get; private set; }

        protected BaseQueryHandler(IDocumentSession documentSession)
        {
            Argument.IsNotNull(documentSession, "documentSession");

            DocumentSession = documentSession;
        }
    }
}
