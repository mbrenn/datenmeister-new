using System.Linq;
using System.Threading.Tasks;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models;
using DatenMeister.Runtime;
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