using System.Collections;
using Pragmatic.Interaction;
using StructureMap.Pipeline;

namespace Pragmatic.StructureMap
{
    public class InteractionScopeLifecycle : ILifecycle
    {
        public static readonly string StructureMapInstancesDictionaryKey = "STRUCTUREMAP-INSTANCES-562DAE3E-0374-4FAB-A7EC-7E1AC25FFA92";

        public void EjectAll()
        {
            FindCache().DisposeAndClear();
        }

        public IObjectCache FindCache()
        {
            IDictionary items = InteractionScope.Items;

            if (!items.Contains(StructureMapInstancesDictionaryKey))
            {
                lock (items.SyncRoot)
                {
                    if (!items.Contains(StructureMapInstancesDictionaryKey))
                    {
                        var cache = new MainObjectCache();
                        items.Add(StructureMapInstancesDictionaryKey, cache);

                        return cache;
                    }
                }
            }

            return (IObjectCache)items[StructureMapInstancesDictionaryKey];
        }

        public string Scope { get { return typeof(InteractionScope).Name.Replace("Lifecycle", ""); } }
    }
}
