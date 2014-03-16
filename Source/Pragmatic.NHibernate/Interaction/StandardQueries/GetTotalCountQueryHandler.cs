using NHibernate;
using Pragmatic.Interaction;
using Pragmatic.Interaction.StandardQueries;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.NHibernate.Interaction.StandardQueries
{
    public sealed class GetTotalCountQueryHandler<T> : BaseQueryHandler, IQueryHandler<GetTotalCountQuery<T>, int> where T : class
    {
        public GetTotalCountQueryHandler(ISession session) : base(session) { }

        public int Execute(GetTotalCountQuery<T> query)
        {
            Argument.IsNotNull(query, "query");

            return query.Criteria.IsSome ? Session.QueryOver<T>().Where(query.Criteria.Value).RowCount() : Session.QueryOver<T>().RowCount();
        }
    }
}
