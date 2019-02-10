using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Functions.Queries
{
    public class OrderByProperty : IReflectiveCollection, IHasExtent
    {
        private readonly string _orderByProperty;
        private readonly IReflectiveCollection _parent;

        public OrderByProperty(IReflectiveCollection parent, string property)
        {
            _parent = parent;
            _orderByProperty = property;

            if (_parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            if (_orderByProperty == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
        }

        public bool add(object value)
        {
            return _parent.add(value);
        }

        public bool addAll(IReflectiveSequence value)
        {
            return _parent.addAll(value);
        }

        public void clear()
        {
            _parent.clear();
        }

        public IEnumerator<object> GetEnumerator()
        {
            foreach (var item in _parent
                .OrderBy(x => (x as IObject)?.get(_orderByProperty).ToString()))
            {
                yield return item;
            }
        }

        public bool remove(object value)
        {
            return _parent.remove(value);
        }

        public int size()
        {
            return _parent.size();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets the extent associated to the parent extent
        /// </summary>
        public IExtent Extent => (_parent as IHasExtent)?.Extent;
    }
}