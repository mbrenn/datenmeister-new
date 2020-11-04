using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Implementation.DotNet;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.XMI.EMOF;

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
            if (File.Exists(path))
            {
                try
                {
                    Logger.Info($"Loading public integration from {path}");
                    var provider = new XmiProvider(XDocument.Load(path));
                    var extent = new MofExtent(provider);
                    if (!(extent.elements().FirstOrDefault() is IObject element))
                    {
                        var message = "The given extent does not contain an element being usable for conversion";
                        Logger.Warn(message);
                        throw new InvalidOperationException(message);
                    }

                    var settings = DotNetConverter.ConvertToDotNetObject<PublicIntegrationSettings>(element);
                    settings.settingsFilePath = path;
                    settings.databasePath = settings.databasePath != null
                        ? Environment.ExpandEnvironmentVariables(settings.databasePath)
                        : null;

                    if (!string.IsNullOrEmpty(settings.databasePath))
                    {
                        Environment.SetEnvironmentVariable("dm_databasepath", settings.databasePath);
                    }

                    foreach (var variable in settings.environmentVariable
                        .Where(variable => variable.key != null && variable.value != null))
                    {
                        Environment.SetEnvironmentVariable(variable.key!, variable.value);
                    }

                    return settings;
                }
                catch (Exception exc)
                {
                    Logger.Error($"Exception occured during Loading of Xmi: {exc.Message}");
                }
            }

            Logger.Info($"No Xmi-File for external configuration found in: {path}");
            return null;
        }
    }
}