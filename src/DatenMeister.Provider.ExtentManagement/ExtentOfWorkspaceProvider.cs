using Autofac;
using DatenMeister.Core;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Provider;
using DatenMeister.Core.Provider;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.DependencyInjection;
using DatenMeister.Extent.Manager.ExtentStorage;

namespace DatenMeister.Provider.ExtentManagement;

/// <summary>
///     Contains all workspaces in an extent like structure
/// </summary>
public class ExtentOfWorkspaceProvider : IProvider
{
    /// <summary>
    ///     Gets the uri of the extent which contains the workspaces
    /// </summary>
    public const string WorkspaceTypeUri = WorkspaceNames.UriExtentInternalTypes;

    /// <summary>
    ///     Stores the capabilities of the provider
    /// </summary>
    /// <returns></returns>
    private readonly ProviderCapability _providerCapability = new()
    {
        IsTemporaryStorage = true,
        CanCreateElements = false
    };

    /// <summary>
    ///     Initializes a new instance of the ExtentOfWorkspaces
    /// </summary>
    /// <param name="scope">The dependency inject</param>
    public ExtentOfWorkspaceProvider(IDatenMeisterScope scope)
    {
        WorkspaceLogic = scope.WorkspaceLogic;
        ExtentManager = scope.Resolve<ExtentManager>();
    }

    /// <summary>
    ///     Initializes a new instance of the ExtentOfWorkspaceProvider
    /// </summary>
    /// <param name="workspaceLogic">The logic of the workspace</param>
    /// <param name="scopeStorage">The scope storage</param>
    public ExtentOfWorkspaceProvider(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
    {
        ExtentManager = new ExtentManager(workspaceLogic, scopeStorage);
        WorkspaceLogic = workspaceLogic;
    }

    public ExtentManager ExtentManager { get; set; }

    /// <summary>
    ///     Gets the workspace logic for the elements
    /// </summary>
    public IWorkspaceLogic WorkspaceLogic { get; }

    public IProviderObject CreateElement(string? metaClassUri)
    {
        throw new InvalidOperationException("The creation of new elements is not supported via MOF." +
                                            "Use WorkspaceLogic to create new extents");
    }

    public void AddElement(IProviderObject? valueAsObject, int index = -1)
    {
        throw new NotImplementedException("Not implemented");
    }

    public bool DeleteElement(string id)
    {
        throw new NotImplementedException("Not implemented");
    }

    public void DeleteAllElements()
    {
        throw new NotImplementedException("Not implemented");
    }

    public IProviderObject? Get(string? id)
    {
        if (id == null || string.IsNullOrEmpty(id)) return null;

        var result = WorkspaceLogic.GetWorkspace(id);
        if (result == null) return null;

        return new WorkspaceObject(this, result);
    }

    public IEnumerable<IProviderObject> GetRootObjects()
    {
        var workspaces = WorkspaceLogic.Workspaces;
        return workspaces.Select(x => new WorkspaceObject(this, x));
    }

    /// <summary>
    ///     Gets the capabilities of the provider
    /// </summary>
    /// <returns></returns>
    public ProviderCapability GetCapabilities()
    {
        return _providerCapability;
    }
}