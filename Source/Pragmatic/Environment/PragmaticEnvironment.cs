namespace Pragmatic.Environment
{
    public static class PragmaticEnvironment
    {
        public static EntityAssemblyCollection EntityAssemblies { get; private set; }

        static PragmaticEnvironment()
        {
            EntityAssemblies = new EntityAssemblyCollection();
        }
    }
}