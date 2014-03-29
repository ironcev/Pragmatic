using System;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Interaction.EntityDeletion
{
    public abstract class EntityDeleter<TEntity> : IEntityDeleter where TEntity : Entity
    {
        protected UnitOfWork UnitOfWork { get; private set; }
        protected QueryExecutor QueryExecutor { get; private set; }

        protected EntityDeleter(UnitOfWork unitOfWork, QueryExecutor queryExecutor)
        {
            Argument.IsNotNull(unitOfWork, "unitOfWork");
            Argument.IsNotNull(queryExecutor, "queryExecutor");

            UnitOfWork = unitOfWork;
            QueryExecutor = queryExecutor;
        }

        void IEntityDeleter.DeleteEntity(Entity entity)
        {
            Argument.IsNotNull(entity, "entity");
            Argument.IsValid(entity is TEntity,
                             string.Format("The entity deleter '{0}' can delete only entities of type '{1}'. The provided entity is of type '{2}'", GetType(), typeof(TEntity), entity.GetType()),
                             "entity");

            DeleteEntity((TEntity)entity);
        }

        public void DeleteEntity(Guid entityId)
        {
            DeleteEntityUponResponseOfCanDeleteEntityRequest(CanDeleteEntity(entityId), entityId);
        }

        public void DeleteEntity(TEntity entity)
        {
            Argument.IsNotNull(entity, "entity");

            DeleteEntityUponResponseOfCanDeleteEntityRequest(CanDeleteEntity(entity), entity.Id);
        }

        private void DeleteEntityUponResponseOfCanDeleteEntityRequest(Response<Option<TEntity>> response, Guid entityId)
        {
            Operation.IsValid(!response.HasErrors && response.Result.IsSome,
                               string.Format("The entity of type '{0}' cannot be deleted. The entity id is: {1}.", typeof(TEntity).FullName, entityId));

            DeleteEntityCore(response.Result.Value);            
        }

        protected abstract void DeleteEntityCore(TEntity entity);

        public Response<Option<TEntity>> CanDeleteEntity(Guid entityId)
        {
            Response response = new Response();

            Option<TEntity> entity = QueryExecutor.GetById<TEntity>(entityId);
           
            if (entity.IsNone)
            {
                response.AddError(() => EntityResources.EntityDoesNotExistInTheSystem); // TODO-IG: Replace the generic word entity with the localized entity description.
                return Response<Option<TEntity>>.From(response);
            }

            return CanDeleteEntityCore(entity.Value);
        }

        Response<Option<Entity>> IEntityDeleter.CanDeleteEntity(Guid entityId)
        {
            Response<Option<TEntity>> response = CanDeleteEntity(entityId);
            Option<Entity> entity = response.Result.MapToOption(result => (Entity)result);
            Response<Option<Entity>> convertedResponse = new Response<Option<Entity>>(entity);
            convertedResponse.Add(response); // Add original messages to the converted response.
            return convertedResponse;
        }

        public Response<Option<TEntity>> CanDeleteEntity(TEntity entity)
        {
            Argument.IsNotNull(entity, "entity");

            return CanDeleteEntityCore(entity);
        }

        protected abstract Response<Option<TEntity>> CanDeleteEntityCore(TEntity entity);
    }

    public interface IEntityDeleter
    {
        void DeleteEntity(Guid entityId);
        void DeleteEntity(Entity entity);
        Response<Option<Entity>> CanDeleteEntity(Guid entityId);
    }
}
