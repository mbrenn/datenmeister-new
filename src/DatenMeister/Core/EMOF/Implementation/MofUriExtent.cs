using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.ChangeEvents;
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
        /// Defines a possible logger
        /// </summary>
        private static readonly ClassLogger Logger = new ClassLogger(typeof(MofUriExtent));

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
        private string UriOfExtent
        {
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
        public MofUriExtent(IProvider provider, ChangeEventManager changeEventManager = null) :
            base(provider, changeEventManager)
        {
            _navigator = new ExtentUrlNavigator<MofElement>(this);

            if (provider is IHasUriResolver hasUriResolver)
            {
                hasUriResolver.UriResolver = this;
            }
        }


        /// <inheritdoc />
        public MofUriExtent(IProvider provider, string uri, ChangeEventManager changeEventManager = null) :
            this(provider, changeEventManager)
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
        public IElement Resolve(string uri, ResolveType resolveType, bool traceFailing = true)
        {
            var result = ResolveInternal(uri, resolveType);
            if (result == null && traceFailing)
            {
                Logger.Trace($"URI not resolved: {uri}");
            }

            return result;
        }

        private IElement ResolveInternal(string uri, ResolveType resolveType)
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
                    var workspaceResult = _Workspace?.Resolve(uri, resolveType, false);
                    if (workspaceResult != null)
                    {
                        return workspaceResult;
                    }
                }
            }

            var alreadyVisited = new HashSet<Runtime.Workspaces.Workspace>();
            
            if ((resolveType & (ResolveType.NoWorkspace | ResolveType.NoMetaWorkspaces)) == 0)
            {
                // Now look into the explicit extents, if no specific constraint is given
                foreach (var metaExtent in MetaExtents)
                {
                    var element = metaExtent.element(uri);
                    if (element != null)
                    {
                        return element;
                    }
                }
                
                var workspaceResult = ResolveByMetaWorkspaces(uri, _Workspace, alreadyVisited);
                if (workspaceResult != null)
                {
                    return workspaceResult;
                }
            }

            // If still not found, do a full search in every extent in every workspace
            if (resolveType == ResolveType.Default)
            {
                foreach (var workspace in GiveMe.Scope.WorkspaceLogic.Workspaces)
                {
                    if (alreadyVisited.Contains(workspace))
                    {
                        continue;
                    }

                    foreach (var extent in workspace.extent.OfType<IUriExtent>())
                    {
                        var result = extent.element(uri);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                }
            }

            return null;
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