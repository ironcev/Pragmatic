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
            var entityDeleters = _entityDeleterResolver.ResolveEntityDeleter(typeof(EntityDeleter<>).MakeGenericType(typeof(TEntity))).ToArray();

            if (entityDeleters.Length > 1)
                throw new NotSupportedException(string.Format("There are {1} entity deleters defined for the entity of type '{2}'.{0}" + 
                                              "Having more than one entity deleter per entity type is not supported.{0}" +
                                              "The defined entity deleters are:{0}{3}",
                                              Environment.NewLine,
                                              entityDeleters.Length,
                                              typeof(TEntity),
                                              entityDeleters.Aggregate(string.Empty, (output, commandHandler) => output + commandHandler.GetType() + Environment.NewLine)));

            return entityDeleters.Length > 0 ? (EntityDeleter<TEntity>)entityDeleters[0] : null; // TODO-IG: Do we want to leave it like this? This potentially throws InvalidCastException.
        }
    }
}