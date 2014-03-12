using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Pragmatic.Interaction.StandardQueries;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Interaction
{
    public static class QueryExecutorExstensions
    {
        public static Option<T> GetById<T>(this QueryExecutor queryExecutor, Guid id) where T : class
        {
            Argument.IsNotNull( queryExecutor, "queryExecutor" );

            return queryExecutor.Execute( new GetByIdQuery<T> { Id = id } );
        }

        public static Option<T> GetOne<T>(this QueryExecutor queryExecutor, Expression<Func<T, bool>> criteria) where T : class
        {
            Argument.IsNotNull( queryExecutor, "queryExecutor" );
            Argument.IsNotNull( criteria, "criteria" );

            return queryExecutor.Execute( new GetOneQuery<T> { Criteria = criteria } );
        }

        public static IEnumerable<T> GetAll<T>(this QueryExecutor queryExecutor) where T : class
        {
            Argument.IsNotNull( queryExecutor, "queryExecutor" );

            return queryExecutor.Execute( new GetAllQuery<T>() );
        }

        public static IEnumerable<T> GetAll<T>(this QueryExecutor queryExecutor, Expression<Func<T, bool>> criteria) where T : class
        {
            Argument.IsNotNull( queryExecutor, "queryExecutor" );
            Argument.IsNotNull( criteria, "criteria" );

            return queryExecutor.Execute( new GetAllQuery<T> { Criteria = criteria } );
        }
    }
}
