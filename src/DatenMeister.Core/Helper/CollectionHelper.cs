using System.Collections;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Provider;
using DatenMeister.Core.Provider;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.Core.Helper;

public static class CollectionHelper
{
    public static IList<T> ToList<T>(this IReflectiveCollection collection) where T : notnull =>
        new ReflectiveList<T>(collection);

    public static IList<T> ToList<T>(this IReflectiveCollection collection, Func<object?, T> wrapFunc,
        Func<T, object?> unwrapFunc) where T : notnull 
        =>
            new ReflectiveList<T>(collection, wrapFunc, unwrapFunc);

    /// <summary>
    /// Gets the first element of the reflective collection.
    /// Returns null, if no element is existing. If the given element is not
    /// a enumeration, the element itself will be returned
    /// </summary>
    /// <param name="value">Value to be queried</param>
    /// <returns>Returned element</returns>
    public static object MakeSingle(object value)
    {
        if (DotNetHelper.IsOfEnumeration(value))
        {
            var asEnumeration = (IEnumerable) value;
            return asEnumeration.Cast<object>().First();
        }

        return value;
    }

    public static IEnumerable<IObject> OnlyObjects(this IEnumerable<object?> values)
    {
        foreach (var x in values)
        {
            if (x is IObject onlyObject)
            {
                yield return onlyObject;
            }
        }
    }

    /// <summary>
    /// Gets all metaclasses of a reflectivecollection, where each metaclass is returned once
    /// </summary>
    /// <param name="values">Reflective collection to be evaluated</param>
    /// <returns>Enumeration of distincting elements</returns>
    public static IEnumerable<IElement> GetMetaClasses(this IEnumerable<object> values)
    {
        var set = new HashSet<IElement?>();
        foreach (var value in values)
        {
            if (value is IElement x)
            {
                var metaClass = x.getMetaClass();
                if (metaClass is { } result)
                {
                    if (set.Add(metaClass))
                    {
                        yield return result;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Gets the associated extent of the reflective collection
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static IExtent? GetUriExtentOf(this IReflectiveCollection collection)
    {
        var mofReflection = collection as IHasExtent ??
                            throw new ArgumentException(@"Not of type IHasExtent", nameof(collection));
        return mofReflection.Extent;
    }

    /// <summary>
    /// Enumerates the given collection and only performs a resolving,
    /// if the <c>noReferences</c> flag is not set
    /// </summary>
    /// <param name="collection">Collection to be parsed</param>
    /// <param name="noReferences">true, if the references shall not be parsed</param>
    /// <returns>Enumeration of elements</returns>
    public static IEnumerable EnumerateWithNoResolving(IEnumerable collection, bool noReferences)
    {
        // and is an enumeration
        if (collection is MofReflectiveSequence enumerationConverted && noReferences)
        {
            foreach (var innerValue in enumerationConverted.Enumerate(true))
            {
                if (innerValue is IElement asElement)
                {
                    // and inner value is an IElement
                    yield return asElement;
                }
            }
        }
        else
        {
            foreach (var innerValue in collection)
            {
                if (innerValue is IElement asElement)
                {
                    // and inner value is an IElement
                    yield return asElement;
                }
            }
        }
    }

    /// <summary>
    /// Moves the element up by one position within the collection
    /// </summary>
    /// <param name="collection">Collection to be changed</param>
    /// <param name="elementToBeMovedUp">Element to be moved up</param>
    /// <returns>True, if the item is in the collection (even though it had not been moved)</returns>
    public static bool MoveElementUp(this IReflectiveSequence collection, IObject elementToBeMovedUp)
    {
        // Check, if we can directly use the function of the provider
        if (collection is MofReflectiveSequence reflectiveSequence
            && reflectiveSequence.MofObject.ProviderObject
                is IProviderObjectSupportsListMovements supportsListMovements)
        {
            var element = MofExtent.ConvertForProviderUsage(elementToBeMovedUp);
            var result = supportsListMovements.MoveElementUp(reflectiveSequence.PropertyName, element);
            reflectiveSequence.UpdateContent();
            return result;
        }

        // Object does not support moving up, so we have to do it manually. 
        var n = 0;
        var position = -1;
        foreach (var element in collection)
        {
            if (element != null && element.Equals(elementToBeMovedUp))
            {
                position = n;
                break;
            }

            n++;
        }

        // Check now, if something has found, 
        if (position == -1 || position == 0)
        {
            // If position is the first one, then nothing can be changed, but operation is successful
            return position == 0;
        }
            
        collection.remove(position);
            
        if (elementToBeMovedUp is MofObject mofObject)
        {
            // Removes the extent, so the given MofObject is now unconnected to the extent before it will
            // be reconnected
            mofObject.Extent = null;
        }
            
        collection.add(position - 1, elementToBeMovedUp);
        return true;
    }

    /// <summary>
    /// Moves the element up by one position within the collection
    /// </summary>
    /// <param name="collection">Collection to be changed</param>
    /// <param name="elementToBeMovedDown">Element to be moved down</param>
    /// <returns>True, if the item is in the collection (even though it had not been moved)</returns>
    public static bool MoveElementDown(this IReflectiveSequence collection, IObject elementToBeMovedDown)
    {
        // Check, if we can directly use the function of the provider
        if (collection is MofReflectiveSequence reflectiveSequence)
        {
            if (reflectiveSequence.MofObject.ProviderObject is IProviderObjectSupportsListMovements supportsListMovements)
            {
                var element = MofExtent.ConvertForProviderUsage(elementToBeMovedDown);
                var result = supportsListMovements.MoveElementDown(reflectiveSequence.PropertyName, element);
                reflectiveSequence.UpdateContent();
                return result;
            }
        }
            
        var n = 0;
        var count = collection.size();
        var position = -1;
        foreach (var element in collection)
        {
            if (element != null && element.Equals(elementToBeMovedDown))
            {
                position = n;
                break;
            }

            n++;
        }

        // Check now, if something has found, 
        if (position == -1 || position == count - 1)
        {
            // If position is the first one, then nothing can be changed, but operation is successful
            return position == count - 1;
        }

        collection.remove(position);
        if (elementToBeMovedDown is MofObject mofObject)
        {
            // Removes the extent, so the given MofObject is now unconnected to the extent before it will
            // be reconnected
            mofObject.Extent = null;
        }
            
        collection.add(position + 1, elementToBeMovedDown);
        return true;
    }
}