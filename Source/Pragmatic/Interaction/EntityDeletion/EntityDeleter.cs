using System;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Interaction.EntityDeletion
{
    public abstract class EntityDeleter<TEntity> where TEntity : Entity
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

        public Response<Option<TEntity>> CanDeleteEntity(TEntity entity)
        {
            Argument.IsNotNull(entity, "entity");

            return CanDeleteEntityCore(entity);
        }

        protected abstract Response<Option<TEntity>> CanDeleteEntityCore(TEntity entity);
    }
}
