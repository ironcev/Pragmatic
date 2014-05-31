using System;
using SwissKnife;

namespace Pragmatic.Interaction.StandardQueries
{
    public sealed class GetByIdQuery<T> : IQuery<Option<T>> where T : class
    {
        public Guid Id { get; set; }
    }

    public sealed class GetByIdQuery : IQuery<Option<object>>
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