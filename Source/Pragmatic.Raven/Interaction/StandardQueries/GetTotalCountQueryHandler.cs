using System.Linq;
using Pragmatic.Interaction;
using Pragmatic.Interaction.StandardQueries;
using Raven.Client;
using Raven.Client.Linq;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Raven.Interaction.StandardQueries
{
    public sealed class GetTotalCountQueryHandler<T> : BaseQueryHandler, IQueryHandler<GetTotalCountQuery<T>, int> where T : class
    {
        public GetTotalCountQueryHandler(IDocumentSession documentSession) : base(documentSession) { }

        public int Execute(GetTotalCountQuery<T> query)
        {
            Argument.IsNotNull(query, "query");

            RavenQueryStatistics statistics;

            IRavenQueryable<T> ravenQuery = DocumentSession.Query<T>().Statistics(out statistics);

            if (query.Criteria.IsSome) ravenQuery = ravenQuery.Where(query.Criteria.Value);

            var dummyResult = ravenQuery.Take(0).ToArray();

            return statistics.TotalResults;
        }
    }
}
