using System.Diagnostics;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Actions.ActionHandler
{
    public class CommandExecutionActionHandler : IActionHandler
    {
        /// <summary>
        /// Defines the logger 
        /// </summary>
        private static readonly ClassLogger Logger = new(typeof(CommandExecutionActionHandler));
            
        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.equals(
                _DatenMeister.TheOne.Actions.__CommandExecutionAction) == true;
        }

        public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
        {
            await Task.Run(() =>
            {
                var command = action.getOrDefault<string>(_DatenMeister._Actions._CommandExecutionAction.command);
                var arguments = action.getOrDefault<string>(_DatenMeister._Actions._CommandExecutionAction.arguments);
                var workingDirectory =
                    action.getOrDefault<string>(_DatenMeister._Actions._CommandExecutionAction.workingDirectory);

                /* Translates the command if required */
                var foundCommand = actionLogic.WorkspaceLogic.GetManagementWorkspace()
                    .GetAllDescendents()
                    .WhenMetaClassIs(_DatenMeister.TheOne.CommonTypes.OSIntegration.__CommandLineApplication)
                    .WhenPropertyHasValue(_DatenMeister._CommonTypes._OSIntegration._CommandLineApplication.name,
                        command)
                    .OfType<IElement>()
                    .FirstOrDefault();
                if (foundCommand != null)
                {
                    var oldCommand = command;
                    command =
                        foundCommand.getOrDefault<string>(
                            _DatenMeister._CommonTypes._OSIntegration._CommandLineApplication.applicationPath);
                    Logger.Info($"Translated process: {oldCommand} {command}");
                }

                // Expands the environment variables
                command = Environment.ExpandEnvironmentVariables(command);

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
            });

            return null;
        }
    }
}