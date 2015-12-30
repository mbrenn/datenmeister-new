using System;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.EMOF.Proxy;

namespace DatenMeister.Runtime.Proxies.ReadOnly
{
    public class ReadOnlyObject : ProxyMofObject
    {
        public ReadOnlyObject(IObject value) : base(value)
        {
        }

        public override void set(object property, object value)
        {
            throw new InvalidOperationException($"Element is readonly");
        }

        public override void unset(object property)
        {
            throw new InvalidOperationException($"Element is readonly");
        }
    }
}