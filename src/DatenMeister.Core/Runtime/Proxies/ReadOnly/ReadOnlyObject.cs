using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.Runtime.Proxies.ReadOnly;

public class ReadOnlyObject(IObject value) : ProxyMofObject(value)
{
    public override void set(string property, object? value)
    {
        throw new ReadOnlyAccessException("Element is readonly");
    }

    public override void unset(string property)
    {
        throw new ReadOnlyAccessException("Element is readonly");
    }
}