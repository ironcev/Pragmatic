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
}
