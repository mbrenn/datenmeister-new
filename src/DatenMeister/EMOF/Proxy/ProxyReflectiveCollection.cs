using System.Collections;
using System.Collections.Generic;
using DatenMeister.EMOF.Interface.Common;

namespace DatenMeister.EMOF.Proxy
{
    public class ProxyReflectiveCollection : IReflectiveCollection
    {
        protected IReflectiveCollection _collection;

        public ProxyReflectiveCollection(IReflectiveCollection collection)
        {
            _collection = collection;
        }

        public virtual bool add(object value)
        {
            return _collection.add(value);
        }

        public virtual bool addAll(IReflectiveSequence value)
        {
            return _collection.addAll(value);
        }

        public virtual void clear()
        {
            _collection.clear();
        }

        public virtual IEnumerator<object> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        public virtual bool remove(object value)
        {
            return _collection.remove(value);
        }

        public virtual int size()
        {
            return _collection.size();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}