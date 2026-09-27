using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Plugins;
using DatenMeister.Plugins.Helper;

namespace DatenMeister.Web.StaticPages;

// ReSharper disable once UnusedType.Global
public class Plugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    public Task Start(PluginLoadingPosition position)
    {
        // Defines the pluginhelper
        var pluginHelper = new PluginHelper(
            new PluginHelperConfiguration
            {
                PluginType = typeof(Plugin),
                Position = position,
                ScopeStorage = scopeStorage,
                WorkspaceLogic = workspaceLogic
            });
        
        pluginHelper.AddActionHandler(new CreateStaticWebPageActionHandler());
        return Task.CompletedTask;
    }
}