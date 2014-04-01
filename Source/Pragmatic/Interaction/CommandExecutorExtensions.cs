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
            // The check for entity type is done in the constructor of the DeleteEntityByIdCommand.

            return commandExecutor.Execute(new DeleteEntityByIdCommand { EntityId = entityId, EntityType = entityType });
        }

        public static Response DeleteEntity(this CommandExecutor commandExecutor, DeleteEntityByIdCommand deleteEntityByIdCommand)
        {
            Argument.IsNotNull(commandExecutor, "commandExecutor");
            Argument.IsNotNull(deleteEntityByIdCommand, "deleteEntityByIdCommand");

            return commandExecutor.Execute(deleteEntityByIdCommand);
        }
    }
}
