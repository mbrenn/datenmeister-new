using DatenMeister.Integration;
using DatenMeister.Runtime.Plugins;
using DatenMeister.WPF.Modules.UserInteractions;

namespace DatenMeister.Modules.ExtentPropertyElementHandler
{
    /// <summary>
    /// This plugin allows the definition of extentproperties in the extent property dialog.
    /// It will create a user interaction for each defined Property Type. 
    /// </summary>
    [PluginDependency(typeof(UserInteractionPlugin))]
    [PluginLoading(PluginLoadingPosition.AfterInitialization)]
    public class ExtentPropertyInteractionPlugin : IDatenMeisterPlugin
    {
        private readonly IScopeStorage _scopeStorage;

        public ExtentPropertyInteractionPlugin(IScopeStorage scopeStorage)
        {
            _scopeStorage = scopeStorage;
        }

        public void Start(PluginLoadingPosition position)
        {
             _scopeStorage.Get<UserInteractionState>().ElementInteractionHandler.Add(
                 new ExtentPropertyUserInteraction(_scopeStorage));
        }
    }
}