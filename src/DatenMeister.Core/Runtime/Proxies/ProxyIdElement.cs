using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Core.Runtime.Proxies;

public class ProxyIdElement(IObject element, string id) : IElement, IHasId
{
    public bool equals(object? other)
    {
        return element.equals(other);
    }

    public object? get(string property)
    {
        return element.get(property);
    }

    public void set(string property, object? value)
    {
        element.set(property, value);
    }

    public bool isSet(string property)
    {
        return element.isSet(property);
    }

    public void unset(string property)
    {
        element.unset(property);
    }

    public IElement? metaclass =>
        (element as IElement ?? throw new InvalidOperationException("not an element")).metaclass;

    public IElement? getMetaClass()
    {
        return (element as IElement ?? throw new InvalidOperationException("not an element")).getMetaClass();
    }

    public IElement? container()
    {
        return (element as IElement ?? throw new InvalidOperationException("not an element")).container();
    }

    public string? Id { get; } = id;
}