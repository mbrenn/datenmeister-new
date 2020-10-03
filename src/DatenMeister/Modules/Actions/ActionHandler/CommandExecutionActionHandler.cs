using System;
using System.Diagnostics;
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
                _DatenMeister.TheOne.Actions.__CommandExecutionAction) == true;
        }

        public void Evaluate(ActionLogic actionLogic, IElement action)
        {
            var command = action.getOrDefault<string>(_DatenMeister._Actions._CommandExecutionAction.command);
            var arguments = action.getOrDefault<string>(_DatenMeister._Actions._CommandExecutionAction.arguments);
            var workingDirectory = action.getOrDefault<string>(_DatenMeister._Actions._CommandExecutionAction.workingDirectory);

            Logger.Info($"Process started: {command} {arguments}");
            var startInfo = new ProcessStartInfo
            {
                FileName = command,
                UseShellExecute = true
            };

            if (!string.IsNullOrEmpty(arguments))
            {
                startInfo.Arguments = arguments;
            }

            if (!string.IsNullOrEmpty(workingDirectory))
            {
                startInfo.WorkingDirectory = workingDirectory;
            }

            var process = Process.Start(startInfo)
                          ?? throw new InvalidOperationException("Process was not created");
            process.WaitForExit();

            Logger.Info($"Process exited: {command} {arguments}");
        }
    }
}