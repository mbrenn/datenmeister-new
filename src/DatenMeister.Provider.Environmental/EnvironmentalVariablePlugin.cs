using DatenMeister.Core;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Plugins;

namespace DatenMeister.Provider.Environmental;

[PluginLoading(PluginLoadingPosition.AfterInitialization |PluginLoadingPosition.AfterLoadingOfExtents)]
// ReSharper disable once UnusedType.Global
public class EnvironmentalVariablePlugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
    : IDatenMeisterPlugin
{
    public const string DefaultExtentUri = "dm:///_internal/environmentalvariables/";

    public IScopeStorage ScopeStorage { get; } = scopeStorage;

    public async Task Start(PluginLoadingPosition position)
    {
        switch (position)
        {
            case PluginLoadingPosition.AfterInitialization:
            {
                var mapper = ScopeStorage.Get<ProviderToProviderLoaderMapper>();
                mapper.AddMapping(
                    _ExtentLoaderConfigs.TheOne.__EnvironmentalVariableLoaderConfig,
                    _ => new EnvironmentalProvider());
                break;
            }
            case PluginLoadingPosition.AfterLoadingOfExtents:
                var mgmtProvider = workspaceLogic.GetManagementWorkspace();
                if (mgmtProvider.FindExtent(DefaultExtentUri) == null)
                {
                    var extentLoader = new ExtentManager(workspaceLogic, ScopeStorage);
                    var loaderConfig = InMemoryObject.CreateEmpty(
                        _ExtentLoaderConfigs.TheOne.__EnvironmentalVariableLoaderConfig);
                    loaderConfig.set(
                        _ExtentLoaderConfigs._EnvironmentalVariableLoaderConfig.extentUri,
                        DefaultExtentUri);
                    loaderConfig.set(
                        _ExtentLoaderConfigs._EnvironmentalVariableLoaderConfig.workspaceId,
                        WorkspaceNames.WorkspaceManagement);
                    await extentLoader.LoadExtent(loaderConfig);
                }

                break;
        }
    }
}