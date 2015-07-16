using System;
using System.Linq.Expressions;
using Pragmatic.Interaction.StandardQueries;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Interaction
{
    public static class QueryExecutorExtensions
    {
        public static Option<T> GetById<T>(this QueryExecutor queryExecutor, Guid id) where T : class
        {
            Argument.IsNotNull(queryExecutor, "queryExecutor");

            return queryExecutor.Execute(new GetByIdQuery<T> { Id = id });
        }

        public static Option<object> GetById(this QueryExecutor queryExecutor, Type entityType, Guid id)
        {
            Argument.IsNotNull(queryExecutor, "queryExecutor");
            ArgumentCheck.EntityTypeRepresentsEntityType(entityType, "entityType");

            return queryExecutor.Execute(new GetByIdQuery { EntityType = entityType, EntityId = id });
        }

        public static Option<T> GetOne<T>(this QueryExecutor queryExecutor, Expression<Func<T, bool>> criteria) where T : class
        {
            Argument.IsNotNull(queryExecutor, "queryExecutor");
            Argument.IsNotNull(criteria, "criteria");

            return queryExecutor.Execute(new GetOneQuery<T> { Criteria = criteria });
        }

        public static Option<T> GetOne<T>(this QueryExecutor queryExecutor, Expression<Func<T, bool>> criteria, OrderBy<T> orderBy) where T : class
        {
            Argument.IsNotNull(queryExecutor, "queryExecutor");
            Argument.IsNotNull(criteria, "criteria");

            return queryExecutor.Execute(new GetOneQuery<T> { Criteria = criteria, OrderBy = orderBy });
        }

        public static IPagedEnumerable<T> GetAll<T>(this QueryExecutor queryExecutor) where T : class
        {
            Argument.IsNotNull(queryExecutor, "queryExecutor");

            return queryExecutor.Execute(new GetAllQuery<T>());
        }

        public static IPagedEnumerable<T> GetAll<T>(this QueryExecutor queryExecutor, Paging paging) where T : class
        {
            Argument.IsNotNull(queryExecutor, "queryExecutor");
            Argument.IsNotNull(paging, "paging");

            return queryExecutor.Execute(new GetAllQuery<T> { Paging = paging });
        }

        public static IPagedEnumerable<T> GetAll<T>(this QueryExecutor queryExecutor, OrderBy<T> orderBy) where T : class
        {
            Argument.IsNotNull(queryExecutor, "queryExecutor");
            Argument.IsNotNull(orderBy, "orderBy");

            return queryExecutor.Execute(new GetAllQuery<T> { OrderBy = orderBy });
        }

        public static IPagedEnumerable<T> GetAll<T>(this QueryExecutor queryExecutor, OrderBy<T> orderBy, Paging paging) where T : class
        {
            Argument.IsNotNull(queryExecutor, "queryExecutor");
            Argument.IsNotNull(orderBy, "orderBy");
            Argument.IsNotNull(paging, "paging");

            return queryExecutor.Execute(new GetAllQuery<T> { OrderBy = orderBy, Paging = paging });
        }

        public static IPagedEnumerable<T> GetAll<T>(this QueryExecutor queryExecutor, Expression<Func<T, bool>> criteria) where T : class
        {
            Argument.IsNotNull(queryExecutor, "queryExecutor");
            Argument.IsNotNull(criteria, "criteria");

            return queryExecutor.Execute(new GetAllQuery<T> { Criteria = criteria });
        }

        public static IPagedEnumerable<T> GetAll<T>(this QueryExecutor queryExecutor, Expression<Func<T, bool>> criteria, Paging paging) where T : class
        {
            Argument.IsNotNull(queryExecutor, "queryExecutor");
            Argument.IsNotNull(criteria, "criteria");
            Argument.IsNotNull(paging, "paging");

            return queryExecutor.Execute(new GetAllQuery<T> { Criteria = criteria, Paging = paging });
        }

        public static IPagedEnumerable<T> GetAll<T>(this QueryExecutor queryExecutor, Expression<Func<T, bool>> criteria, OrderBy<T> orderBy) where T : class
        {
            Argument.IsNotNull(queryExecutor, "queryExecutor");
            Argument.IsNotNull(criteria, "criteria");
            Argument.IsNotNull(orderBy, "orderBy");

            return queryExecutor.Execute(new GetAllQuery<T> { Criteria = criteria, OrderBy = orderBy });
        }

        public static IPagedEnumerable<T> GetAll<T>(this QueryExecutor queryExecutor, Expression<Func<T, bool>> criteria, OrderBy<T> orderBy, Paging paging) where T : class
        {
            Argument.IsNotNull(queryExecutor, "queryExecutor");
            Argument.IsNotNull(criteria, "criteria");
            Argument.IsNotNull(orderBy, "orderBy");
            Argument.IsNotNull(paging, "paging");

            return queryExecutor.Execute(new GetAllQuery<T> { Criteria = criteria, OrderBy = orderBy, Paging = paging });
        }

        public static int GetTotalCount<T>(this QueryExecutor queryExecutor) where T : class
        {
            Argument.IsNotNull(queryExecutor, "queryExecutor");

            return queryExecutor.Execute(new GetTotalCountQuery<T>());
        }

        public static int GetTotalCount<T>(this QueryExecutor queryExecutor, Expression<Func<T, bool>> criteria) where T : class
        {
            Argument.IsNotNull(queryExecutor, "queryExecutor");
            Argument.IsNotNull(criteria, "criteria");

            return queryExecutor.Execute(new GetTotalCountQuery<T> { Criteria = criteria });
        }
    }
}
