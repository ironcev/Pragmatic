using System;
using SwissKnife;

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
                    return GuidUtility.NewSequentialGuid();

                return Guid.NewGuid();
            };
        }
    }
}
