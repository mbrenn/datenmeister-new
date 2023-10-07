using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Proxies;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Implements the MOF interface for the uriextent
    /// </summary>
    public partial class MofUriExtent : MofExtent, IUriExtent, IUriResolver, IHasAlternativeUris
    {
        /// <summary>
        /// Defines the name for the uri property
        /// </summary>
        public const string UriPropertyName = "__uri";

        private const string AlternativeUrlsProperty = "__alternativeUrls";

        /// <summary>
        /// Defines a possible logger
        /// </summary>
        private static readonly ClassLogger Logger = new(typeof(MofUriExtent));

        private IWorkspaceLogic? _cachedWorkspaceLogic;

        /// <summary>
        /// Stores the navigator
        /// </summary>
        private readonly ExtentUrlNavigator _navigator;

        /// <summary>
        /// Stores the resolver cache
        /// </summary>
        private readonly ResolverCache _resolverCache = new();

        /// <inheritdoc />
        public MofUriExtent(
            IProvider provider,
            IScopeStorage? scopeStorage) :
            base(provider, scopeStorage)
        {
            _navigator = new ExtentUrlNavigator(this, scopeStorage);

            if (provider is IHasUriResolver hasUriResolver)
            {
                hasUriResolver.UriResolver = this;
            }

            MetaXmiElement?.Extent?.AddMetaExtent(this);

            if (scopeStorage != null)
            {
                _cachedWorkspaceLogic = new WorkspaceLogic(scopeStorage);
            }
        }

        /// <summary>
        /// Associates a certain workspace logic to the extent.
        /// This method can be used to allow that references are resolved through the workspace logic
        /// It is especially required for temporary extents
        /// </summary>
        /// <param name="workspaceLogic"></param>
        public void AssociateWorkspaceLogic(IWorkspaceLogic workspaceLogic)
        {
            _cachedWorkspaceLogic = workspaceLogic;
        }


        /// <inheritdoc />
        public MofUriExtent(IProvider provider, string uri, IScopeStorage? scopeStorage) :
            this(provider, scopeStorage)
        {
            UriOfExtent = uri;
        }

        /// <summary>
        /// Gets an enumeration of alternative uris
        /// </summary>
        public IList<string> AlternativeUris
        {
            get => new ReflectiveList<string>(new MofReflectiveSequence(GetMetaObject(), AlternativeUrlsProperty));
            set => set(AlternativeUrlsProperty, value);
        }

        /// <summary>
        /// Gets or sets the uri of the extent
        /// </summary>
        public string UriOfExtent
        {
            get
            {
                var uri = isSet(UriPropertyName) ? get(UriPropertyName) : null;
                if (uri == null)
                {
                    return string.Empty;
                }

                return uri.ToString() ?? string.Empty;
            }

            set => set(UriPropertyName, value);
        }

        /// <inheritdoc />
        public string contextURI()
            => UriOfExtent;

        /// <inheritdoc />
        public string uri(IElement element)
            => _navigator.uri(element);

        /// <inheritdoc />
        public IElement? element(string uri)
            => _navigator.element(uri) as IElement;

        /// <inheritdoc />
        public object? Resolve(string uri, ResolveType resolveType, bool traceFailing = true, string? workspace = null)
        {
            uri = Migration.MigrateUriForResolver(uri);

            // Check, if we have a cache...
            /*var cachedResult = _resolverCache.GetElementFor(uri, resolveType);
            if (cachedResult != null && false)
            {
                // TODO: Find here a better solution than just blocking the resolver
                // Issue is that, when there is a new extent with the same extent uri,
                // duplicate items will be just being able to resolved once, even though
                // a new extent is being created. This effects especially dm://temp
                return cachedResult;
            }*/

            // We have to find it
            var result = ResolveInternal(workspace, uri, resolveType);
            if (result == null && traceFailing)
            {
                Logger.Debug($"URI not resolved: {uri} from Extent: {contextURI()}");
            }

            if (result != null)
            {
                // Deactivated
                //_resolverCache.AddElementFor(uri, resolveType, result);
            }

            return result;
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
        public override string ToString()
            => $"UriExtent: {contextURI()}";

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

        private object? ResolveInternal(string? workspace, string uri, ResolveType resolveType)
        {
            var currentWorkspace = Workspace;
            if (!string.IsNullOrEmpty(currentWorkspace?.id) && !string.IsNullOrEmpty(workspace) && currentWorkspace.id != workspace)
            {
                // Wrong workspace, we need to jump directly into the workspace

                return _cachedWorkspaceLogic?.GetWorkspace(workspace)
                    ?.Resolve(uri, resolveType, false, workspace);
            }

            if (!resolveType.HasFlagFast(ResolveType.OnlyMetaClasses))
            {
                var result = _navigator.element(uri);
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

            var alreadyVisited = new HashSet<Workspace>();

            if ((resolveType & (ResolveType.NoWorkspace | ResolveType.NoMetaWorkspaces)) == 0)
            {
                // Now look into the explicit extents, if no specific constraint is given
                foreach (var element in
                         MetaExtents
                             .OfType<IUriExtent>()
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

            if (resolveType.HasFlagFast(ResolveType.AlsoTypeWorkspace)
                && _cachedWorkspaceLogic != null)
            {
                var typesWorkspace = _cachedWorkspaceLogic.TryGetTypesWorkspace();
                if (typesWorkspace != null)
                {
                    foreach (var result in
                             typesWorkspace.extent
                                 .OfType<IUriExtent>()
                                 .Select(extent => extent.GetUriResolver().Resolve(uri, ResolveType.NoWorkspace, false))
                                 .Where(result => result != null))
                    {
                        return result;
                    }
                }
            }

            // If still not found, do a full search in every extent in every workspace
            if (resolveType.HasFlagFast(ResolveType.Default) && _cachedWorkspaceLogic != null)
            {
                foreach (var innerWorkspace in _cachedWorkspaceLogic.Workspaces)
                {
                    if (innerWorkspace.IsDynamicWorkspace) continue;
                    if (alreadyVisited.Contains(innerWorkspace))
                    {
                        continue;
                    }

                    // Check, if there is an extent with the name
                    var extent = innerWorkspace.FindExtent(uri);
                    if (extent != null)
                    {
                        return extent;
                    }

                    // If there is not an extent, then check, if there is an item
                    foreach (var result in
                             innerWorkspace.extent
                                 .OfType<IUriExtent>()
                                 .Select(innerExtent => innerExtent.element(uri))
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
            Workspace? workspace,
            HashSet<Workspace>? alreadyVisited = null)
        {
            alreadyVisited ??= new HashSet<Workspace>();
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
        /// Clears all resolve caches
        /// </summary>
        internal void ClearResolveCache()
        {
            _resolverCache.Clear();
            _navigator.ClearResolveCache();
        }

        private class ResolverCache
        {
            private readonly ConcurrentDictionary<ResolverKey, object> _cache = new();

            public void Clear()
            {
                _cache.Clear();
            }

            public object? GetElementFor(string uri, ResolveType resolveType)
            {
                return _cache.TryGetValue(new ResolverKey(uri, resolveType), out var result) ? result : null;
            }

            public void AddElementFor(string uri, ResolveType resolveType, object foundElement)
            {
                _cache[new ResolverKey(uri, resolveType)] = foundElement;
            }

            private class ResolverKey
            {
                private readonly ResolveType _resolveType;
                private readonly string _uri;

                public ResolverKey(string uri, ResolveType resolveType)
                {
                    _uri = uri;
                    _resolveType = resolveType;
                }

                protected bool Equals(ResolverKey other)
                {
                    return string.Equals(_uri, other._uri, StringComparison.InvariantCulture) &&
                           _resolveType == other._resolveType;
                }

                public override bool Equals(object? obj)
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
    }
}