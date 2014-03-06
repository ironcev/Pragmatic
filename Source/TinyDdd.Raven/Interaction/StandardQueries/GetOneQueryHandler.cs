using System.Linq;
using Raven.Client;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;
using TinyDdd.Interaction;
using TinyDdd.Interaction.StandardQueries;

namespace TinyDdd.Raven.Interaction.StandardQueries
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
