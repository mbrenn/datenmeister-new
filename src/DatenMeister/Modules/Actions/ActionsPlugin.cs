using DatenMeister.Core;
using DatenMeister.Integration;
using DatenMeister.Runtime.Plugins;

namespace DatenMeister.Modules.Actions
{
    /// <inheritdoc />
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ActionsPlugin : IDatenMeisterPlugin
    {
        /// <summary>
        /// Gets the scope storage
        /// </summary>
        private readonly IScopeStorage _scopeStorage;

        public ActionsPlugin(IScopeStorage scopeStorage)
        {
            _scopeStorage = scopeStorage;
        }
        
        public void Start(PluginLoadingPosition position)
        {
            _scopeStorage.Add(ActionLogicState.GetDefaultLogicState());
        }
    }
}