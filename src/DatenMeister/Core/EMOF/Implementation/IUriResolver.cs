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
        IElement Resolve(string uri);

        /// <summary>
        /// Resolves a certain element by id
        /// </summary>
        /// <param name="id">Path to be queried</param>
        /// <returns>The found element for the id</returns>
        IElement ResolveById(string id);

        /// <summary>
        /// Adds an extent as a meta extent, so it will also be used to retrieve the element
        /// </summary>
        /// <param name="extent">Extent to be added</param>
        void AddMetaExtent(IUriExtent extent);
    }
}