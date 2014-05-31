using NHibernate;
using Pragmatic.Interaction;
using Pragmatic.Interaction.StandardQueries;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.NHibernate.Interaction.StandardQueries
{
    public sealed class GetByIdQueryHandler<T> : BaseQueryHandler, IQueryHandler<GetByIdQuery<T>, Option<T>> where T : class
    {
        public GetByIdQueryHandler(ISession session) : base(session) { }

        public Option<T> Execute(GetByIdQuery<T> query)
        {
            Argument.IsNotNull(query, "query");

            return Session.Get<T>(query.Id);
        }
    }

    public sealed class GetByIdQueryHandler : BaseQueryHandler, IQueryHandler<GetByIdQuery, Option<object>>
    {
        public GetByIdQueryHandler(ISession session) : base(session) { }

        public Option<object> Execute(GetByIdQuery query)
        {
            Argument.IsNotNull(query, "query");

            return Session.Get(query.EntityType, query.EntityId); // TODO-IG: Again, EntityType could be null. Define how to deal with this situations.
        }
    }
}
