using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Environment
{
    // Immutable.
    public class EntityAssembly // TODO-IG: Implement equality. Maybe implement as struct?
    {
        private static readonly Assembly _thisAssembly = Assembly.GetExecutingAssembly();
        private static readonly string _thisAssemblyFullName = Assembly.GetExecutingAssembly().GetName().FullName;

        private readonly Type[] _entityTypes;

        public Assembly Assembly { get; private set; }
        public IEnumerable<Type> EntityTypes { get { return _entityTypes; } } 

        public EntityAssembly(Assembly assembly)
        {
            Argument.IsNotNull(assembly, "assembly");
            Argument.IsValid(assembly.GetReferencedAssemblies().Any(assemblyName => assemblyName.FullName == _thisAssemblyFullName), // TODO-IG: Add override for Argument.IsValid() where the second argument is Func<string>.
                             string.Format("The assembly '{1}' is not an entity assembly because it does not reference '{2}'.{0}" +
                                           "Entity assembly is an assembly that references '{2}' and has at least one class that derives from '{3}'.",
                                           System.Environment.NewLine,
                                           assembly,
                                           _thisAssembly,
                                           typeof(Entity)
                                           ),
                             "assembly");

            Assembly = assembly;

            _entityTypes = assembly.GetTypes().Where(IsEntityType).ToArray();

            Argument.IsValid(_entityTypes.Length > 0,
                             string.Format("The assembly '{1}' is not an entity assembly because it does have any class that derives from '{3}'.{0}" +
                                           "Entity assembly is an assembly that references '{2}' and has at least one class that derives from '{3}'.",
                                           System.Environment.NewLine,
                                           assembly,
                                           _thisAssembly,
                                           typeof(Entity)
                                           ),
                             "assembly");
        }

        public static bool IsEntityAssembly(Assembly assembly)
        {
            Argument.IsNotNull(assembly, "assembly");

            return assembly.GetReferencedAssemblies().Any(assemblyName => assemblyName.FullName == _thisAssemblyFullName) &&
                   assembly.GetTypes().Any(IsEntityType);
        }

        private static bool IsEntityType(Type type)
        {
            System.Diagnostics.Debug.Assert(type != null);

            return type != typeof (Entity) && typeof (Entity).IsAssignableFrom(type);
        }

        public override string ToString()
        {
            return Assembly.ToString();
        }
    }
}
