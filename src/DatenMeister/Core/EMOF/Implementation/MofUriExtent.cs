using System.Linq;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider;
using DatenMeister.Runtime;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Implements the MOF interface for the uriextent
    /// </summary>
    public class MofUriExtent : MofExtent, IUriExtent, IUriResolver
    {
        private readonly string _uri;
        private readonly ExtentUrlNavigator<MofElement> _navigator;

        /// <inheritdoc />
        public MofUriExtent(IProvider provider, string uri) :
            base(provider)
        {
            _uri = uri;
            _navigator = new ExtentUrlNavigator<MofElement>(this);
        }

        /// <inheritdoc />
        public string contextURI()
        {
            return _uri;
        }

        /// <inheritdoc />
        public string uri(IElement element)
        {
            return _navigator.uri(element);
        }

        /// <inheritdoc />
        public IElement element(string uri)
        {
            return _navigator.element(uri);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"UriExent: {contextURI()}";
        }

        /// <summary>
        /// Gets the id of the element as defined in the uri. 
        /// The format of the uri is expected to be like protocol://path#id.
        /// The text after the first '#' is returned
        /// </summary>
        /// <param name="uri">Uri to be looked for</param>
        /// <returns>Found uri</returns>
        public static string GetIdOfUri(string uri)
        {
            var pos = uri.IndexOf('#');
            if (pos == -1)
            {
                return uri;
            }

            return uri.Substring(pos + 1);
        }
        
        /// <inheritdoc />
        public IElement Resolve(string uri)
        {
            var result = element(uri);
            if (result != null)
            {
                return result;
            }

            // Now look into the explicit extents
            foreach (var metaExtent in MetaExtents)
            {
                var element = metaExtent.element(uri);
                if (element != null)
                {
                    return element;
                }
            }

            // If still not found, look into the meta workspaces. Nevertheless, no recursion
            var metaWorkspaces = Workspace?.MetaWorkspaces;
            if (metaWorkspaces != null)
            {
                foreach (var metaWorkspace in metaWorkspaces)
                {
                    foreach (var metaExtent in metaWorkspace.extent.OfType<IUriExtent>())
                    {
                        var element = metaExtent.element(uri);
                        if (element != null)
                        {
                            return element;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Resolves an object by just having the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IElement ResolveById(string id)
        {
            var uri = contextURI() + "#" + id;
            return element(uri);
        }
    }
}