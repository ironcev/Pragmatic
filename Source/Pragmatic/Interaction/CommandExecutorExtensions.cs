using System;
using Pragmatic.Interaction.StandardCommands;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Interaction
{
    public static class CommandExecutorExtensions
    {
        public static Response DeleteEntity<TEntity>(this CommandExecutor commandExecutor, TEntity entityToDelete) where TEntity : Entity
        {
            Argument.IsNotNull(commandExecutor, "commandExecutor");
            Argument.IsNotNull(entityToDelete, "entityToDelete");

            return commandExecutor.Execute(new DeleteEntityCommand<TEntity> { EntityToDelete = entityToDelete });
        }

        public static Response DeleteEntity(this CommandExecutor commandExecutor, Entity entityToDelete)
        {
            Argument.IsNotNull(commandExecutor, "commandExecutor");
            Argument.IsNotNull(entityToDelete, "entityToDelete");

            return commandExecutor.Execute(new DeleteEntityCommand { EntityToDelete = entityToDelete });
        }

        public static Response DeleteEntity<TEntity>(this CommandExecutor commandExecutor, Guid entityId) where TEntity : Entity
        {
            Argument.IsNotNull(commandExecutor, "commandExecutor");

            return commandExecutor.Execute(new DeleteEntityByIdCommand { EntityId = entityId, EntityType = typeof(TEntity) });
        }

        public static Response DeleteEntity(this CommandExecutor commandExecutor, Type entityType, Guid entityId)
        {
            Argument.IsNotNull(commandExecutor, "commandExecutor");
            Argument.IsNotNull(entityType, "entityType"); // TODO-IG: Code duplication!
            Argument.IsValid(typeof(Entity).IsAssignableFrom(entityType),
                             string.Format("Entity type does not derive from '{0}'. Entity type must derive from '{0}'. The entity type is: '{1}'.", typeof(Entity), entityType),
                             "entityType");


            return commandExecutor.Execute(new DeleteEntityByIdCommand { EntityId = entityId, EntityType = entityType });
        }
    }
}
