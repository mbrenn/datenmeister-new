using System.Diagnostics;
using BurnSystems;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;

namespace DatenMeister.Actions.ActionHandler
{
    public class PowershellExecutionActionHandler : IActionHandler
    {
        /// <summary>
        /// Defines the logger 
        /// </summary>
        private readonly ClassLogger _logger = new(typeof(CommandExecutionActionHandler));
        
        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.equals(
                _DatenMeister.TheOne.Actions.__PowershellExecutionAction) == true;
        }

        public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
        {
            await Task.Run(() =>
            {
                var script = action.getOrDefault<string>(_DatenMeister._Actions._PowershellExecutionAction.script);
                var workingDirectory =
                    action.getOrDefault<string>(_DatenMeister._Actions._PowershellExecutionAction.workingDirectory);

                var tempPath = Path.Combine(Path.GetTempPath(),
                    StringManipulation.RandomString(16) + ".ps1");

                File.WriteAllText(tempPath, script);

                _logger.Info("Powershell started");
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

                _logger.Info("Powershell exited");
            });

            return null;
        }
    }
}