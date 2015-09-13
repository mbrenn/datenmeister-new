using DatenMeister.EMOF.Interface.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace DatenMeister.EMOF.Proxy
{
    public class ProxyReflectiveSequence : IReflectiveSequence
    {
        /// <summary>
        /// Stores the sequence
        /// </summary>
        protected IReflectiveSequence _sequence;

        public ProxyReflectiveSequence(IReflectiveSequence sequence)
        {
            _sequence = sequence;
        }

        public virtual bool add(object value)
        {
            return _sequence.add(value);
        }

        public virtual void add(int index, object value)
        {
            _sequence.add(index, value);
        }

        public virtual bool addAll(IReflectiveSequence value)
        {
            return _sequence.addAll(value);
        }

        public virtual void clear()
        {
            _sequence.clear();
        }

        public virtual object get(int index)
        {
            return _sequence.get(index);
        }

        public virtual IEnumerator<object> GetEnumerator()
        {
            return _sequence.GetEnumerator();
        }

        public virtual bool remove(object value)
        {
            return _sequence.remove(value);
        }

        public virtual void remove(int index)
        {
            _sequence.remove(index);
        }

        public virtual object set(int index, object value)
        {
            return _sequence.set(index, value);
        }

        public virtual int size()
        {
            return _sequence.size();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
