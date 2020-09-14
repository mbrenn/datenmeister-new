using System.Collections.Generic;
using DatenMeister.Modules.Actions.ActionHandler;

namespace DatenMeister.Modules.Actions
{
    /// <summary>
    /// Stores the data of the action logic within the instance
    /// </summary>
    public class ActionLogicState
    {
        /// <summary>
        /// Gets the action handlers
        /// </summary>
        public List<IActionHandler> ActionHandlers { get; }
            = new List<IActionHandler>();

        /// <summary>
        /// Adds one action handler
        /// </summary>
        /// <param name="actionHandler">ActionHandler to be added</param>
        public void AddActionHandler(IActionHandler actionHandler)
        {
            lock (ActionHandlers)
            {
                ActionHandlers.Add(actionHandler);
            }
        }

        public static ActionLogicState GetDefaultLogicState()
        {
            var logicState = new ActionLogicState();
            logicState.AddActionHandler(new LoggingWriterActionHandler());
            logicState.AddActionHandler(new CommandExecutionActionHandler());

            return logicState;
        }
    }
}