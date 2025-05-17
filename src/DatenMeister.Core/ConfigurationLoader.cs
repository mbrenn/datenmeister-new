using System.Reflection;
using System.Xml.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Provider.Xmi;

namespace DatenMeister.Core
{
    public class ConfigurationLoader
    {
        /// <summary>
        /// Defines the XmiFileName
        /// </summary>
        public const string XmiFileName = "DatenMeister.Settings.xmi";
        
        /// <summary>
        /// Defines the class logger
        /// </summary>
        private static readonly ILogger Logger = new ClassLogger(typeof(ConfigurationLoader));

        /// <summary>
        /// Loads the settings from the given file.
        /// If file path is not set, the default XmiFilename will be used
        /// </summary>
        /// <param name="path">Path from which the configuration shall be loaded</param>
        /// <returns>The loaded Xml document</returns>
        public static MofExtent LoadSetting(string? path = null)
        {
            path ??= XmiFileName;

            if (!Path.IsPathRooted(path))
            {
                var assembly = Assembly.GetEntryAssembly() ??
                               throw new InvalidOperationException("Entry assembly is null");

                var assemblyDirectoryName = Path.GetDirectoryName(assembly.Location) ??
                                            throw new InvalidOperationException("Assembly Directory Name is null");

                path = Path.Combine(assemblyDirectoryName, path);
            }

            try
            {
                if (File.Exists(path))
                {
                    Logger.Info($"Loading public integration from {path}");
                    var loadedXml = XDocument.Load(path);
                    return LoadConfigurationByXml(loadedXml);
                }
            }
            catch (Exception exc)
            {
                Logger.Error($"Exception occured during Loading of Xmi: {exc.Message}");
            }

            Logger.Error($"There is no configuration file at {path}");
            throw new InvalidOperationException($"There is no configuration file at {path}");
        }

        /// <summary>
        /// Loads the configuration by the xml
        /// </summary>
        /// <param name="loadedXml">The xml Document storing the configuration</param>
        /// <returns>The extent</returns>
        public static MofExtent LoadConfigurationByXml(XDocument loadedXml)
        {
            var provider = new XmiProvider(loadedXml);
            return new MofExtent(provider);
        }
    }
}