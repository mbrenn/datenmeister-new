using System;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Stores the extension methods for emof
    /// </summary>
    public static class EMOFExtensions
    {
        /// <summary>
        /// Returns the element, representing the .Net class
        /// </summary>
        /// <param name="extent">Extent to be used as base</param>
        /// <param name="type">Type to be resolves</param>
        /// <returns>The element being resolved</returns>
        public static IElement ToResolvedElement(this MofExtent extent, Type type)
        {
            var element = extent.TypeLookup.ToElement(type);
            if (string.IsNullOrEmpty(element))
                return null;

            return extent.GetUriResolver().Resolve(element, ResolveType.Default);
        }
    }
}