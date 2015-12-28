using DatenMeister.EMOF.Interface.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace DatenMeister.EMOF.Proxy
{
    public class ProxyReflectiveCollection : IReflectiveCollection
    {
        protected IReflectiveCollection Collection;

        public ProxyReflectiveCollection(IReflectiveCollection collection)
        {
            Collection = collection;
        }

        public virtual bool add(object value)
        {
            return Collection.add(value);
        }

        public virtual bool addAll(IReflectiveSequence value)
        {
            return Collection.addAll(value);
        }

        public virtual void clear()
        {
            Collection.clear();
        }

        public virtual IEnumerator<object> GetEnumerator()
        {
            return Collection.GetEnumerator();
        }

        public virtual bool remove(object value)
        {
            return Collection.remove(value);
        }

        public virtual int size()
        {
            return Collection.size();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
