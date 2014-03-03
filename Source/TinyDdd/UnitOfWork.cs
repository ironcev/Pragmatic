using System;
using System.Collections.Generic;
using System.Linq;
using SwissKnife.Diagnostics.Contracts;

namespace TinyDdd
{
    /// <summary>
    /// Exposes methods which enable registration of entities that need to be saved or deleted from repositories in a single transaction.
    /// Unit of work supports nested commits i.e. multiple calls to <see cref="Commit"/> on single unit of work instance.
    /// Only the last <see cref="Commit"/> will actually be executed.
    /// We assume that there is only one unit of work instance running in a single transaction.
    /// For example, in a web application we will usually have single unit of work instance per HTTP request.
    /// In a desktop application we could have single unit of work per tread.
    /// </summary>
    public abstract class UnitOfWork
    {
        private enum RegistrationType
        {
            AddOrUpdate,
            Delete
        };

        private class Registration
        {
            internal RegistrationType RegistrationType { get; private set; }
            internal Entity Entity { get; private set; }

            private Registration(RegistrationType registrationType, Entity entity)
            {
                System.Diagnostics.Debug.Assert(entity != null);

                RegistrationType = registrationType;
                Entity = entity;
            }

            internal static Registration AddOrUpdate(Entity entity)
            {
                return new Registration(RegistrationType.AddOrUpdate, entity);
            }

            internal static Registration Delete(Entity entity)
            {
                return new Registration(RegistrationType.Delete, entity);
            }
        }

        /// <summary>
        /// Counts how many times the <see cref="Begin"/> method is called.
        /// </summary>
        private int _counter;

        private readonly List<Registration> _registrations = new List<Registration>();

        public void Begin()
        {
            _counter++;
        }

        public void RegisterEntityToAddOrUpdate(IAggregateRoot entity)
        {
            Argument.IsNotNull(entity, "entity");
            Argument.Is<Entity>((object)entity, "entity");
            CheckThatUnitOfWorkHasBegun();

            _registrations.Add(Registration.AddOrUpdate((Entity)entity));
        }

        public void RegisterEntityToDelete(IAggregateRoot entity)
        {
            Argument.IsNotNull(entity, "entity");
            Argument.Is<Entity>((object)entity, "entity");
            CheckThatUnitOfWorkHasBegun();

            _registrations.Add(Registration.Delete((Entity)entity));
        }

        public void Commit()
        {
            CheckThatUnitOfWorkHasBegun();

            if (--_counter != 0) return;

            // Give unique ids to all entites that are registered as added.
            foreach (var registration in _registrations.Where(registration => registration.RegistrationType == RegistrationType.AddOrUpdate &&
                                                                          registration.Entity.IsNewEntity))
            {
                // An entity can be register several times for adding or updating.
                // We have to assign the id to it only once.
                if (!registration.Entity.IsNewEntity) continue;

                registration.Entity.Id = Guid.NewGuid();
            }
            
            // Mark the registered changes in the underlying persistance.
            foreach (var registration in _registrations)
            {
                if (registration.RegistrationType == RegistrationType.AddOrUpdate)
                    MarkEntityAsAddedOrUpdated(registration.Entity);
                else
                    MarkEntityAsDeleted(registration.Entity);
            }

            SaveMarkedChanges();

            _registrations.Clear();
        }

        protected abstract void MarkEntityAsAddedOrUpdated(Entity entity);
        protected abstract void MarkEntityAsDeleted(Entity entity);
        protected abstract void SaveMarkedChanges();

        private void CheckThatUnitOfWorkHasBegun()
        {
            Operation.IsValid(_counter > 0, string.Format("Unit of work has not begun. Unit of work must begin before any of its methods are called.")); // TODO-IG: Add to the message that the Begin() method has to be called after the Identifier supports method names with brackets.
        }
    }
}
