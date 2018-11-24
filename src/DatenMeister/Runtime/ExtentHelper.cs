using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Functions.Queries;

namespace DatenMeister.Runtime
{
    public static class ExtentHelper
    {
        private const string DatenmeisterDefaultTypePackage = "__DatenMeister.DefaultTypePackage";

        private const string ExtentType = "__ExtentType";

        /// <summary>
        /// Sets the extent type
        /// </summary>
        /// <param name="extent">Extent, which shall get a type</param>
        /// <param name="extentType">Type of the extent to be set</param>
        public static void SetExtentType(this IExtent extent, string extentType)
        {
            extent.set(ExtentType, extentType);
        }

        /// <summary>
        /// Gets the extent type
        /// </summary>
        /// <param name="extent">Type of the extent to be set</param>
        public static string GetExtentType(this IExtent extent)
        {
            return extent?.GetOrDefault(ExtentType)?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Sets the default type package which is shown, when the user wants 
        /// to create a new item
        /// </summary>
        /// <param name="extent">Extent shall get the default type package</param>
        /// <param name="defaultTypePackage">The element which shall be considered as the 
        /// default type package</param>
        public static void SetDefaultTypePackage(this IExtent extent, IElement defaultTypePackage)
        {
            extent.set(DatenmeisterDefaultTypePackage, defaultTypePackage);
        }

        /// <summary>
        /// Gets the default type package
        /// </summary>
        /// <param name="extent">Extent to be used</param>
        /// <returns>The found element</returns>
        public static IElement GetDefaultTypePackage(this IExtent extent)
        {
            return extent?.GetOrDefault(DatenmeisterDefaultTypePackage) as IElement;
        }

        /// <summary>
        /// Gets the factory of the given extent
        /// </summary>
        /// <returns>The created factory</returns>
        public static IFactory GetFactory(this IExtent extent) => new MofFactory(extent);

        /// <summary>
        /// Creates a temporary uri for an extent. 
        /// It starts with 'datenmeister:///temp/' and finishes 
        /// </summary>
        /// <returns>The created uri</returns>
        public static string CreateTemporaryExtentUri()
        {
            return $"datenmeister:///{Guid.NewGuid()}";
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

        public static IEnumerable<string> GetProperties(IReflectiveCollection elements)
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
            if (value is IHasExtent objectKnowsExtent)
            {
                var foundExtent = objectKnowsExtent.Extent as IUriExtent;
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