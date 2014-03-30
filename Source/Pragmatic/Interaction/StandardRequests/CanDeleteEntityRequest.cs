using System;
using SwissKnife;

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
                ArgumentCheck.EntityTypeRepresentsEntityType(value, "value");

                _entityType = value;
            }
        }
    }
}
