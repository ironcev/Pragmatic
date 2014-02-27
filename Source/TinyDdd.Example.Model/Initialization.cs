using StructureMap;
using StructureMap.Configuration.DSL;
using TinyDdd.Interaction;

namespace TinyDdd.Example.Model
{
    class Initialization
    {
        static Initialization()
        {
            ObjectFactory.Configure(x => x.AddRegistry(new ExampleRegistry()));
        }

        internal static void Initialize()
        {
            // Does nothing. The actual initialization is done in the static constructors.
            // This is just to trigger the constructor.
            // It's a neat trick to get thread safety :-)
        }

        private class ExampleRegistry : Registry
        {
            internal ExampleRegistry()
            {
                Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.ConnectImplementationsToTypesClosing(typeof(ICommandHandler<,>));
                    scan.ConnectImplementationsToTypesClosing(typeof(IQueryHandler<,>));
                });
            }
        }
    }
}
