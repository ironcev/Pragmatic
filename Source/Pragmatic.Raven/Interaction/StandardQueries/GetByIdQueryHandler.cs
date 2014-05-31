using System;
using Pragmatic.Interaction;
using Pragmatic.Interaction.StandardQueries;
using Raven.Client;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Raven.Interaction.StandardQueries
{
    public sealed class GetByIdQueryHandler<T> : BaseQueryHandler, IQueryHandler<GetByIdQuery<T>, Option<T>> where T : class
    {
        public GetByIdQueryHandler(IDocumentSession documentSession) : base(documentSession) { }

        public Option<T> Execute(GetByIdQuery<T> query)
        {
            Argument.IsNotNull(query, "query");

            return DocumentSession.Load<T>(query.Id);
        }
    }

    public sealed class GetByIdQueryHandler : BaseQueryHandler, IQueryHandler<GetByIdQuery, Option<object>>
    {
        public GetByIdQueryHandler(IDocumentSession documentSession) : base(documentSession) { }

        public Option<object> Execute(GetByIdQuery query)
        {
            Argument.IsNotNull(query, "query");

            // RavenDB does not have non-generic equivalent of the Load<T>() method. That's why this trick with reflection.

            // TODO-IG: Use SwissKnife.Identifier once when support for methods is implemented.
            var loadMethod = typeof(IDocumentSession).GetMethod("Load", new [] { typeof(ValueType)}, null)
                                                     .MakeGenericMethod(query.EntityType); // TODO-IG: Like on all other queries, check for arguments. EntityType could be null if it is not set. Find common way to deal with these situations.

            return loadMethod.Invoke(DocumentSession, new object[] { query.EntityId });
        }
    }
}
