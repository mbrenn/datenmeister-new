using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.Actions.ActionHandler
{
    public class CommandExecutionActionHandler : IActionHandler
    {
        /// <summary>
        /// Defines the logger 
        /// </summary>
        private readonly ClassLogger Logger = new ClassLogger(typeof(CommandExecutionActionHandler));
            
        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.@equals(
                _Actions.TheOne.__CommandExecutionAction) == true;
        }

        public void Evaluate(ActionLogic actionsLogic, IElement action)
        {
            var command = action.getOrDefault<string>(_Actions._CommandExecutionAction.command);
            var arguments = action.getOrDefault<string>(_Actions._CommandExecutionAction.arguments);

            Logger.Info($"Process started: {command} {arguments}");

            var process = DotNetHelper.CreateProcess(command, arguments);
            process.WaitForExit();

            Logger.Info($"Process exited: {command} {arguments}");
        }
    }
}