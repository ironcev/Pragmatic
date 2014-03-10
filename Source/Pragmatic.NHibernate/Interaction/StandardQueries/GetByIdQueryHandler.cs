using NHibernate;
using Pragmatic.Interaction;
using Pragmatic.Interaction.StandardQueries;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.NHibernate.Interaction.StandardQueries
{
    public sealed class GetByIdQueryHandler<T> : BaseQuery, IQueryHandler<GetByIdQuery<T>, Option<T>> where T : class
    {
        public GetByIdQueryHandler(ISession session) : base(session) { }

        public Option<T> Execute(GetByIdQuery<T> query)
        {
            Argument.IsNotNull(query, "query");

            return Session.Get<T>(query.Id);
        }
    }
}
