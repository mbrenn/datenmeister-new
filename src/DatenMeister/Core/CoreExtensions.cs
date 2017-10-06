using DatenMeister.Core.EMOF.Interface.Identifiers;

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
            return extent.isSet("__ExtentType") ? extent.get("__ExtentType").ToString() : string.Empty;
        }

    }
}