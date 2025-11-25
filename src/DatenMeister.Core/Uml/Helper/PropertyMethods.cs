using System.Globalization;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models.EMOF;

namespace DatenMeister.Core.Uml.Helper;

/// <summary>
/// Defines some helper classes for property objects
/// </summary>
public static class PropertyMethods
{
    /// <summary>
    /// Gets the type of the property
    /// </summary>
    /// <param name="value">Property being queried</param>
    /// <returns>Type of the property</returns>
    public static IElement? GetPropertyType(IObject value) =>
        value.getOrDefault<IElement>(_UML._CommonStructure._TypedElement.type);

    public static bool IsComposite(IObject property)
    {
        return property.getOrDefault<bool>(_UML._Classification._Property.isComposite)
               || property.getOrDefault<string>(_UML._Classification._Property.aggregation) == "composite";
    }

    /// <summary>
    /// Gets the value whether the property is a collection by
    /// </summary>
    /// <param name="value">Value to ne evaluated</param>
    /// <returns>true, if the given property is a collection</returns>
    public static bool IsCollection(IObject value)
    {
        var multiplicity = value.getOrDefault<string?>(_UML._CommonStructure._MultiplicityElement.upper);

        if (multiplicity == null)
        {
            // If value is not set, try to find it via the uppervalue
            var temp = value.getOrDefault<IElement>(_UML._CommonStructure._MultiplicityElement.upperValue);
            multiplicity = temp?.getOrDefault<string>(_UML._Values._LiteralInteger.value);
        }

        if (multiplicity == "*")
        {
            return true;
        }

        if (string.IsNullOrEmpty(multiplicity))
        {
            return false;
        }

        if (Int32.TryParse(
                multiplicity,
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out var multiplicityNumber))
        {
            return multiplicityNumber > 1;
        }

        return false;
    }
}