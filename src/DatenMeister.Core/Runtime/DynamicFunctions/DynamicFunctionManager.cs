using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.Runtime.DynamicFunctions;

/// <summary>
/// Defines the dynamic function manager which is allocated to a workspace
/// </summary>
public class DynamicFunctionManager
{
    /// <summary>
    /// Stores the derived properties
    /// </summary>
    private readonly Dictionary<DerivedPropertyKey, DerivedProperty> _derivedProperties = new();

    /// <summary>
    /// Gets the dynamic property of the element via the function manager.
    /// The dynamic function manager is called by the workspace in which
    /// the metaclass is hosted
    /// </summary>
    /// <param name="element">Element to be evaluated</param>
    /// <param name="metaclass">Metaclass whose property is queried</param>
    /// <param name="property">Property name to be evaluated</param>
    /// <returns>Tuple in which the first element contains the information
    /// whether the property value was found and second element in which the
    /// property value is retrieved</returns>
    public (bool, object?) GetDerivedPropertyValue(IObject element, IElement metaclass, string property)
    {
        if (element == null || metaclass == null)
        {
            return (false, null);
        }

        if (_derivedProperties.TryGetValue(new DerivedPropertyKey(metaclass, property), out var propertyValue))
        {
            return (true, propertyValue.PropertyFunction(element));
        }

        return (false, null);
    }

    public bool HasDerivedProperty(IElement type, string property)
    {
        return _derivedProperties.ContainsKey(new DerivedPropertyKey(type, property));
    }

    /// <summary>
    /// Adds a function to retrieve a property value from a metaclass by using the 
    /// </summary>
    /// <param name="type">Type to which the property function will be added</param>
    /// <param name="property">Property name to be used</param>
    /// <param name="propertyFunction">The function which concerts the element's property to a value</param>
    public void AddDerivedProperty(IElement type, string property, Func<IObject, object> propertyFunction)
    {
        _derivedProperties[new DerivedPropertyKey(type, property)] =
            new DerivedProperty(propertyFunction);
    }
}

/// <summary>
/// Defines the key for derived property
/// </summary>
class DerivedPropertyKey
{
    public DerivedPropertyKey(IElement metaClass, string property)
    {
        MetaClass = metaClass;
        Property = property;
    }

    public IElement MetaClass { get; }

    public string Property { get; }

    public override bool Equals(object? obj)
    {
        if (obj is DerivedPropertyKey key)
        {
            return Equals(key);
        }

        return false;
    }

    protected bool Equals(DerivedPropertyKey other)
    {
        return MetaClass.Equals(other.MetaClass) && Property == other.Property;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (MetaClass.GetHashCode() * 397) ^ Property.GetHashCode();
        }
    }
}

class DerivedProperty
{
    public DerivedProperty(Func<IObject, object> propertyFunction)
    {
        PropertyFunction = propertyFunction;
    }

    /// <summary>
    /// Gets or sets the property function
    /// </summary>
    public Func<IObject, object> PropertyFunction { get; set; }
}