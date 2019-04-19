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
        /// <param name="uri"></param>
        /// <returns></returns>
        IElement Resolve(string uri, ResolveType resolveType, bool traceFailing = true);

        /// <summary>
        /// Resolves a certain element by id
        /// </summary>
        /// <param name="id">Path to be queried</param>
        /// <returns>The found element for the id</returns>
        IElement ResolveById(string id);
    }

    [Flags]
    public enum ResolveType
    {
        /// <summary>
        /// Default resolving process in which all extents in current workspace but also meta workspaces are resolved.
        /// If nothing was found, a full search will be started
        /// </summary>
        Default = 1, 

        /// <summary>
        /// Resolving in which the current workspace will not be looked. Useful to look for meta classes
        /// </summary>
        OnlyMetaClasses = 2,

        /// <summary>
        /// Resolving in which the current extent is resolved solely
        /// </summary>
        NoWorkspace = 4,

        /// <summary>
        /// Performs the resolving process only in the current workspace
        /// </summary>
        NoMetaWorkspaces = 8
    }
}