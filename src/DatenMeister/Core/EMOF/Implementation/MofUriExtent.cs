using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Stores the list of alternative uris which are used to make the elements more available. 
        /// </summary>
        private readonly List<string> _alternativeUris = new List<string>();

        /// <summary>
        /// Gets an enumeration of alternative uris
        /// </summary>
        public IEnumerable<string> AlternativeUris => _alternativeUris;

        /// <summary>
        /// Stores the navigator
        /// </summary>
        private readonly ExtentUrlNavigator<MofElement> _navigator;

        /// <inheritdoc />
        public MofUriExtent(IProvider provider, string uri) :
            base(provider)
        {
            _uri = uri;
            _navigator = new ExtentUrlNavigator<MofElement>(this);

            if (provider is IHasUriResolver hasUriResolver)
            {
                hasUriResolver.UriResolver = this;
            }
        }

        /// <summary>
        /// Adds an alternative uri to the extent. The elements of the extent can also be found by the other uris
        /// </summary>
        /// <param name="alternativeUri">Alternative Uri to be added</param>
        public void AddAlternativeUri(string alternativeUri)
        {
            if (!_alternativeUris.Contains(alternativeUri))
            {
                _alternativeUris.Add(alternativeUri);
            }
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
        public IElement Resolve(string uri, ResolveType resolveType)
        {
            if (resolveType != ResolveType.OnlyMetaClasses)
            {
                var result = element(uri);
                if (result != null)
                {
                    return result;
                }

                if ((resolveType & ResolveType.NoWorkspace) == 0)
                {
                    var workspaceResult = Workspace?.Resolve(uri, resolveType);
                    if (workspaceResult != null)
                    {
                        return workspaceResult;
                    }
                }
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

            var resolve = ResolveByMetaWorkspaces(uri, Workspace);
            return resolve;
        }

        /// <summary>
        /// Resolves the the given uri by looking through each meta workspace of the workspace
        /// </summary>
        /// <param name="uri">Uri being retrieved</param>
        /// <param name="workspace">Workspace whose meta workspaces were queried</param>
        /// <param name="alreadyVisited">Set of all workspaces already being visited. This avoid unnecessary recursion and unlimited recursion</param>
        /// <returns>Found element or null, if not found</returns>
        private IElement ResolveByMetaWorkspaces(
            string uri, 
            Runtime.Workspaces.Workspace workspace,
            HashSet<Runtime.Workspaces.Workspace> alreadyVisited = null)
        {
            alreadyVisited = alreadyVisited ?? new HashSet<Runtime.Workspaces.Workspace>();
            if (alreadyVisited.Contains(workspace))
            {
                return null;
            }
            alreadyVisited.Add(workspace);

            // If still not found, look into the meta workspaces. Nevertheless, no recursion
            var metaWorkspaces = workspace?.MetaWorkspaces;
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

                    var elementByMeta = ResolveByMetaWorkspaces(uri, metaWorkspace, alreadyVisited);
                    if (elementByMeta != null)
                    {
                        return elementByMeta;
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