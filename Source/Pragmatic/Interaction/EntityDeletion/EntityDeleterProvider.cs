using System;
using System.Linq;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Interaction.EntityDeletion
{
    public class EntityDeleterProvider
    {
        private readonly IEntityDeleterResolver _entityDeleterResolver;

        public EntityDeleterProvider(IEntityDeleterResolver entityDeleterResolver)
        {
            Argument.IsNotNull(entityDeleterResolver, "entityDeleterResolver");

            _entityDeleterResolver = entityDeleterResolver;
        }

        public Option<EntityDeleter<TEntity>> GetEntityDeleterFor<TEntity>() where TEntity : Entity
        {
            return GetEntityDeleterFor(typeof(TEntity)).MapToOption(deleter => (EntityDeleter<TEntity>)deleter);
        }

        public Option<IEntityDeleter> GetEntityDeleterFor(Type entityType)
        {
            ArgumentCheck.EntityTypeRepresentsEntityType(entityType, "entityType");

            var entityDeleters = _entityDeleterResolver.ResolveEntityDeleter(typeof(EntityDeleter<>).MakeGenericType(entityType)).ToArray();

            if (entityDeleters.Length > 1)
                throw new NotSupportedException(string.Format("There are {1} entity deleter types defined for the entity of type '{2}'.{0}" +
                                              "Having more than one entity deleter per entity type is not supported.{0}" +
                                              "The defined entity deleter types are:{0}{3}",
                                              Environment.NewLine,
                                              entityDeleters.Length,
                                              entityType,
                                              entityDeleters.Aggregate(string.Empty, (output, commandHandler) => output + commandHandler.GetType() + Environment.NewLine)));

            return entityDeleters.Length > 0 ? Option<IEntityDeleter>.Some((IEntityDeleter)entityDeleters[0]) : Option<IEntityDeleter>.None; // TODO-IG: Do we want to leave it like this? This potentially throws InvalidCastException.
        }
    }
}