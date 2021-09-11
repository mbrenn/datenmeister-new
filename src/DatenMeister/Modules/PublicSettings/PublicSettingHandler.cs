using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Implementation.DotNet;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Provider.Xmi;

namespace DatenMeister.Modules.PublicSettings
{
    /// <summary>
    /// This is helper class being used to load and understand the public setting
    /// </summary>
    public class PublicSettingHandler
    {
        /// <summary>
        /// Defines the XmiFileName
        /// </summary>
        public static string XmiFileName = "DatenMeister.Settings.xmi";
        
        /// <summary>
        /// Defines the class logger
        /// </summary>
        private static readonly ILogger Logger = new ClassLogger(typeof(PublicSettingHandler));

        /// <summary>
        /// Loads the public settings for the DatenMeister
        /// </summary>
        /// <param name="directoryPath">Path to the directory</param>
        /// <returns>The found public integrations settings</returns>
        public static PublicIntegrationSettings? LoadSettingsFromDirectory(string directoryPath)
        {
            var path = Path.Combine(directoryPath, XmiFileName);

            var result = LoadSettingsFromFile(path);
            if (result == null)
            {
                Logger.Info($"No Configuration file found in {directoryPath}");
            }

            return result;
        }

        /// <summary>
        /// Loads the settings from 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private static PublicIntegrationSettings? LoadSettingsFromFile(string path)
        {
            PublicIntegrationSettings? settings = null;
            
            if (File.Exists(path))
            {
                try
                {
                    Logger.Info($"Loading public integration from {path}");
                    var extent = ConfigurationLoader.LoadSetting(path);
                    // Goes through all elements
                    foreach (var element in extent.elements().OfType<IElement>())
                    {
                        settings = DotNetConverter.ConvertToDotNetObject<PublicIntegrationSettings>(element);
                        settings.settingsFilePath = path;
                        settings.databasePath = settings.databasePath != null
                            ? Environment.ExpandEnvironmentVariables(settings.databasePath)
                            : null;

                        foreach (var variable in settings.environmentVariable
                            .Where(variable => variable.key != null && variable.value != null))
                        {
                            Environment.SetEnvironmentVariable(variable.key!, variable.value);
                            Logger.Info($"Setting Environmental Variable: {variable.key} = {variable.value}");
                        }
                    }
                }
                catch (Exception exc)
                {
                    Logger.Error($"Exception occured during Loading of Xmi: {exc.Message}");
                }
            }
            
            // Now starts to set the set the environment according the public settings
            if (!string.IsNullOrEmpty(settings?.databasePath))
            {
                Environment.SetEnvironmentVariable("dm_DatabasePath", settings?.databasePath);
            }
                                
            // Now set the default paths, if they are not set
            SetEnvironmentVariableToDesktopFolderIfNotExisting(
                "dm_ImportPath", "import");
            SetEnvironmentVariableToDesktopFolderIfNotExisting(
                "dm_ReportPath", "report");
            SetEnvironmentVariableToDesktopFolderIfNotExisting(
                "dm_ExportPath", "export");
                    
            Environment.SetEnvironmentVariable(
                "dm_ApplicationPath",
                Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location));

            Logger.Info($"No Xmi-File for external configuration found in: {path}");
            return settings;

            void SetEnvironmentVariableToDesktopFolderIfNotExisting(string dmImportPath, string folderName)
            {
                var importPath = Environment.GetEnvironmentVariable(dmImportPath);
                if (string.IsNullOrEmpty(importPath))
                {
                    Environment.SetEnvironmentVariable(
                        dmImportPath,
                        Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                            folderName));
                }
            }
        }
    }
}