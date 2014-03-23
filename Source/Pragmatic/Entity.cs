using System;
using System.Runtime.CompilerServices;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic
{
    /// <summary>
    /// Base class for all entities in the domain.
    /// </summary>
    [Serializable]
    public abstract class Entity
    {
        private static Func<Type, Guid> _idGenerator;

        public static Func<Type, Guid> IdGenerator
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get { return _idGenerator; }

            [MethodImpl(MethodImplOptions.Synchronized)] set
            {
                Argument.IsNotNull(value, "value");

                _idGenerator = value;
            }
        }

        static Entity()
        {
            _idGenerator = type => Guid.NewGuid();
        }

        /// <summary>
        /// The key of the <see cref="Entity"/>.
        /// If it is <see cref="Guid.Empty"/>, the <see cref="Entity"/> is not yet persisted.
        /// </summary>
        /// <remarks>
        /// Identity Key pattern.
        /// </remarks>
        public Guid Id
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// True if entity is not yet persisted.
        /// </summary>
        public bool IsNewEntity
        {
            get;
            internal set;
        }

        protected Entity()
        {
            Id = IdGenerator(GetType());
            IsNewEntity = true;
        }
    }
}
