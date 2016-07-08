using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Proxies.ReadOnly
{
    public class ReadOnlyElement : ProxyMofElement
    {
        public ReadOnlyElement(IElement element) : base(element)
        {
        }

        public override void set(string property, object value)
        {
            throw new ReadOnlyAccessException("Element is read-only");
        }

        public override void unset(string property)
        {
            throw new ReadOnlyAccessException("Element is read-only");
        }

        public override void setMetaClass(IElement metaClass)
        {
            throw new ReadOnlyAccessException("Element is read-only");
        }
    }
}