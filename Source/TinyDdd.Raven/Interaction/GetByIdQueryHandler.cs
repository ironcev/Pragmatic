using Raven.Client;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;
using TinyDdd.Interaction;

namespace TinyDdd.Raven.Interaction
{
    public class GetByIdQueryHandler<TEntity> : BaseQuery, IQueryHandler<GetByIdQuery<TEntity>, Option<TEntity>> where TEntity : Entity, IAggregateRoot
    {
        public GetByIdQueryHandler(IDocumentSession documentSession) : base(documentSession) { }

        public Option<TEntity> Execute(GetByIdQuery<TEntity> query)
        {
            Argument.IsNotNull(query, "query");

            return DocumentSession.Load<TEntity>(query.Id);
        }
    }
}
