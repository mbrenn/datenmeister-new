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
        public static PublicIntegrationSettings? LoadSettings(string directoryPath)
        {
            var path = Path.Combine(directoryPath, XmiFileName);
            
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
                    settings.databasePath = Environment.ExpandEnvironmentVariables(settings.databasePath);
                    return settings;
                }
                catch (Exception exc)
                {
                    Logger.Error($"Exception occured during Loading of Xmi: {exc.Message}");
                }
            }

            Logger.Info($"No Xmi-File for external configuration found in: {path}");
            Logger.Info($"No Configuration file found in {directoryPath}");
            return null;
        }
    }
}