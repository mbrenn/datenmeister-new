using DatenMeister.Core;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Plugins;

namespace DatenMeister.Extent.Manager.ExtentStorage;

[PluginLoading]
// ReSharper disable once UnusedType.Global
public class EventManagerEventHandler : IDatenMeisterPlugin
{
    private readonly IScopeStorage _scopeStorage;

    public EventManagerEventHandler(IScopeStorage scopeStorage)
    {
        _scopeStorage = scopeStorage;
    }

    public Task Start(PluginLoadingPosition position)
    {
        switch (position)
        {
            case PluginLoadingPosition.AfterLoadingOfExtents:
                var workspaceData = _scopeStorage.Get<WorkspaceData>();
                workspaceData.WorkspaceRemoved += async (_, y) =>
                {
                    var logic = new WorkspaceLogic(_scopeStorage);
                    var manager = new ExtentManager(logic, _scopeStorage);
                    await manager.DetachAllExtents(info =>
                        info.Configuration
                            .getOrDefault<string>(
                                _DatenMeister._ExtentLoaderConfigs._ExtentLoaderConfig.workspaceId)
                        == y.Id);
                };
                break;
        }

        return Task.CompletedTask;
    }
}