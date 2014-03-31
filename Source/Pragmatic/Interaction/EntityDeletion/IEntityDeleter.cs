using System;
using SwissKnife;

namespace Pragmatic.Interaction.EntityDeletion
{
    public interface IEntityDeleter
    {
        Response DeleteEntity(Guid entityId);
        Response DeleteEntity(Entity entity);
        Response<Option<Entity>> CanDeleteEntity(Guid entityId);
    }
}