using DatenMeister.Core.Interfaces.Provider;

namespace DatenMeister.Core.Provider.Proxies;

public class ProxyIdProviderObject(IProviderObject element, string? id) : IProviderObject
{
    public IProvider Provider => element.Provider;

    public string? Id { get; set; } = id;

    public string? MetaclassUri
    {
        get => element.MetaclassUri;
        set => element.MetaclassUri = value;
    }

    public bool IsPropertySet(string property)
    {
        return element.IsPropertySet(property);
    }

    public object? GetProperty(string property, ObjectType objectType = ObjectType.None)
    {
        return element.GetProperty(property, objectType);
    }

    public IEnumerable<string> GetProperties()
    {
        return element.GetProperties();
    }

    public bool DeleteProperty(string property)
    {
        return element.DeleteProperty(property);
    }

    public void SetProperty(string property, object? value)
    {
        element.SetProperty(property, value);
    }

    public void EmptyListForProperty(string property)
    {
        element.EmptyListForProperty(property);
    }

    public bool AddToProperty(string property, object value, int index = -1)
    {
        return element.AddToProperty(property, value, index);
    }

    public bool RemoveFromProperty(string property, object value)
    {
        return element.RemoveFromProperty(property, value);
    }

    public bool HasContainer()
    {
        return element.HasContainer();
    }

    public IProviderObject? GetContainer()
    {
        return element.GetContainer();
    }

    public void SetContainer(IProviderObject? value)
    {
        element.SetContainer(value);
    }
}