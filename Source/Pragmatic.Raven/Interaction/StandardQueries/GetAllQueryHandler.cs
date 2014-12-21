using System.Linq;
using Pragmatic.Interaction;
using Pragmatic.Interaction.StandardQueries;
using Raven.Client;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Raven.Interaction.StandardQueries
{
    public sealed class GetAllQueryHandler<T> : BaseQueryHandler, IQueryHandler<GetAllQuery<T>, IPagedEnumerable<T>> where T : class
    {
        public GetAllQueryHandler(IDocumentSession documentSession) : base(documentSession) { }

        public IPagedEnumerable<T> Execute(GetAllQuery<T> query)
        {
            Argument.IsNotNull(query, "query");

            return query.Criteria.IsSome ? 
                   DocumentSession.Query<T>().OrderBy(query.OrderBy).Where(query.Criteria.Value).ToPagedEnumerable(query.Paging) : 
                   DocumentSession.Query<T>().OrderBy(query.OrderBy).ToPagedEnumerable(query.Paging);
        }
    }
}
