using System;
using System.Diagnostics;
using System.IO;
using BurnSystems;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.Actions.ActionHandler
{
    public class PowershellExecutionActionHandler : IActionHandler
    {
        /// <summary>
        /// Defines the logger 
        /// </summary>
        private readonly ClassLogger Logger = new ClassLogger(typeof(CommandExecutionActionHandler));
        
        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.@equals(
                _Actions.TheOne.__PowershellExecutionAction) == true;
        }

        public void Evaluate(ActionLogic actionLogic, IElement action)
        {
            var script = action.getOrDefault<string>(_Actions._PowershellExecutionAction.script);
            var workingDirectory = action.getOrDefault<string>(_Actions._PowershellExecutionAction.workingDirectory);

            var tempPath = Path.Combine(Path.GetTempPath(),
                StringManipulation.RandomString(16) + ".ps1");

            File.WriteAllText(tempPath, script);

            Logger.Info($"Powershell started");
            var startInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe", 
                UseShellExecute = true, 
                Arguments = tempPath
            };

            if (!string.IsNullOrEmpty(workingDirectory))
            {
                startInfo.WorkingDirectory = workingDirectory;
            }

            var process = Process.Start(startInfo)
                          ?? throw new InvalidOperationException("Process was not created");
            process.WaitForExit();
            
            File.Delete(tempPath);

            Logger.Info($"Powershell exited");
        }
    }
}