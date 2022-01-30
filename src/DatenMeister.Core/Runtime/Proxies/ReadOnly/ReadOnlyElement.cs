using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.Runtime.Proxies.ReadOnly
{
    public class ReadOnlyElement : ProxyMofElement
    {
        public ReadOnlyElement(MofElement element) : base(element)
        {
        }

        public override void set(string property, object? value)
        {
            throw new ReadOnlyAccessException("Element is read-only");
        }

        public override void unset(string property)
        {
            throw new ReadOnlyAccessException("Element is read-only");
        }

        public override void SetMetaClass(IElement? metaClass)
        {
            throw new ReadOnlyAccessException("Element is read-only");
        }
    }
}