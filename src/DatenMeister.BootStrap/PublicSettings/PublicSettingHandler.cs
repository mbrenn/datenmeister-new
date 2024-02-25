using System;
using System.IO;
using System.Linq;
using System.Reflection;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation.DotNet;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.BootStrap.PublicSettings
{
    /// <summary>
    /// This is helper class being used to load and understand the public setting
    /// </summary>
    public class PublicSettingHandler
    {
        /// <summary>
        /// Defines the XmiFileName
        /// </summary>
        public const string XmiFileName = "DatenMeister.Settings.xmi";

        /// <summary>
        /// Defines the class logger
        /// </summary>
        private static readonly ILogger Logger = new ClassLogger(typeof(PublicSettingHandler));

        /// <summary>
        /// Loads the public settings for the DatenMeister
        /// </summary>
        /// <param name="directoryPath">Path to the directory</param>
        /// <param name="configurationExtent">The configuration extent</param>
        /// <returns>The found public integrations settings</returns>
        public static PublicIntegrationSettings? LoadSettingsFromDirectory(
            string directoryPath, out IExtent? configurationExtent)
        {
            configurationExtent = LoadExtentFromDirectory(directoryPath, out var path);

            if (configurationExtent != null)
            {
                var result = ParseSettingsFromFile(configurationExtent);
        
                if (result == null)
                {
                    Logger.Info($"No Configuration file found in {path}");
                    return null;
                }
                
                result.settingsFilePath = path;
                return result;
            }

            Logger.Info($"No Configuration file found in {path}");

            return null;
        }

        /// <summary>
        /// Loads the extent from the directory
        /// </summary>
        /// <param name="directoryPath">Path of the directory</param>
        /// <param name="path">Path to file where the configuration is loaded</param>
        /// <returns>The extent storing the configuration</returns>
        public static IExtent? LoadExtentFromDirectory(string directoryPath, out string path)
        {
            path = Path.Combine(directoryPath, XmiFileName);

            if (File.Exists(path))
            {
                try
                {
                    // Loads the settings
                    Logger.Info($"Loading public integration from {path}");
                    return ConfigurationLoader.LoadSetting(path);
                }
                catch (Exception exc)
                {
                    Logger.Error($"Exception occured during Loading of Xmi: {exc.Message}");
                }
            }

            return null;
        }

        /// <summary>
        /// Loads the settings from 
        /// </summary>
        /// <param name="extentConfiguration">The extent which was loaded and which
        /// contains the configuration. It can be null, if no configuration
        /// has been found</param>
        /// <returns>The integration settings</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static PublicIntegrationSettings? ParseSettingsFromFile(
            IExtent? extentConfiguration)
        {
            if (extentConfiguration == null)
            {
                return null;
            }

            // We got a configuration, so we can try to parse it. 
            var settings = ParsePublicIntegrationSettings(extentConfiguration);

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

        /// <summary>
        /// Parses the extent for configuration and returns the public integration
        /// setting information
        /// </summary>
        /// <param name="extent">Extent to be parsed. All elements will be evaluated
        /// </param>
        /// <returns>The configuration itself</returns>
        private static PublicIntegrationSettings? ParsePublicIntegrationSettings(IExtent extent)
        {
            PublicIntegrationSettings? settings = null;
            foreach (var element in extent.elements().OfType<IElement>())
            {
                settings = DotNetConverter.ConvertToDotNetObject<PublicIntegrationSettings>(element);

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

            return settings;
        }
    }
}