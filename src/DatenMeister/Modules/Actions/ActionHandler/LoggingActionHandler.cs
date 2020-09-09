using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.Actions.ActionHandler
{
    /// <summary>
    /// Defines the logging action handler
    /// </summary>
    public class LoggingActionHandler : IActionHandler
    {
        /// <summary>
        /// Defines the class logger
        /// </summary>
        private static readonly ILogger ClassLogger = new ClassLogger(typeof(LoggingActionHandler));
        
        public bool IsResponsible(IElement node)
        {
            if (node.getMetaClass()?.@equals(
                _Actions.TheOne.__LoggingWriterAction) == true)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Evaluates the plugin 
        /// </summary>
        /// <param name="actionsPlugin">Action plugin to be added</param>
        /// <param name="action">Action to be executed</param>
        public void Evaluate(ActionLogic actionsPlugin, IElement action)
        {
            var message = action.getOrDefault<string>(_Actions._LoggingWriterAction.message);
            if (message != null)
            {
                ClassLogger.Info(message);
            }
        }
    }
}