using System.Collections;
using System.Collections.Generic;
using DatenMeister.EMOF.Interface.Common;

namespace DatenMeister.EMOF.Proxy
{
    public class ProxyReflectiveSequence : IReflectiveSequence
    {
        /// <summary>
        ///     Stores the sequence
        /// </summary>
        protected IReflectiveSequence Sequence;

        public ProxyReflectiveSequence(IReflectiveSequence sequence)
        {
            Sequence = sequence;
        }

        public virtual bool add(object value)
        {
            return Sequence.add(value);
        }

        public virtual void add(int index, object value)
        {
            Sequence.add(index, value);
        }

        public virtual bool addAll(IReflectiveSequence value)
        {
            return Sequence.addAll(value);
        }

        public virtual void clear()
        {
            Sequence.clear();
        }

        public virtual object get(int index)
        {
            return Sequence.get(index);
        }

        public virtual IEnumerator<object> GetEnumerator()
        {
            return Sequence.GetEnumerator();
        }

        public virtual bool remove(object value)
        {
            return Sequence.remove(value);
        }

        public virtual void remove(int index)
        {
            Sequence.remove(index);
        }

        public virtual object set(int index, object value)
        {
            return Sequence.set(index, value);
        }

        public virtual int size()
        {
            return Sequence.size();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
