using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Core.Extensions;

/// <summary>
/// Defines the metaclass 
/// </summary>
public class MetaClassGroup<T>(IElement? metaClass)
{
    /// <summary>
    /// Gets the metaclass
    /// </summary>
    public IElement? MetaClass { get; } = metaClass;

    /// <summary>
    /// Gets the elements
    /// </summary>
    public HashSet<T> Elements { get; } = new();
}

/// <summary>
/// Supports the grouping by metaclass
/// </summary>
public class ByMetaClassGrouper
{
    public static List<MetaClassGroup<IObject>> Group(IReflectiveCollection collection)
    {
        var result = new List<MetaClassGroup<IObject>>();

        foreach (var item in collection.OfType<IObject>())
        {
            var metaClass = (item as IElement)?.getMetaClass();
            var foundGroup = result.FirstOrDefault(x => x.MetaClass == metaClass);

            if (foundGroup == null)
            {
                foundGroup = new MetaClassGroup<IObject>(metaClass);
                result.Add(foundGroup);
            }

            foundGroup.Elements.Add(item);
        }

        return result;
    }
        
    /// <summary>
    /// Groups by the metaclasses
    /// </summary>
    /// <param name="collection">Collection </param>
    /// <param name="conversionToObject">The method which converts the elements in the list</param>
    /// <typeparam name="T">Type of the elements in the list which allows the conversion
    /// </typeparam>
    /// <returns>The grouped list</returns>
    public static List<MetaClassGroup<T>> Group<T>(IEnumerable<T> collection, Func<T, IObject?> conversionToObject)
    {
        var result = new List<MetaClassGroup<T>>();

        foreach (var item in collection)
        {
            var convertedElement = conversionToObject(item);
            if (convertedElement == null) continue;
                
            var metaClass = (convertedElement as IElement)?.getMetaClass();
            var foundGroup = result.FirstOrDefault(x => x.MetaClass == metaClass);

            if (foundGroup == null)
            {
                foundGroup = new MetaClassGroup<T>(metaClass);
                result.Add(foundGroup);
            }

            foundGroup.Elements.Add(item);
        }

        return result;
    }
}