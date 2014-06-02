using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SwissKnife.Collections;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Environment
{
    public class EntityAssemblyCollection : Collection<EntityAssembly>
    {
        public EntityAssemblyCollection()
        {
            Items.AddMany(AppDomain.CurrentDomain.GetAssemblies().Where(EntityAssembly.IsEntityAssembly).Select(assembly => new EntityAssembly(assembly))); 
        }

        protected override void InsertItem(int index, EntityAssembly item)
        {
            Argument.IsNotNull(item, "item");

            if (Items.AsEnumerable().Any(entityAssembly => entityAssembly.Assembly == item.Assembly)) return; // TODO-IG: Replace with equality once when implemented for EntityAssembly.

            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, EntityAssembly item)
        {
            // Replaces the item at the specified index with a new object.
            throw new NotSupportedException(string.Format("Entity assemblies that are already contained in entity assembly collection cannot be replaced with other entity assemblies." +
                                                          "They can only be added, inserted, or removed.{0}" +
                                                          "You tried to replaced an already contained entity assembly '{1}' with the assembly '{2}'.",
                                                          System.Environment.NewLine,
                                                          Items[index],
                                                          item));
        }

        public IEnumerable<Type> EntityTypes { get { return Items.SelectMany(entityAssembly => entityAssembly.EntityTypes); } } 
    }
}
