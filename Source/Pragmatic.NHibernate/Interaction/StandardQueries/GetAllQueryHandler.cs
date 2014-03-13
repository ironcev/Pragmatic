using System.Collections.Generic;
using NHibernate;
using Pragmatic.Interaction;
using Pragmatic.Interaction.StandardQueries;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.NHibernate.Interaction.StandardQueries
{
    public sealed class GetAllQueryHandler<T> : BaseQuery, IQueryHandler<GetAllQuery<T>, IEnumerable<T>> where T : class
    {
        public GetAllQueryHandler(ISession session) : base(session) { }

        public IEnumerable<T> Execute(GetAllQuery<T> query)
        {
            Argument.IsNotNull(query, "query");

            return query.Criteria.IsSome ? Session.QueryOver<T>().Where(query.Criteria.Value).List() : Session.QueryOver<T>().List();
        }
    }
}
