using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Plugins;

namespace DatenMeister.Extent.Manager.ExtentStorage;

[PluginLoading]
// ReSharper disable once UnusedType.Global
public class EventManagerEventHandler(IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    public Task Start(PluginLoadingPosition position)
    {
        switch (position)
        {
            case PluginLoadingPosition.AfterLoadingOfExtents:
                var workspaceData = scopeStorage.Get<WorkspaceData>();
                workspaceData.WorkspaceRemoved += async (_, y) =>
                {
                    var logic = new WorkspaceLogic(scopeStorage);
                    var manager = new ExtentManager(logic, scopeStorage);
                    await manager.DetachAllExtents(info =>
                        info.Configuration
                            .getOrDefault<string>(
                                _ExtentLoaderConfigs._ExtentLoaderConfig.workspaceId)
                        == y.Id);
                };
                break;
        }

        return Task.CompletedTask;
    }
}