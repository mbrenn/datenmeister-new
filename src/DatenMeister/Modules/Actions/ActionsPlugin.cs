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
            _scopeStorage.Add(new ActionLogicState());
        }

        /// <summary>
        /// Executes the given action
        /// </summary>
        /// <param name="actionSet">Actions-Set to be executed</param>
        public async void ExecuteActionSet(IElement actionSet)
        {
            var actions = actionSet.getOrDefault<IReflectiveCollection>(
                _Actions._ActionSet.action);
            foreach (var action in actions.OfType<IElement>())
            {
                await ExecuteAction(action);
            }
        }

        /// <summary>
        /// Executes a certain action 
        /// </summary>
        /// <param name="action">Action to be executed</param>
        public async Task ExecuteAction(IElement action)
        {
            foreach (var actionHandler in _scopeStorage.Get<ActionLogicState>().ActionHandlers)
            {
                if (actionHandler.IsResponsible(action))
                {
                    await Task.Run(() =>
                        actionHandler.Evaluate(this, action));
                }
            }
        }
    }
}