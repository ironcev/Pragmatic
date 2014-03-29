using System;
using Pragmatic.Interaction.StandardRequests;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Interaction
{
    public static class RequestExecutorExtensions
    {
        public static Response<Option<TEntity>> CanDeleteEntity<TEntity>(this RequestExecutor requestExecutor, Guid entityId) where TEntity : Entity
        {
            Argument.IsNotNull(requestExecutor, "requestExecutor");

            return requestExecutor.Execute(new CanDeleteEntityRequest<TEntity> { EntityId = entityId });
        }
    }
}
