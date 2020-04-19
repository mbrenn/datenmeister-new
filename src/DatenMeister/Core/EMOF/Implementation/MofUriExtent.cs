#nullable enable

using System;
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
        private class ResolverCache
        {
            private readonly Dictionary<ResolverKey, IElement> _cache
             = new Dictionary<ResolverKey, IElement>();

            public void Clear()
            {
                lock (_cache)
                {
                    _cache.Clear();
                }
            }

            public IElement? GetElementFor(string uri, ResolveType resolveType)
            {
                lock (_cache)
                {
                    return _cache.TryGetValue(new ResolverKey(uri, resolveType), out var result) ? result : null;
                }
            }

            public void AddElementFor(string uri, ResolveType resolveType, IElement foundElement)
            {
                lock (_cache)
                {
                    _cache[new ResolverKey(uri, resolveType)] = foundElement;
                }
            }

            private class ResolverKey
            {
                private readonly string _uri;

                private readonly ResolveType _resolveType;

                public ResolverKey(string uri, ResolveType resolveType)
                {
                    _uri = uri;
                    _resolveType = resolveType;
                }

                protected bool Equals(ResolverKey other)
                {
                    return string.Equals(_uri, other._uri, StringComparison.InvariantCulture) && _resolveType == other._resolveType;
                }

                public override bool Equals(object obj)
                {
                    if (ReferenceEquals(null, obj)) return false;
                    if (ReferenceEquals(this, obj)) return true;
                    if (obj.GetType() != GetType()) return false;
                    return Equals((ResolverKey) obj);
                }

                public override int GetHashCode()
                {
                    unchecked
                    {
                        return (_uri.GetHashCode() * 397) ^ (int) _resolveType;
                    }
                }
            }
        }
        
        /// <summary>
        /// Stores the resolver cache
        /// </summary>
        private ResolverCache _resolverCache = new ResolverCache();
        
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
        public MofUriExtent(IProvider provider, ChangeEventManager? changeEventManager = null) :
            base(provider, changeEventManager)
        {
            _navigator = new ExtentUrlNavigator<MofElement>(this);

            if (provider is IHasUriResolver hasUriResolver)
            {
                hasUriResolver.UriResolver = this;
            }
        }


        /// <inheritdoc />
        public MofUriExtent(IProvider provider, string uri, ChangeEventManager? changeEventManager = null) :
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
            
            SignalUpdateOfContent();
        }

        /// <inheritdoc />
        public string contextURI()
            => UriOfExtent;

        /// <inheritdoc />
        public string uri(IElement element)
            => _navigator.uri(element);

        /// <inheritdoc />
        public IElement? element(string uri)
            => _navigator.element(uri);

        /// <inheritdoc />
        public override string ToString()
            => $"UriExent: {contextURI()}";

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
        public IElement? Resolve(string uri, ResolveType resolveType, bool traceFailing = true)
        {
            // Check, if we have a cache...
            var cachedResult = _resolverCache.GetElementFor(uri, resolveType);
            if (cachedResult != null)
            {
                return cachedResult;
            }
            
            
            // We have to find it
            var result = ResolveInternal(uri, resolveType);
            if (result == null && traceFailing)
            {
                Logger.Trace($"URI not resolved: {uri}");
            }

            if (result != null)
            {
                _resolverCache.AddElementFor(uri, resolveType, result);
            }

            return result;
        }

        private IElement? ResolveInternal(string uri, ResolveType resolveType)
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
                    var workspaceResult = Workspace?.Resolve(uri, resolveType, false);
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
                foreach (var element in
                    MetaExtents
                        .Select(metaExtent => metaExtent.element(uri))
                        .Where(element => element != null))
                {
                    return element;
                }

                var workspaceResult = ResolveByMetaWorkspaces(uri, Workspace, alreadyVisited);
                if (workspaceResult != null)
                {
                    return workspaceResult;
                }
            }

            // If still not found, do a full search in every extent in every workspace
            if (resolveType == ResolveType.Default && GiveMe.Scope?.WorkspaceLogic != null)
            {
                foreach (var workspace in GiveMe.Scope.WorkspaceLogic.Workspaces)
                {
                    if (alreadyVisited.Contains(workspace))
                    {
                        continue;
                    }

                    foreach (var result in
                        workspace.extent
                            .OfType<IUriExtent>()
                            .Select(extent => extent.element(uri))
                            .Where(result => result != null))
                    {
                        return result;
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
        private IElement? ResolveByMetaWorkspaces(
            string uri,
            Runtime.Workspaces.Workspace? workspace,
            HashSet<Runtime.Workspaces.Workspace>? alreadyVisited = null)
        {
            alreadyVisited ??= new HashSet<Runtime.Workspaces.Workspace>();
            if (workspace != null && alreadyVisited.Contains(workspace))
            {
                return null;
            }

            if (workspace != null)
            {
                alreadyVisited.Add(workspace);
            }

            // If still not found, look into the meta workspaces. Nevertheless, no recursion
            var metaWorkspaces = workspace?.MetaWorkspaces;
            if (metaWorkspaces != null)
            {
                foreach (var metaWorkspace in metaWorkspaces)
                {
                    foreach (var element in
                        metaWorkspace.extent
                            .OfType<IUriExtent>()
                            .Select(metaExtent => metaExtent.element(uri))
                            .Where(element => element != null))
                    {
                        return element;
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
        public IElement? ResolveById(string id)
        {
            var uri = contextURI() + "#" + id;
            return element(uri);
        }
    }
}