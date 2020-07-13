using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Functions.Queries
{
    public class OrderByProperties : IReflectiveCollection, IHasExtent
    {
        private readonly List<string> _orderByProperty;
        private readonly IReflectiveCollection _parent;

        public OrderByProperties(IReflectiveCollection parent, IEnumerable<string> properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }
            
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }
            
            _parent = parent;
            _orderByProperty = properties.ToList();
        }

        public bool add(object value)
            => _parent.add(value);

        public bool addAll(IReflectiveSequence value) =>
            _parent.addAll(value);

        public void clear()
        {
            _parent.clear();
        }

        public IEnumerator<object> GetEnumerator()
        {
            // If there is no ordering
            if (_orderByProperty.Count == 0)
            {
                foreach (var item in _parent)
                {
                    yield return _parent;
                }
            }

            // Build up the Query
            var firstColumn = _orderByProperty[0];
            var current =
                firstColumn.StartsWith("!")
                    ? _parent
                        .OrderByDescending(x => (x as IObject)?.getOrDefault<string>(firstColumn))
                    : _parent
                        .OrderBy(x => (x as IObject)?.getOrDefault<string>(firstColumn));

            for (var n = 1; n < _orderByProperty.Count; n++)
            {
                var currentColumn = _orderByProperty[n];
                if (currentColumn.StartsWith("!"))
                {
                    currentColumn = currentColumn.Substring(1);
                    current = current
                        .ThenByDescending(x => (x as IObject)?.getOrDefault<string>(currentColumn));
                }
                else
                {
                    current = current
                        .ThenBy(x => (x as IObject)?.getOrDefault<string>(currentColumn));
                }
            }
            
            foreach (var item in current)
            {
                if (item == null) continue;
                
                yield return item;
            }
        }

        public bool remove(object? value) => _parent.remove(value);

        public int size() =>
            _parent.size();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        /// <summary>
        /// Gets the extent associated to the parent extent
        /// </summary>
        public IExtent? Extent =>
            (_parent as IHasExtent)?.Extent;
    }
}