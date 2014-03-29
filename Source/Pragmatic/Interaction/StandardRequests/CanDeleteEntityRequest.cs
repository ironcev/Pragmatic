using System;
using SwissKnife;

namespace Pragmatic.Interaction.StandardRequests
{
    public sealed class CanDeleteEntityRequest<TEntity> : IRequest<Response<Option<TEntity>>> where TEntity : Entity // TODO-IG: Think about putting specific response type.
    {
        public Guid EntityId { get; set; }
    }
}
