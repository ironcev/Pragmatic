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

        public static Response<Option<Entity>> CanDeleteEntity(this RequestExecutor requestExecutor, Type entityType, Guid entityId)
        {
            Argument.IsNotNull(requestExecutor, "requestExecutor");
            // The entityType argument is checked in the CanDeleteEntityRequest.EntityType setter.

            return requestExecutor.Execute(new CanDeleteEntityRequest { EntityId = entityId, EntityType = entityType });
        }

        public static Response<Option<Entity>> CanDeleteEntity(this RequestExecutor requestExecutor, CanDeleteEntityRequest canDeleteEntityRequest)
        {
            Argument.IsNotNull(requestExecutor, "requestExecutor");
            Argument.IsNotNull(canDeleteEntityRequest, "canDeleteEntityRequest");

            return requestExecutor.Execute(canDeleteEntityRequest);
        }
    }
}
