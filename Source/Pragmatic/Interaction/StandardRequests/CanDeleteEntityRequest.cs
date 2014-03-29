using System;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Interaction.StandardRequests
{
    public sealed class CanDeleteEntityRequest<TEntity> : IRequest<Response<Option<TEntity>>> where TEntity : Entity
    {
        public Guid EntityId { get; set; }
    }

    public sealed class CanDeleteEntityRequest : IRequest<Response<Option<Entity>>>
    {
        public Guid EntityId { get; set; }

        private Type _entityType;
        public Type EntityType
        {
            get { return _entityType; }
            set
            {
                Argument.IsNotNull(value, "value");
                Argument.IsValid(typeof(Entity).IsAssignableFrom(value),
                                 string.Format("Entity type does not derive from '{0}'. Entity type must derive from '{0}'. The entity type is: '{1}'.", typeof(Entity), value),
                                 "entityType");

                _entityType = value;
            }
        }
    }
}
