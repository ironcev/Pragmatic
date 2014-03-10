using System.Collections.Generic;
using Pragmatic.Interaction;
using Pragmatic.Interaction.StandardQueries;
using Raven.Client;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Raven.Interaction.StandardQueries
{
    public sealed class GetAllQueryHandler<T> : BaseQuery, IQueryHandler<GetAllQuery<T>, IEnumerable<T>> where T : class
    {
        public GetAllQueryHandler(IDocumentSession documentSession) : base(documentSession) { }

        public IEnumerable<T> Execute(GetAllQuery<T> query)
        {
            Argument.IsNotNull(query, "query");

            return DocumentSession.Query<T>();
        }
    }
}
