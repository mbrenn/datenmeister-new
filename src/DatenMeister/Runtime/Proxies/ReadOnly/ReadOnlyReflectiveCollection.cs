using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;

namespace DatenMeister.Runtime.Proxies.ReadOnly
{
    public class ReadOnlyReflectiveCollection : ProxyReflectiveCollection
    {
        public ReadOnlyReflectiveCollection(IReflectiveSequence sequence) : base(sequence)
        {
            ActivateObjectConversion(
                x => new ReadOnlyElement((MofElement) x),
                x => new ReadOnlyObject(x),
                x => x.GetProxiedElement(),
                x => x.GetProxiedElement());
        }

        public override bool add(object value) =>
            throw new ReadOnlyAccessException("Sequence is read-only.");

        public override bool addAll(IReflectiveSequence value) =>
            throw new ReadOnlyAccessException("Sequence is read-only.");

        public override void clear()
        {
            throw new ReadOnlyAccessException("Sequence is read-only.");
        }

        public override bool remove(object value) =>
            throw new ReadOnlyAccessException("Sequence is read-only.");
    }
}