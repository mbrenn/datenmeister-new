using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.Runtime.Proxies.ReadOnly;

public class ReadOnlyObject : ProxyMofObject
{
    public ReadOnlyObject(IObject value) : base(value)
    {
    }

    public override void set(string property, object? value)
    {
        throw new ReadOnlyAccessException("Element is readonly");
    }

    public override void unset(string property)
    {
        throw new ReadOnlyAccessException("Element is readonly");
    }
}