using System;

namespace Pragmatic.Example.Model
{
    internal class EntityIdGenerator
    {
        public static Func<Type, Guid> GenerateId { get; private set; }

        static EntityIdGenerator()
        {
            GenerateId = type =>
            {
                if (type == typeof (Company))
                    return Guid.NewGuid(); // TODO-IG: Add sequential guid generation here, once when it's added to SwissKnife.

                return Guid.NewGuid();
            };
        }
    }
}
