using System.Collections.Generic;
using NHibernate;
using SwissKnife.Diagnostics.Contracts;
using TinyDdd.Interaction;
using TinyDdd.Interaction.StandardQueries;

namespace TinyDdd.NHibernate.Interaction.StandardQueries
{
    public sealed class GetAllQueryHandler<T> : BaseQuery, IQueryHandler<GetAllQuery<T>, IEnumerable<T>> where T : class
    {
        public GetAllQueryHandler(ISession session) : base(session) { }

        public IEnumerable<T> Execute(GetAllQuery<T> query)
        {
            Argument.IsNotNull(query, "query");

            return Session.QueryOver<T>().List();
        }
    }
}
