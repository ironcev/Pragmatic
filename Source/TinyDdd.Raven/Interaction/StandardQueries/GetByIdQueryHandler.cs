using Raven.Client;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;
using TinyDdd.Interaction;
using TinyDdd.Interaction.StandardQueries;

namespace TinyDdd.Raven.Interaction.StandardQueries
{
    public class GetByIdQueryHandler<T> : BaseQuery, IQueryHandler<GetByIdQuery<T>, Option<T>> where T : class
    {
        public GetByIdQueryHandler(IDocumentSession documentSession) : base(documentSession) { }

        public Option<T> Execute(GetByIdQuery<T> query)
        {
            Argument.IsNotNull(query, "query");

            return DocumentSession.Load<T>(query.Id);
        }
    }
}
