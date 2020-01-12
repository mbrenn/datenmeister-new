using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider;
using DatenMeister.Runtime.Proxies;

namespace DatenMeister.Runtime
{
    public static class CollectionHelper
    {
        public static IList<T> ToList<T>(this IReflectiveCollection collection) =>
            new ReflectiveList<T>(collection);

        public static IList<T> ToList<T>(this IReflectiveCollection collection, Func<object, T> wrapFunc,
            Func<T, object> unwrapFunc)
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
                return asEnumeration.Cast<object>().FirstOrDefault();
            }

            return value;
        }

        public static IEnumerable<IObject> OnlyObjects(this IEnumerable<object> values)
        {
            return values.Select(x => x as IObject).Where(x => x != null);
        }

        /// <summary>
        /// Gets all metaclasses of a reflectivecollection, where each metaclass is returned once
        /// </summary>
        /// <param name="values">Reflective collection to be evaluated</param>
        /// <returns>Enumeration of distincting elements</returns>
        public static IEnumerable<IElement> GetMetaClasses(this IEnumerable<object> values)
        {
            return values.OfType<IElement>().Select(x => x.getMetaClass()).Where(x => x != null).Distinct();
        }

        /// <summary>
        /// Gets the associated extent of the reflective collection
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IExtent GetAssociatedExtent(this IReflectiveCollection collection)
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
            if (collection is MofReflectiveSequence reflectiveSequence)
            {
                if (reflectiveSequence.MofObject.ProviderObject is IProviderObjectSupportsListMovements supportsListMovements)
                {
                    var element = MofExtent.ConvertForProviderUsage(elementToBeMovedUp);
                    return supportsListMovements.MoveElementUp(reflectiveSequence.PropertyName, element);
                }
            }
            
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
                // If position is the first one, then nothing can be changed
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
                    return supportsListMovements.MoveElementDown(reflectiveSequence.PropertyName, element);
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
                // If position is the first one, then nothing can be changed
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
}