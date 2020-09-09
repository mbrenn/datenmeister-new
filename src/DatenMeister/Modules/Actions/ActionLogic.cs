using System.Linq;
using System.Threading.Tasks;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.Actions
{
    public class ActionLogic
    {
        private static readonly ILogger ClassLogger = new ClassLogger(typeof(ActionLogic));
        
        public IWorkspaceLogic WorkspaceLogic { get; }
        public IScopeStorage ScopeStorage { get; }

        public ActionLogic(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            WorkspaceLogic = workspaceLogic;
            ScopeStorage = scopeStorage;
        }
        
        /// <summary>
        /// Executes the given action
        /// </summary>
        /// <param name="actionSet">Actions-Set to be executed</param>
        public async Task ExecuteActionSet(IElement actionSet)
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
            foreach (var actionHandler in ScopeStorage.Get<ActionLogicState>().ActionHandlers)
            {
                var found = false;
                if (actionHandler.IsResponsible(action))
                {
                    await Task.Run(() =>
                        actionHandler.Evaluate(this, action));
                    found = false;
                }

                if (!found)
                {
                    ClassLogger.Warn($"Did not found action handler for {action}");
                }
            }
        }
    }
}