using DatenMeister.Core;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Plugins;
using DatenMeister.WPF.Modules.UserInteractions;

namespace DatenMeister.WPF.Modules.ExtentPropertyElementHandler;

/// <summary>
/// This plugin allows the definition of extentproperties in the extent property dialog.
/// It will create a user interaction for each defined Property Type. 
/// </summary>
[PluginDependency(typeof(UserInteractionPlugin))]
[PluginLoading(PluginLoadingPosition.AfterInitialization)]
public class ExtentPropertyInteractionPlugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
    : IDatenMeisterPlugin
{
    public void Start(PluginLoadingPosition position)
    {
        scopeStorage.Get<UserInteractionState>().ElementInteractionHandler.Add(
            new ExtentPropertyUserInteraction(workspaceLogic, scopeStorage));
    }
}