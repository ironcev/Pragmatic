using System.Linq;
using Pragmatic.Interaction;
using Pragmatic.Interaction.StandardQueries;
using Raven.Client;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Raven.Interaction.StandardQueries
{
    public sealed class GetTotalCountQueryHandler<T> : BaseQueryHandler, IQueryHandler<GetTotalCountQuery<T>, int> where T : class
    {
        public GetTotalCountQueryHandler(IDocumentSession documentSession) : base(documentSession) { }

        public int Execute(GetTotalCountQuery<T> query)
        {
            Argument.IsNotNull(query, "query");

            return query.Criteria.IsSome ? DocumentSession.Query<T>().Where(query.Criteria.Value).Count() : DocumentSession.Query<T>().Count();
        }
    }
}
