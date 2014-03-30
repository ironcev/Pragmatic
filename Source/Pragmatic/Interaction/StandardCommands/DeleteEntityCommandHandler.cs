using Pragmatic.Interaction.EntityDeletion;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Interaction.StandardCommands
{
    public sealed class DeleteEntityCommandHandler<TEntity> : ICommandHandler<DeleteEntityCommand<TEntity>, Response> where TEntity : Entity
    {
        private readonly EntityDeleterProvider _entityDeleterProvider;

        public DeleteEntityCommandHandler(EntityDeleterProvider entityDeleterProvider)
        {
            Argument.IsNotNull(entityDeleterProvider, "entityDeleterProvider");

            _entityDeleterProvider = entityDeleterProvider;
        }

        public Response Execute(DeleteEntityCommand<TEntity> command)
        {
            Response response = new Response();

            var entityDeleter = _entityDeleterProvider.GetEntityDeleterFor<TEntity>();

            if (entityDeleter.IsNone)
            {
                response.AddError(() => EntityResources.DeletingEntitiesOfTypeIsNotForseen); // TODO-IG: Replace the generic word entity with the localized entity description.
                return response;
            }

            // We want this to throw exception if the entity cannot be deleted.
            entityDeleter.Value.DeleteEntity(command.EntityToDelete);

            // In case of a successful deletion an empty response is returned. 
            return response;
        }
    }

    public sealed class DeleteEntityCommandHandler : ICommandHandler<DeleteEntityCommand, Response>
    {
        private readonly EntityDeleterProvider _entityDeleterProvider;

        public DeleteEntityCommandHandler(EntityDeleterProvider entityDeleterProvider)
        {
            Argument.IsNotNull(entityDeleterProvider, "entityDeleterProvider");

            _entityDeleterProvider = entityDeleterProvider;
        }

        public Response Execute(DeleteEntityCommand command)
        {
            Response response = new Response();

            var entityDeleter = _entityDeleterProvider.GetEntityDeleterFor(command.EntityToDelete.GetType());

            if (entityDeleter.IsNone)
            {
                response.AddError(() => EntityResources.DeletingEntitiesOfTypeIsNotForseen); // TODO-IG: Replace the generic word entity with the localized entity description.
                return response;
            }

            // We want this to throw exception if the entity cannot be deleted.
            entityDeleter.Value.DeleteEntity(command.EntityToDelete);

            // In case of a successful deletion an empty response is returned. 
            return response;
        }
    }
}
