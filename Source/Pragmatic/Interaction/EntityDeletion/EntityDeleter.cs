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

        Response IEntityDeleter.DeleteEntity(Entity entity)
        {
            Argument.IsNotNull(entity, "entity");
            Argument.IsValid(entity is TEntity,
                             string.Format("The entity deleter '{0}' can delete only entities of type '{1}'. The provided entity is of type '{2}'.", GetType(), typeof(TEntity), entity.GetType()),
                             "entity");

            return DeleteEntity((TEntity)entity);
        }

        public Response DeleteEntity(Guid entityId)
        {
            return DeleteEntityUponResponseOfCanDeleteEntityRequest(CanDeleteEntity(entityId));
        }

        public Response DeleteEntity(TEntity entity)
        {
            Argument.IsNotNull(entity, "entity");

            return DeleteEntityUponResponseOfCanDeleteEntityRequest(CanDeleteEntity(entity));
        }

        private Response DeleteEntityUponResponseOfCanDeleteEntityRequest(Response<Option<TEntity>> canDeleteEntityResponse)
        {
            if (canDeleteEntityResponse.HasErrors || canDeleteEntityResponse.Result.IsNone)
                return canDeleteEntityResponse;

            return DeleteEntityCore(canDeleteEntityResponse.Result.Value);    
        }

        protected virtual Response DeleteEntityCore(TEntity entity)
        {
            UnitOfWork.Begin();
            UnitOfWork.RegisterEntityToDelete(entity);
            UnitOfWork.Commit();

            return new Response();
        }

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
}
