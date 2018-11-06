using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Proxies;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Implements the MOF interface for the uriextent
    /// </summary>
    public class MofUriExtent : MofExtent, IUriExtent, IUriResolver
    {
        /// <summary>
        /// Gets an enumeration of alternative uris
        /// </summary>
        public IList<string> AlternativeUris
        {
            get => new ReflectiveList<string>(new MofReflectiveSequence(GetMetaObject(), "__alternativeUrls"));
            set => set("__alternativeUrls", value);
        }

        /// <summary>
        /// Stores the navigator
        /// </summary>
        private readonly ExtentUrlNavigator<MofElement> _navigator;

        /// <summary>
        /// Gets or sets the uri of the extent
        /// </summary>
        private string UriOfExtent {
            get
            {
                var uri = isSet("__uri") ? get("__uri") : null;
                if (uri == null)
                {
                    return string.Empty;
                }

                return uri.ToString();
            }

            set => set("__uri", value);
        }

        /// <inheritdoc />
        public MofUriExtent(IProvider provider) :
            base(provider)
        {
            _navigator = new ExtentUrlNavigator<MofElement>(this);

            if (provider is IHasUriResolver hasUriResolver)
            {
                hasUriResolver.UriResolver = this;
            }
        }


        /// <inheritdoc />
        public MofUriExtent(IProvider provider, string uri) :
            this(provider)
        {
            UriOfExtent = uri;
        }

        /// <summary>
        /// Adds an alternative uri to the extent. The elements of the extent can also be found by the other uris
        /// </summary>
        /// <param name="alternativeUri">Alternative Uri to be added</param>
        public void AddAlternativeUri(string alternativeUri)
        {
            if (!AlternativeUris.Contains(alternativeUri))
            {
                AlternativeUris.Add(alternativeUri);
            }
        }

        /// <inheritdoc />
        public string contextURI()
        {
            return UriOfExtent;
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