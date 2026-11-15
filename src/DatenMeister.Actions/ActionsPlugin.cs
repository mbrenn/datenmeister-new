using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Plugins;

namespace DatenMeister.Actions;

/// <inheritdoc />
// ReSharper disable once ClassNeverInstantiated.Global
[PluginLoading(PluginLoadingPosition.AfterInitialization)]
public class ActionsPlugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    public const string DmInternTypesActionsDatenmeister = "dm:///intern.types.actions.datenmeister/";

    public Task Start(PluginLoadingPosition position)
    {
        if (position == PluginLoadingPosition.AfterInitialization)
        {
            var found = scopeStorage.TryGet<ActionLogicState>();
            if (found != null)
            {
                throw new InvalidOperationException("ActionLogicState is already present");
            }

            scopeStorage.Add(ActionLogicState.GetDefaultLogicState());
        }

        if (position != PluginLoadingPosition.AfterLoadingOfExtents)
        {
            AddExtentForTypesFromManifest("Xmi.DatenMeister.Actions.Types.xmi", DmInternTypesActionsDatenmeister);
        }

        return Task.CompletedTask;
    }
    
    /// <summary>
    /// Loads an adds an extent from the manifest. It used the configuration to identify the assembly
    /// </summary>
    /// <param name="manifestPath">Relative path of the manifest without the assembly name.
    /// (e.g. 'Xmi.Types.xmi' for a file 'Xmi/Types.xmi')</param>
    /// <param name="extentName">Name of the extent in which the manifest will be added</param>
    private void AddExtentForTypesFromManifest(string manifestPath, string extentName)
    {
        LoadExtentFromManifest(manifestPath, extentName, WorkspaceNames.WorkspaceTypes);
    }

    private void LoadExtentFromManifest(string manifestPath, string extentName, string workspaceName)
    {
        
        var extentManager = new ExtentManager(workspaceLogic, scopeStorage);
        var resourcePathTypes = AssemblyName + "." + manifestPath;
        extentManager.LoadNonPersistentExtentFromResources(typeof(ActionsPlugin), resourcePathTypes,
            workspaceName, extentName);
    }

    /// <summary>
    /// Gets the name of the assembly without .dll out of the PluginType
    /// </summary>
    private string AssemblyName =>
        Path.GetFileNameWithoutExtension(
            typeof(ActionsPlugin).Assembly.ManifestModule.Name 
            ?? throw new InvalidOperationException("There was no AssemblyQualifedName. This should not happen."));
}