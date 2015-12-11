using System.Collections.Generic;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

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
            var result = new List<object>();
            foreach (var item in extent.elements())
            {
                if (item is IObjectExt)
                {
                    var itemAsObjectExt = item as IObjectExt;
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
    }
}