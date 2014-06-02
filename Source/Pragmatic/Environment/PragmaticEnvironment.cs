using System;
using System.Runtime.CompilerServices;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Environment
{
    public static class PragmaticEnvironment
    {
        public static EntityAssemblyCollection EntityAssemblies { get; private set; }

        private static Func<Type, Guid> _idGenerator;
        public static Func<Type, Guid> IdGenerator
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get { return _idGenerator; }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                Argument.IsNotNull(value, "value");
                _idGenerator = value;
            }
        }

        static PragmaticEnvironment()
        {
            EntityAssemblies = new EntityAssemblyCollection();

            _idGenerator = type => GuidUtility.NewSequentialGuid();
        }
    }
}