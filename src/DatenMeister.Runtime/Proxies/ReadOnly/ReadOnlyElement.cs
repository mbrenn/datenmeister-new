using System;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.EMOF.Proxy;

namespace DatenMeister.Runtime.Proxies.ReadOnly
{
    public class ReadOnlyElement : ProxyMofElement
    {
        public ReadOnlyElement(IElement element) : base(element)
        {
        }

        public override void set(object property, object value)
        {
            throw new InvalidOperationException("Element is read-only");
        }

        public override void unset(object property)
        {
            throw new InvalidOperationException("Element is read-only");
        }
    }
}