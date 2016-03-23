using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.EMOF.Queries;

namespace DatenMeister.EMOF.Helper
{
    public static class ExtentHelper
    {
        /// <summary>
        ///     Returns an enumeration of all columns that are within the given extent
        /// </summary>
        /// <param name="extent">Extent to be checked</param>
        /// <returns>Enumeration of all columns</returns>
        public static IEnumerable<object> GetProperties(this IUriExtent extent)
        {
            var elements = extent.elements();

            return GetProperties(elements);
        }

        private static IEnumerable<object> GetProperties(this IReflectiveSequence elements)
        {
            var result = new List<object>();
            foreach (var item in elements)
            {
                if (item is IObjectAllProperties)
                {
                    var itemAsObjectExt = item as IObjectAllProperties;
                    var properties = itemAsObjectExt.getPropertiesBeingSet();

                    foreach (var property in properties)
                    {
                        if (!result.Contains(property))
                        {
                            result.Add(property);
                            yield return property;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Tries to retrieve the extent as given by the implemented interface
        /// IObjectKnowsExtent. If the interface is not implemented by the root element
        /// of the given element, the method will return a failure
        /// </summary>
        /// <param name="value">Value, which is queried</param>
        /// <returns></returns>
        public static IExtent GetExtentOf(this IObject value)
        {
            // If the object is contained by another object, query the contained objects 
            // because the extents will only be stored in the root elements
            var asElement = value as IElement;
            var parent = asElement?.container();
            if (parent != null)
            {
                return GetExtentOf(parent);
            }

            // If the object knows the extent to which it belongs to, it will return it
            var objectKnowsExtent = value as IObjectKnowsExtent;
            if (objectKnowsExtent != null)
            {
                return objectKnowsExtent.Extents.FirstOrDefault() as IUriExtent;
            }
            else
            {
                throw new ArgumentException($"The following element does not implement the IObjectKnowsExtent interface: {value}");
            }
        }

        public static IUriExtent GetUriExtentOf(this IObject value)
        {
            var result =  GetExtentOf(value);
            if (result == null)
            {
                return null;
            }

            // Checks, if the given result is a uriextent
            var resultAsUriExtent = result as IUriExtent;
            if (resultAsUriExtent == null)
            {
                throw new InvalidOperationException($"The returned extent is not an IUriExtent {result}");
            }

            return resultAsUriExtent;
        }

        /// <summary>
        /// Finds out of an enumeration of extents, the extent that has the given element.
        /// </summary>
        /// <param name="extents">Extents to be parsed</param>
        /// <param name="value">Element to be found</param>
        /// <returns>the extent with the element or none</returns>
        public static IUriExtent WithElement(this IEnumerable<IUriExtent> extents, IObject value)
        {
            // If the object is contained by another object, query the contained objects 
            // because the extents will only be stored in the root elements
            var asElement = value as IElement;
            var parent = asElement?.container();
            if (parent != null)
            {
                return WithElement(extents, parent);
            }

            // If the object knows the extent to which it belongs to, it will return it
            var objectKnowsExtent = value as IObjectKnowsExtent;
            if (objectKnowsExtent != null)
            {
                var foundExtent =  objectKnowsExtent.Extents.FirstOrDefault() as IUriExtent;
                return extents.FirstOrDefault(x => x == foundExtent);
            }

            // First, try to find it via the caching
            var uriExtents = extents as IList<IUriExtent> ?? extents.ToList();

            foreach (var extent in uriExtents)
            {
                if (extent is IExtentCachesObject)
                {
                    var extentAsObjectCache = extent as IExtentCachesObject;
                    if (extentAsObjectCache.HasObject(value))
                    {
                        return extent;
                    }
                }
            }

            // If not successful, try to find it by traditional, but old approach
            foreach (var extent in uriExtents)
            {
                if (extent.elements().GetAllDecendants().Any(x => x.Equals(value)))
                {
                    return extent;
                }
            }

            return null;
        }
    }
}