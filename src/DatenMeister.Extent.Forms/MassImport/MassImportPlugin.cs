using DatenMeister.Actions;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core;
using DatenMeister.Core.Interfaces;
using DatenMeister.Plugins;

namespace DatenMeister.Extent.Forms.MassImport;

[PluginLoading(PluginLoadingPosition.AfterLoadingOfExtents)]
internal class MassImportPlugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    public Task Start(PluginLoadingPosition position)
    {
        scopeStorage.Get<ActionLogicState>().AddActionHandler(
            new PerformMassImportActionHandler(workspaceLogic, scopeStorage));

        return Task.CompletedTask;
    }
}