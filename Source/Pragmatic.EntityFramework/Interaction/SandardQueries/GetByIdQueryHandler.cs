using System.Data.Entity;
using Pragmatic.Interaction;
using Pragmatic.Interaction.StandardQueries;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.EntityFramework.Interaction.SandardQueries
{
    public class GetByIdQueryHandler<T> : BaseQueryHandler, IQueryHandler<GetByIdQuery<T>, Option<T>> where T : class
    {
        public GetByIdQueryHandler(DbContext dbContext) : base(dbContext)
        {
        }

        public Option<T> Execute(GetByIdQuery<T> query)
        {
            Argument.IsNotNull(query, "query");

            return DbContext.Set<T>().Find(query.Id);
        }
    }
}