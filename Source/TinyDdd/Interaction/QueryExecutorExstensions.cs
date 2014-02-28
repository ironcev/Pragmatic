using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;
using TinyDdd.Interaction.StandardQueries;

namespace TinyDdd.Interaction
{
    public static class QueryExecutorExstensions
    {
        public static Option<TEntity> GetById<TEntity>(this QueryExecutor queryExecutor, Guid id) where TEntity : Entity, IAggregateRoot
        {
            Argument.IsNotNull(queryExecutor, "queryExecutor");

            return queryExecutor.Execute(new GetByIdQuery<TEntity> {Id = id});
        }

        public static Option<TEntity> GetOne<TEntity>(this QueryExecutor queryExecutor, Expression<Func<TEntity, bool>> criteria) where TEntity : Entity, IAggregateRoot
        {
            Argument.IsNotNull(queryExecutor, "queryExecutor");
            Argument.IsNotNull(criteria, "criteria");

            return queryExecutor.Execute(new GetOneQuery<TEntity> { Criteria = criteria });
        }

        public static IEnumerable<TEntity> GetAll<TEntity>(this QueryExecutor queryExecutor) where TEntity : Entity, IAggregateRoot
        {
            Argument.IsNotNull(queryExecutor, "queryExecutor");

            return queryExecutor.Execute(new GetAllQuery<TEntity>());
        }
    }
}
