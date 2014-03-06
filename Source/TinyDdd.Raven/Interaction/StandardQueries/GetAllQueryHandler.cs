using System.Collections.Generic;
using Raven.Client;
using SwissKnife.Diagnostics.Contracts;
using TinyDdd.Interaction;
using TinyDdd.Interaction.StandardQueries;

namespace TinyDdd.Raven.Interaction.StandardQueries
{
    public class GetAllQueryHandler<T> : BaseQuery, IQueryHandler<GetAllQuery<T>, IEnumerable<T>> where T : class
    {
        public GetAllQueryHandler(IDocumentSession documentSession) : base(documentSession) { }

        public IEnumerable<T> Execute(GetAllQuery<T> query)
        {
            Argument.IsNotNull(query, "query");

            return DocumentSession.Query<T>();
        }
    }
}
