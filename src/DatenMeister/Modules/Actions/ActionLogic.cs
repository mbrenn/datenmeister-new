using System;
using System.Linq;
using System.Threading;
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
    public class ActionSetExecutionState
    {
        /// <summary>
        /// Stores the number of actions
        /// </summary>
        private int _numberOfActions;

        /// <summary>
        /// Stores the number of actions being executed
        /// </summary>
        public int NumberOfActions => _numberOfActions;

        public void IncrementNumberOfActions()
        {
            Interlocked.Increment(ref _numberOfActions);
        }
    }
    
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
        public async Task<ActionSetExecutionState> ExecuteActionSet(IElement actionSet)
        {
            var actionSetExecutionState = new ActionSetExecutionState();
            var actions = actionSet.getOrDefault<IReflectiveCollection>(
                _DatenMeister._Actions._ActionSet.action);
            if (actions == null)
            {
                // Nothing to be executed
                return actionSetExecutionState;
            }
            
            foreach (var action in actions.OfType<IElement>())
            {
                await ExecuteAction(action);
                actionSetExecutionState.IncrementNumberOfActions();
            }

            return actionSetExecutionState;
        }

        /// <summary>
        /// Executes a certain action 
        /// </summary>
        /// <param name="action">Action to be executed</param>
        public async Task ExecuteAction(IElement action)
        {
            var found = false;
            foreach (var actionHandler in ScopeStorage.Get<ActionLogicState>().ActionHandlers)
            {
                if (actionHandler.IsResponsible(action))
                {
                    await Task.Run(() =>
                        actionHandler.Evaluate(this, action));
                    found = true;
                }
            }

            if (!found)
            {
                var message = $"Did not found action handler for {action}";
                ClassLogger.Warn(message);
                throw new InvalidOperationException(message);
            }
        }
    }
}