using System.Linq;
using Pragmatic.Interaction;
using Pragmatic.Interaction.StandardQueries;
using Raven.Client;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Raven.Interaction.StandardQueries
{
    public sealed class GetOneQueryHandler<T> : BaseQuery, IQueryHandler<GetOneQuery<T>, Option<T>> where T : class
    {
        public GetOneQueryHandler(IDocumentSession documentSession) : base(documentSession) { }

        public Option<T> Execute(GetOneQuery<T> query)
        {
            Argument.IsNotNull(query, "query");

            return DocumentSession.Query<T>().FirstOrDefault(query.Criteria);
        }
    }
}
