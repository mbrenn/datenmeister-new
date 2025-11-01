using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models.EMOF;

namespace DatenMeister.Core.TypeIndexAssembly.Helper;

/// <summary>
/// Contains some methods to access naming.
/// This is a duplicate to the one in DatenMeister.Core. We would like to avoid to have
/// a circular dependency between the two projects. 
/// </summary>
public static class NamedElementMethods
{
    /// <summary>
    /// Defines the maximum depth for which the full name is being evaluated
    /// </summary>
    private const int MaxDepth = 1000;
    
    /// <summary>
    /// Gets the full path to the given element. It traverses through the container values of the
    /// objects and retrieves the partial names by 'name'.
    /// </summary>
    /// <param name="value">Value to be queried</param>
    /// <param name="separator">Separator</param>
    /// <returns>Full name of the element</returns>
    public static string GetFullName(IObject value, string separator = "::")
    {
        switch (value)
        {
            case null:
                throw new ArgumentNullException(nameof(value));
            case MofObjectShadow shadow:
                return shadow.Uri;
            case IElement valueAsElement:
                var current = valueAsElement.container();
                var result = GetName(value);
                var depth = 0;

                while (current != null)
                {
                    var currentName = GetName(current);
                    result = $"{currentName}{separator}{result}";
                    current = current.container();
                    depth++;

                    if (depth > MaxDepth)
                    {
                        throw new InvalidOperationException(
                            $"The full name of the element {value} could not be retrieved due to an infinite loop. (Threshold is 1000)");
                    }
                }

                return result;
        }

        throw new InvalidOperationException(
            $"The type of the given element is not supported: {value.GetType().FullName}");
    }

    /// <summary>
    /// Gets the name of an element as a very simplified implementation
    /// </summary>
    /// <param name="value">Element from which the name is getting retrieved</param>
    /// <returns>Name of the element</returns>
    public static string GetName(IObject value)
    {
        return value.getOrDefault<string>(_UML._CommonStructure._NamedElement.name);
    }
    
}