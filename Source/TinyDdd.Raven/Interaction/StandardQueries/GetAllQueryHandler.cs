using System.Collections.Generic;
using Raven.Client;
using SwissKnife.Diagnostics.Contracts;
using TinyDdd.Interaction;
using TinyDdd.Interaction.StandardQueries;

namespace TinyDdd.Raven.Interaction.StandardQueries
{
    public class GetAllQueryHandler<TEntity> : BaseQuery, IQueryHandler<GetAllQuery<TEntity>, IEnumerable<TEntity>> where TEntity : Entity, IAggregateRoot
    {
        public GetAllQueryHandler(IDocumentSession documentSession) : base(documentSession) { }

        public IEnumerable<TEntity> Execute(GetAllQuery<TEntity> query)
        {
            Argument.IsNotNull(query, "query");

            return DocumentSession.Query<TEntity>();
        }
    }
}
