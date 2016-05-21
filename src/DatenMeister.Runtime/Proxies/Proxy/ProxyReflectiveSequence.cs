using System.Collections;
using DatenMeister.EMOF.Interface.Common;

namespace DatenMeister.EMOF.Proxy
{
    public class ProxyReflectiveSequence : ProxyReflectiveCollection, IReflectiveSequence
    {
        /// <summary>
        ///     Stores the sequence
        /// </summary>
        protected IReflectiveSequence Sequence
        {
            get
            {
                return Collection as IReflectiveSequence;
            }
        }

        public ProxyReflectiveSequence(IReflectiveSequence sequence) : base ( sequence)
        {
            
        }

        public virtual void add(int index, object value)
        {
            Sequence.add(index, PrivatizeElementFunc(value));
        }

        public virtual object get(int index)
        {
            return PublicizeElementFunc( Sequence.get(index));
        }

        public virtual void remove(int index)
        {
            Sequence.remove(index);
        }

        public virtual object set(int index, object value)
        {
            return PublicizeElementFunc (Sequence.set(index, PrivatizeElementFunc(value)));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public new ProxyReflectiveSequence ActivateObjectConversion()
        {
            base.ActivateObjectConversion();
            return this;
        }
    }
}
