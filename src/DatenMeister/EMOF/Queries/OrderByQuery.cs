using DatenMeister.EMOF.Interface.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.EMOF.Queries
{
    public class OrderByQuery : IReflectiveCollection
    {
        private IReflectiveCollection _parent;

        private object _orderByProperty;

        public OrderByQuery (IReflectiveCollection parent, object property)
        {
            _parent = parent;
            _orderByProperty = property;

            if ( _parent == null )
            {
                throw new ArgumentNullException(nameof(parent));
            }

            if(_orderByProperty == null )
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
                .OrderBy(x => (x as IObject).get(_orderByProperty).ToString()))
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
            return this.GetEnumerator();
        }
    }
}
