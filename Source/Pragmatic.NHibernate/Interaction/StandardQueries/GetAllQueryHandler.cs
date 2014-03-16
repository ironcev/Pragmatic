using NHibernate;
using Pragmatic.Interaction;
using Pragmatic.Interaction.StandardQueries;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.NHibernate.Interaction.StandardQueries
{
    public sealed class GetAllQueryHandler<T> : BaseQueryHandler, IQueryHandler<GetAllQuery<T>, IPagedEnumerable<T>> where T : class
    {
        public GetAllQueryHandler(ISession session) : base(session) { }

        public IPagedEnumerable<T> Execute(GetAllQuery<T> query)
        {
            Argument.IsNotNull(query, "query");

            var queryOver = Session.QueryOver<T>().OrderBy(query.OrderBy);

            // Constant expressions are not allowed so we are falling back to standard if check. E.g. this is not possible: var where = query.Criteria.IsSome ? query.Criteria.Value : t => true;
            if (query.Criteria.IsSome) queryOver.Where(query.Criteria.Value);

            return queryOver.ToPagedEnumerable(query.Paging);
        }
    }
}
