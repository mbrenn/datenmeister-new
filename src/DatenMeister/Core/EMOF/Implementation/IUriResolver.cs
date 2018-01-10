using System;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Identifiers;
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
        IElement Resolve(string uri, ResolveType resolveType);

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
        Default = 1, 
        OnlyMetaClasses = 2,
        NoWorkspace = 4
    }
}