using System.Data.Entity;
using Pragmatic.Interaction;
using Pragmatic.Interaction.StandardQueries;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.EntityFramework.Interaction.StandardQueries
{
    public class GetByIdQueryHandler<T> : BaseQueryHandler, IQueryHandler<GetByIdQuery<T>, Option<T>> where T : class
    {
        public GetByIdQueryHandler(DbContext dbContext) : base(dbContext) { }

        public Option<T> Execute(GetByIdQuery<T> query)
        {
            Argument.IsNotNull(query, "query");

            return DbContext.Set<T>().Find(query.Id);
        }
    }

    public sealed class GetByIdQueryHandler : BaseQueryHandler, IQueryHandler<GetByIdQuery, Option<object>>
    {
        public GetByIdQueryHandler(DbContext dbContext) : base(dbContext) { }

        public Option<object> Execute(GetByIdQuery query)
        {
            Argument.IsNotNull(query, "query");

            return DbContext.Set(query.EntityType).Find(query.EntityId); // TODO-IG: Again, EntityType could be null. Define how to deal with this situations.
        }
    }
}