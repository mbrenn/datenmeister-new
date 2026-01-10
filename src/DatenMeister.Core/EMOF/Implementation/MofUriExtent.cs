using System.Collections.Concurrent;
using System.Threading;
using BurnSystems.Logging;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Provider;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Proxies;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.TypeIndexAssembly;
using DatenMeister.Core.TypeIndexAssembly.Model;

namespace DatenMeister.Core.EMOF.Implementation;

/// <summary>
/// Implements the MOF interface for the uriextent
/// </summary>
public partial class MofUriExtent : MofExtent, IUriExtent, IUriResolver, IHasAlternativeUris, IKnowsUri
{
    /// <summary>
    /// Defines the name for the uri property
    /// </summary>
    public const string UriPropertyName = "__uri";

    private const string AlternativeUrlsProperty = "__alternativeUrls";

    private string? _cachedContextUri;

    /// <summary>
    /// Defines a possible logger
    /// </summary>
    private static readonly ClassLogger Logger = new(typeof(MofUriExtent));

    private IWorkspaceLogic? _cachedWorkspaceLogic;

    /// <summary>
    /// Stores the navigator
    /// </summary>
    public ExtentUrlNavigator Navigator { get; }

    /// <inheritdoc />
    public MofUriExtent(
        IProvider provider,
        IScopeStorage? scopeStorage) :
        base(provider, scopeStorage)
    {
        Navigator = new ExtentUrlNavigator(this, scopeStorage);

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
        get => new ReflectiveList<string>(new MofReflectiveSequence(GetMetaObject(), AlternativeUrlsProperty, null));
        set => set(AlternativeUrlsProperty, value);
    }

    /// <summary>
    /// Gets or sets the uri of the extent
    /// </summary>
    public string UriOfExtent
    {
        get
        {
            if (_cachedContextUri != null)
            {
                return _cachedContextUri;
            }
            
            var uri = isSet(UriPropertyName) ? get(UriPropertyName) : null;
            if (uri == null)
            {
                return _cachedContextUri = string.Empty;
            }

            return _cachedContextUri = uri.ToString() ?? string.Empty;
        }

        set
        {
            set(UriPropertyName, value);
            _cachedContextUri = value;
        }
    }

    /// <inheritdoc />
    public string contextURI()
        => UriOfExtent;

    /// <inheritdoc />
    public string uri(IElement element)
        => Navigator.uri(element);

    /// <inheritdoc />
    public IElement? element(string uri)
        => Navigator.element(uri) as IElement;

    private CoreUriResolver? _coreUriResolver;

    /// <summary>
    /// Stores the number of unresolved URIs
    /// </summary>
    public static long UnresolvedUrisCount;

    /// <inheritdoc />
    public object? Resolve(string? uri, ResolveType resolveType, bool traceFailing = true, string? workspace = null)
    {
        uri = Migration.MigrateUriForResolver(uri);
        if (uri == null)
        {
            // We have found nothing or the uri is regarded as being ignored (like the Mof Tags)
            return null;
        }
        
        var queriedWorkspace = string.IsNullOrEmpty(workspace) ? Workspace : _cachedWorkspaceLogic?.GetWorkspace(workspace);

        // We have to find it
        _coreUriResolver ??= new CoreUriResolver(_cachedWorkspaceLogic);
        var result = _coreUriResolver.Resolve(uri, resolveType, queriedWorkspace, this);
        if (result == null && traceFailing)
        {
            Interlocked.Increment(ref UnresolvedUrisCount);
            Logger.Debug($"URI not resolved: {uri} from Extent: {contextURI()}");
            result = new MofObjectShadow(uri);
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

    /// <summary>
    /// Clears all resolve caches
    /// </summary>
    internal void ClearResolveCache()
    {
        Navigator.ClearResolveCache();
    }

    public string Uri => contextURI();
    
    private TypeIndexLogic? _cachedTypeIndexLogic;

    /// <summary>
    /// Finds the model within the Type Indexing by the given meta class url
    /// </summary>
    /// <param name="metaClassUrl">Url of the metaclass</param>
    /// <param name="lookInMetaWorkspace">true, if the meta workspace shall be searched</param>
    /// <returns>Found model or null in case it was not found</returns>
    public ClassModel? FindModel(string metaClassUrl, bool lookInMetaWorkspace = true)
    {
        if (_cachedWorkspaceLogic == null || Workspace == null)
        {
            return null;
        }

        _cachedTypeIndexLogic ??= new TypeIndexLogic(_cachedWorkspaceLogic);

        return lookInMetaWorkspace 
            ? _cachedTypeIndexLogic.FindClassModelByUrlWithinMetaWorkspaces(Workspace.id, metaClassUrl)
            : _cachedTypeIndexLogic.FindClassModelByUrlWithinWorkspace(Workspace.id, metaClassUrl);
    }
}