using DatenMeister.Core;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Runtime.Plugins;
using DatenMeister.WPF.Modules.UserInteractions;

namespace DatenMeister.WPF.Modules.ExtentPropertyElementHandler
{
    /// <summary>
    /// This plugin allows the definition of extentproperties in the extent property dialog.
    /// It will create a user interaction for each defined Property Type. 
    /// </summary>
    [PluginDependency(typeof(UserInteractionPlugin))]
    [PluginLoading(PluginLoadingPosition.AfterInitialization)]
    public class ExtentPropertyInteractionPlugin : IDatenMeisterPlugin
    {
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly IScopeStorage _scopeStorage;

        public ExtentPropertyInteractionPlugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
        }

        public void Start(PluginLoadingPosition position)
        {
             _scopeStorage.Get<UserInteractionState>().ElementInteractionHandler.Add(
                 new ExtentPropertyUserInteraction(_workspaceLogic, _scopeStorage));
        }
    }
}