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
        private IReflectiveCollection _collection;

        public ProxyReflectiveCollection ( IReflectiveCollection collection)
        {
            _collection = collection;
        }

        public bool add(object value)
        {
            throw new NotImplementedException();
        }

        public bool addAll(IReflectiveSequence value)
        {
            throw new NotImplementedException();
        }

        public void clear()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<object> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool remove(object value)
        {
            throw new NotImplementedException();
        }

        public int size()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
