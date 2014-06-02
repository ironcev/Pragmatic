using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using SwissKnife.Collections;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Environment
{
    public class EntityAssemblyCollection : Collection<EntityAssembly> // TODO-IG: Quick and dirty implementation. We don't need the whole Collection interface. Also, the below implementation is not completely thread safe. Do it properly.
    {
        public EntityAssemblyCollection()
        {
            Items.AddMany(AppDomain.CurrentDomain.GetAssemblies().Where(EntityAssembly.IsEntityAssembly).Select(assembly => new EntityAssembly(assembly))); 
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected override void ClearItems()
        {
            base.ClearItems();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
        }

        public IEnumerable<Type> EntityTypes
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get { return Items.SelectMany(entityAssembly => entityAssembly.EntityTypes); }
        } 
    }
}
