using NHibernate;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;
using TinyDdd.Interaction;
using TinyDdd.Interaction.StandardQueries;

namespace TinyDdd.NHibernate.Interaction.StandardQueries
{
    public sealed class GetOneQueryHandler<T> : BaseQuery, IQueryHandler<GetOneQuery<T>, Option<T>> where T : class
    {
        public GetOneQueryHandler(ISession session) : base( session ) { }

        public Option<T> Execute(GetOneQuery<T> query)
        {
            Argument.IsNotNull( query, "query" );

            return Session.QueryOver<T>().Where( query.Criteria ).Take( 1 ).SingleOrDefault();
        }
    }
}
