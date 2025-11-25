using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models.EMOF;

namespace DatenMeister.Core.Uml.Helper;

/// <summary>
/// Defines the helper methods applicable to the Enumeration
/// </summary>
public class EnumerationMethods
{
    /// <summary>
    /// Gets the enumeration values as the complete objects
    /// </summary>
    /// <param name="enumeration"></param>
    /// <returns></returns>
    public static IReflectiveCollection GetEnumValueObjects(IObject enumeration)
    {
        var result =
            enumeration.getOrDefault<IReflectiveCollection>(_UML._SimpleClassifiers._Enumeration.ownedLiteral);
        return result;
    }

    /// <summary>
    /// Gets the enumeration values themselves
    /// </summary>
    /// <param name="enumeration"></param>
    /// <returns></returns>
    public static IEnumerable<string> GetEnumValues(IObject enumeration)
    {
        var values = GetEnumValueObjects(enumeration);
        if (values == null)
        {
            return [];
        }

        return values.OfType<IObject>()
            .Select(x => x.getOrDefault<string>(_UML._CommonStructure._NamedElement.name));
    }
}