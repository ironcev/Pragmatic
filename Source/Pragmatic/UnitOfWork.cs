using System.Collections.Generic;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic
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
            Add,
            Update,
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

            internal static Registration Add(Entity entity)
            {
                return new Registration(RegistrationType.Add, entity);
            }

            internal static Registration Update(Entity entity)
            {
                return new Registration(RegistrationType.Update, entity);
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

        public void RegisterEntityToAddOrUpdate(Entity entity)
        {
            Argument.IsNotNull(entity, "entity");
            CheckThatUnitOfWorkHasBegun();

            _registrations.Add(entity.IsNewEntity ? Registration.Add(entity) : Registration.Update(entity));
        }

        public void RegisterEntityToDelete(Entity entity)
        {
            Argument.IsNotNull(entity, "entity");
            CheckThatUnitOfWorkHasBegun();

            _registrations.Add(Registration.Delete(entity));
        }

        public void Commit()
        {
            CheckThatUnitOfWorkHasBegun();

            if (--_counter != 0) return;

            
            // Mark the registered changes in the underlying persistance.
            foreach (var registration in _registrations)
            {
                switch (registration.RegistrationType)
                {
                    case RegistrationType.Add:
                        MarkEntityAsAdded(registration.Entity);
                        // Below this point, entites will be persisted.
                        // The persistancy has to store the information that the entity is not a new entity any more.
                        // Of course, we have concurency issues here, but we completely ignore thread safety so far.
                        registration.Entity.IsNewEntity = false;
                        break;
                    case RegistrationType.Update:
                        MarkEntityAsUpdated(registration.Entity);
                        break;
                    case RegistrationType.Delete:
                        MarkEntityAsDeleted(registration.Entity);
                        break;
                }
            }

            SaveMarkedChanges();

            _registrations.Clear();
        }

        protected abstract void MarkEntityAsAdded(Entity entity);
        protected abstract void MarkEntityAsUpdated(Entity entity);
        protected abstract void MarkEntityAsDeleted(Entity entity);
        protected abstract void SaveMarkedChanges();

        private void CheckThatUnitOfWorkHasBegun()
        {
            Operation.IsValid(_counter > 0, string.Format("Unit of work has not begun. Unit of work must begin before any of its methods are called.")); // TODO-IG: Add to the message that the Begin() method has to be called after the Identifier supports method names with brackets.
        }
    }
}
