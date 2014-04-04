using System.Collections;
using Pragmatic.Interaction;
using StructureMap;
using StructureMap.Pipeline;

namespace Pragmatic.StructureMap
{
    public class InteractionScopeLifecycle : ILifecycle
    {
        public static readonly string StructureMapInstancesDictionaryKey = "STRUCTUREMAP-INSTANCES-562DAE3E-0374-4FAB-A7EC-7E1AC25FFA92";

        public void EjectAll(ILifecycleContext context)
        {
            FindCache(context).DisposeAndClear();
        }

        public IObjectCache FindCache(ILifecycleContext context)
        {
            IDictionary items = InteractionScope.Items;

            if (!items.Contains(StructureMapInstancesDictionaryKey))
            {
                lock (items.SyncRoot)
                {
                    if (!items.Contains(StructureMapInstancesDictionaryKey))
                    {
                        var cache = new LifecycleObjectCache();
                        items.Add(StructureMapInstancesDictionaryKey, cache);

                        return cache;
                    }
                }
            }

            return (IObjectCache)items[StructureMapInstancesDictionaryKey];
        }

        public string Description { get { return typeof(InteractionScopeLifecycle).Name.Replace("Lifecycle", ""); } }
    }
}
