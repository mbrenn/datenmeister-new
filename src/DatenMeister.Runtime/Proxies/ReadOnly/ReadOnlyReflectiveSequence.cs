using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Proxy;

namespace DatenMeister.Runtime.Proxies.ReadOnly
{
    public class ReadOnlyReflectiveSequence : ProxyReflectiveSequence
    {
        public ReadOnlyReflectiveSequence(IReflectiveSequence sequence) : base(sequence)
        {
            ActivateObjectConversion(
                x => new ReadOnlyElement(x),
                x => new ReadOnlyObject(x),
                x => x.GetProxiedElement(),
                x => x.GetProxiedElement());
        }

        public override void add(int index, object value)
        {
            throw new ReadOnlyAccessException("Sequence is read-only.");
        }

        public override bool add(object value)
        {
            throw new ReadOnlyAccessException("Sequence is read-only.");
        }

        public override bool addAll(IReflectiveSequence value)
        {
            throw new ReadOnlyAccessException("Sequence is read-only.");
        }

        public override void clear()
        {
            throw new ReadOnlyAccessException("Sequence is read-only.");
        }

        public override void remove(int index)
        {
            throw new ReadOnlyAccessException("Sequence is read-only.");
        }

        public override bool remove(object value)
        {
            throw new ReadOnlyAccessException("Sequence is read-only.");
        }

        public override object set(int index, object value)
        {
            throw new ReadOnlyAccessException("Sequence is read-only.");
        }
    }
}