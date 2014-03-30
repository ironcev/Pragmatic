using System;
using SwissKnife.Diagnostics.Contracts;

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
                Argument.IsNotNull(value, "value"); // TODO-IG: Code duplication!
                Argument.IsValid(typeof(Entity).IsAssignableFrom(value),
                                 string.Format("Entity type does not derive from '{0}'. Entity type must derive from '{0}'. The entity type is: '{1}'.", typeof(Entity), value),
                                 "entityType");

                _entityType = value;
            }
        }
    }
}
