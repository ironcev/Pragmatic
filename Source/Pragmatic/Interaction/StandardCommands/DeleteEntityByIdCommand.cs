using System;

namespace Pragmatic.Interaction.StandardCommands
{
    public sealed class DeleteEntityByIdCommand : Command
    {
        public Guid EntityId { get; set; }

        private Type _entityType;
        public Type EntityType
        {
            get { return _entityType; }
            set
            {
                ArgumentCheck.EntityTypeRepresentsEntityType(value, "value");

                _entityType = value;
            }
        }
    }
}
