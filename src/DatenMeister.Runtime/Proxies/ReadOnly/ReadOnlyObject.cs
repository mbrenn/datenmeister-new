using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Proxies.ReadOnly
{
    public class ReadOnlyObject : ProxyMofObject
    {
        public ReadOnlyObject(IObject value) : base(value)
        {
        }

        public override void set(object property, object value)
        {
            throw new ReadOnlyAccessException($"Element is readonly");
        }

        public override void unset(object property)
        {
            throw new ReadOnlyAccessException($"Element is readonly");
        }
    }
}