using System.Collections;
using DatenMeister.Core.EMOF.Interface.Common;

namespace DatenMeister.Runtime.Proxies
{
    public class ProxyReflectiveSequence : ProxyReflectiveCollection, IReflectiveSequence
    {
        /// <summary>
        ///     Stores the sequence
        /// </summary>
        protected IReflectiveSequence Sequence => Collection as IReflectiveSequence;

        public ProxyReflectiveSequence(IReflectiveSequence sequence) : base(sequence)
        {
        }

        public virtual void add(int index, object value)
        {
            Sequence.add(index, PrivatizeElementFunc(value));
        }

        public virtual object get(int index) =>
            PublicizeElementFunc(Sequence.get(index));

        public virtual void remove(int index)
        {
            Sequence.remove(index);
        }

        public virtual object set(int index, object value) =>
            PublicizeElementFunc(Sequence.set(index, PrivatizeElementFunc(value)));

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public new ProxyReflectiveSequence ActivateObjectConversion()
        {
            base.ActivateObjectConversion();
            return this;
        }
    }
}
