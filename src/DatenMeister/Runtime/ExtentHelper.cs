using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Helper;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime
{
    public static class ExtentHelper
    {
        /// <summary>
        /// Creates a temporary uri for an extent. 
        /// It starts with 'dm:///temp/' and finishes 
        /// </summary>
        /// <returns>The created uri</returns>
        public static string CreateTemporaryExtentUri()
        {
            return $"dm:///{Guid.NewGuid()}";
        }
        /// <summary>
        ///     Returns an enumeration of all columns that are within the given extent
        /// </summary>
        /// <param name="extent">Extent to be checked</param>
        /// <returns>Enumeration of all columns</returns>
        public static IEnumerable<string> GetProperties(this IUriExtent extent)
        {
            var elements = extent.elements();

            return GetProperties(elements);
        }

        private static IEnumerable<string> GetProperties(this IReflectiveSequence elements)
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
                var foundExtent = objectKnowsExtent.Extents.FirstOrDefault() as IUriExtent;
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
                if (AllDescendentsQuery.GetDescendents(extent.elements())
                    .Any(x => x.Equals(value)))
                {
                    return extent;
                }
            }

            return null;
        }
    }
}