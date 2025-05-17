using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;

namespace DatenMeister.Actions.ActionHandler
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
            return node.getMetaClass()?.equals(
                _DatenMeister.TheOne.Actions.__LoggingWriterAction) == true;
        }

        /// <summary>
        /// Evaluates the plugin 
        /// </summary>
        /// <param name="actionLogic">Action plugin to be added</param>
        /// <param name="action">Action to be executed</param>
        public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
        {
            await Task.Run(() =>
            {
                var message = action.getOrDefault<string>(_DatenMeister._Actions._LoggingWriterAction.message);
                if (message != null)
                {
                    LastMessage = message;
                    ClassLogger.Info(message);
                }
            });

            return null;
        }
    }
}