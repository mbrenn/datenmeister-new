using BurnSystems.TimeCache;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Uml.Helper;

namespace DatenMeister.Core.EMOF.Implementation.DefaultValue;

/// <summary>
/// This handler sets the default properties of the metaclass explicitly.
/// This is to be avoided since the default values may change and the default values
/// should be added in case of a reading and not upon the creation of the items themselves
/// </summary>
public static class DefaultValueHandler
{
    /// <summary>
    /// Handles the new item 
    /// </summary>
    /// <param name="newValue">Element which is recently created</param>
    /// <param name="metaClass">Defines the metaclass of the new value</param>
    public static void HandleNewItem(IElement newValue, IElement? metaClass)
    {
        // Gets the type and the associated properties
        var type = metaClass;
        if (type == null)
        {
            // Nothing to do
            return;
        }

        var properties = ClassifierMethods.GetPropertiesOfClassifier(type);
        foreach (var property in properties)
        {
            var defaultValue = property.getOrDefault<object>(_UML._Classification._Property.defaultValue);
            var name = property.getOrDefault<string>(_UML._CommonStructure._NamedElement.name);

            if (defaultValue != null)
            {
                newValue.set(name, defaultValue);
            }
        }
    }

    /// <summary>
    /// Reads the default value of a property. It does not check whether the property already had been set. 
    /// </summary>
    /// <param name="value">Value whose property shall be retrieved</param>
    /// <param name="property">The property that shall be read</param>
    /// <returns>The default value or null, if not set</returns>
    [Obsolete]
    public static T? ReadDefaultValueOfProperty<T>(IElement value, string property)
    {
        var type = value.getMetaClass();
        if (type == null)
        {
            // No type ==> no default value
            return default;
        }

        var tuple = new Tuple<IElement, string>(type, property);

        if (_cachedDefaultValues.TryGetValue(
                tuple,
                out var cachedValue))
        {
            if (cachedValue is T)
            {
                return (T)cachedValue!;
            }

            return default;
        }

        var propertyElement = ClassifierMethods.GetPropertyOfClassifier(type, property);
        if (propertyElement == null)
        {
            // No property ==> No default value
            return default;
        }

        var result = propertyElement.getOrDefault<T>(_UML._Classification._Property.defaultValue);
        _cachedDefaultValues[tuple] = result;
        return result;
    }

    /// <summary>
    /// The cache for the default values.
    /// </summary>
    private static readonly TimeCachedDictionary<Tuple<IElement, string>, object?> _cachedDefaultValues = new();
}