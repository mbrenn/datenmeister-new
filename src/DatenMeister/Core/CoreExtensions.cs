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
        /// <summary>
        /// Sets the extent type
        /// </summary>
        /// <param name="extent">Extent, which shall get a type</param>
        /// <param name="extentType">Type of the extent to be set</param>
        public static void SetExtentType(this IExtent extent, string extentType)
        {
            extent.set("__ExtentType", extentType);
        }

        /// <summary>
        /// Gets the extent type
        /// </summary>
        /// <param name="extent">Type of the extent to be set</param>
        public static string GetExtentType(this IExtent extent)
        {
            return extent?.getOrDefault("__ExtentType")?.ToString() ?? string.Empty;
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