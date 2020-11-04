﻿using System;
using System.Diagnostics;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.Actions.ActionHandler
{
    public class CommandExecutionActionHandler : IActionHandler
    {
        /// <summary>
        /// Defines the logger 
        /// </summary>
        private static readonly ClassLogger Logger = new ClassLogger(typeof(CommandExecutionActionHandler));
            
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
            
            /* Translates the command if required */
            var foundCommand = actionLogic.WorkspaceLogic.GetManagementWorkspace()
                .GetAllDescendents()
                .WhenMetaClassIs(_DatenMeister.TheOne.CommonTypes.OSIntegration.__CommandLineApplication)
                .WhenPropertyHasValue(_DatenMeister._CommonTypes._OSIntegration._CommandLineApplication.name, command)
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
        }
    }
}