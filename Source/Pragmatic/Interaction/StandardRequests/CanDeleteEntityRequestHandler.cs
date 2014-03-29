using Pragmatic.Interaction.EntityDeletion;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Interaction.StandardRequests
{
    public sealed class CanDeleteEntityRequestHandler<TEntity> : IRequestHandler<CanDeleteEntityRequest<TEntity>, Response<Option<TEntity>>> where TEntity : Entity
    {
        private readonly EntityDeleterProvider _entityDeleterProvider;

        public CanDeleteEntityRequestHandler(EntityDeleterProvider entityDeleterProvider)
        {
            Argument.IsNotNull(entityDeleterProvider, "entityDeleterProvider");

            _entityDeleterProvider = entityDeleterProvider;
        }

        public Response<Option<TEntity>> Execute(CanDeleteEntityRequest<TEntity> request)
        {
            Response response = new Response();

            var entityDeleter = _entityDeleterProvider.GetEntityDeleterFor<TEntity>();

            if (entityDeleter.IsNone)
            {
                response.AddError(() => EntityResources.DeletingOfEntitiesOfTypeIsNotForseen); // TODO-IG: Replace the generic word entity with the localized entity description.
                return Response<Option<TEntity>>.From(response);
            }

            return entityDeleter.Value.CanDeleteEntity(request.EntityId);
        }
    }

    public sealed class CanDeleteEntityRequestHandler : IRequestHandler<CanDeleteEntityRequest, Response<Option<Entity>>>
    {
        private readonly EntityDeleterProvider _entityDeleterProvider;

        public CanDeleteEntityRequestHandler(EntityDeleterProvider entityDeleterProvider)
        {
            Argument.IsNotNull(entityDeleterProvider, "entityDeleterProvider");

            _entityDeleterProvider = entityDeleterProvider;
        }

        public Response<Option<Entity>> Execute(CanDeleteEntityRequest request)
        {
            Response response = new Response();

            var entityDeleter = _entityDeleterProvider.GetEntityDeleterFor(request.EntityType);

            if (entityDeleter.IsNone)
            {
                response.AddError(() => EntityResources.DeletingOfEntitiesOfTypeIsNotForseen); // TODO-IG: Replace the generic word entity with the localized entity description.
                return Response<Option<Entity>>.From(response);
            }

            return entityDeleter.Value.CanDeleteEntity(request.EntityId);
        }
    }
}
