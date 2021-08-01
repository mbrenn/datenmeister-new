#nullable enable

using System;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Supports resolving uri to elements within the extent or even within other workspaces
    /// </summary>
    public interface IUriResolver
    {
        /// <summary>
        /// Returns an element by resolving the uri
        /// </summary>
        /// <param name="uri">The uri to be used for resolving</param>
        /// <param name="resolveType">The type of resolve strategy</param>
        /// <param name="traceFailing">True, if a trace event shall be thrown, if
        /// the resolving did not succeed</param>
        /// <returns>The found element or null, if no element was found</returns>
        object? Resolve(string uri, ResolveType resolveType, bool traceFailing = true);

        /// <summary>
        /// Resolves a certain element by id
        /// </summary>
        /// <param name="id">Path to be queried</param>
        /// <returns>The found element for the id</returns>
        IElement? ResolveById(string id);
    }

    [Flags]
    public enum ResolveType
    {
        /// <summary>
        /// Default resolving process in which all extents in current workspace but also meta workspaces are resolved.
        /// If nothing was found, a full search will be started
        /// </summary>
        Default = 0x01,

        /// <summary>
        /// Resolving in which the current workspace will not be looked. The type
        /// will only be looked within the meta workspaces and extents. Useful to look for meta classes
        /// </summary>
        OnlyMetaClasses = 0x02,

        /// <summary>
        /// Resolving in which the current extent is resolved solely
        /// </summary>
        NoWorkspace = 0x04,

        /// <summary>
        /// Performs the resolving process only in the current workspace
        /// </summary>
        NoMetaWorkspaces = 0x08,
        
        /// <summary>
        /// Searches also within the workspace within the types
        /// </summary>
        AlsoTypeWorkspace = 0x10
    }

    public static class ResolveTypeExtensions
    {
        public static bool HasFlagFast(this ResolveType value, ResolveType flag)
        {
            return (value & flag) != 0;
        }
    }
}