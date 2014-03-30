using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Interaction.StandardCommands
{
    public sealed class DeleteEntityCommand<TEntity> : Command where TEntity : Entity
    {
        private TEntity _entityToDelete;

        public TEntity EntityToDelete
        {
            get { return _entityToDelete; }
            set
            {
                Argument.IsNotNull(value, "value");

                _entityToDelete = value;
            }
        }
    }

    // Normally, we would simply have this DeleteEntityCommand : DeleteEntityCommand<Entity> { }.
    // In this case we cannot do that because DeleteEntityCommand<Entity> is sealed since all commands have to be sealed.
    public sealed class DeleteEntityCommand : Command
    {
        private Entity _entityToDelete;

        public Entity EntityToDelete
        {
            get { return _entityToDelete; }
            set
            {
                Argument.IsNotNull(value, "value");

                _entityToDelete = value;
            }
        }
    }
}
