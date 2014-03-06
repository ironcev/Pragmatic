using System.Collections;
using System.Threading;
using SwissKnife.Diagnostics.Contracts;

namespace TinyDdd.Interaction
{
    /// <remarks>
    /// There can be only one <see cref="InteractionScope"/> per thread.
    /// </remarks>
    public static class InteractionScope
    {
        private static readonly ThreadLocal<int> _counter = new ThreadLocal<int>(() => 0);
        private static readonly ThreadLocal<IDictionary> _items = new ThreadLocal<IDictionary>(() => new Hashtable());

        public static IDictionary Items
        {
            get
            {
                CheckThatInteractionScopeHasBegun();

                return _items.Value;
            }
        }

        public static void BeginOrJoin()
        {
            _counter.Value++;
        }

        public static void End()
        {
            CheckThatInteractionScopeHasBegun();

            _counter.Value--;

            if (_counter.Value == 0)
                lock(_items.Value.SyncRoot)
                    _items.Value.Clear(); // Items live as long as interaction scope is open.
        }

        private static void CheckThatInteractionScopeHasBegun()
        {
            Operation.IsValid(_counter.Value > 0, string.Format("Interaction scope has not begun. Interaction scope must begin before any of its methods are called.")); // TODO-IG: Use Identifier.ToString() once it supports method names.
        }
    }
}
