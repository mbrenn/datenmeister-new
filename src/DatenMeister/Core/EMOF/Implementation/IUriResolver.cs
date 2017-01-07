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
    }
}