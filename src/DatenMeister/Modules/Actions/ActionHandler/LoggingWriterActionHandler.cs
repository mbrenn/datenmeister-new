using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.Actions.ActionHandler
{
    /// <summary>
    /// Defines the logging action handler
    /// </summary>
    public class LoggingWriterActionHandler : IActionHandler
    {
        /// <summary>
        /// Defines the class logger
        /// </summary>
        private static readonly ILogger ClassLogger = new ClassLogger(typeof(LoggingWriterActionHandler));

        /// <summary>
        /// Gets or sets the last message that was sent into the logging.
        /// This is mainly used for testing issuees
        /// </summary>
        public static string LastMessage { get; set; } = string.Empty;
        
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
                LastMessage = message;
                ClassLogger.Info(message);
            }
        }
    }
}