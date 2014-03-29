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
    }
}
