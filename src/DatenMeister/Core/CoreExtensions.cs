using System.Collections.Generic;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;

namespace DatenMeister.Core
{
    /// <summary>
    /// Contains a set of methods that support standard operations for Extents
    /// </summary>
    public static class CoreExtensions
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
        /// Sets the properties of the value
        /// </summary>
        /// <param name="value">Object which will receive the values</param>
        /// <param name="properties">Properties to be set</param>
        public static void SetProperties(this IObject value, IDictionary<string, object> properties)
        {
            foreach (var pair in properties)
            {
                value.set(pair.Key, pair.Value);
            }
        }
    }
}