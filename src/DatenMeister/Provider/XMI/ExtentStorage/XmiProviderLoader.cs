using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using BurnSystems.Logging;
using DatenMeister.Integration;
using DatenMeister.Provider.XMI.EMOF;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Provider.XMI.ExtentStorage
{
    [ConfiguredBy(typeof(XmiStorageLoaderConfig))]
    public class XmiProviderLoader : IProviderLoader, IProviderLocking
    {
        private readonly ExtentStorageData _extentStorageData;
        private static readonly ClassLogger Logger = new ClassLogger(typeof(XmiProviderLoader));

        public XmiProviderLoader(IScopeStorage scopeStorage)
        {
            _extentStorageData = scopeStorage.Get<ExtentStorageData>();
        }
        
        private XmiProviderLoader(ExtentStorageData extentStorageData)
        {
            _extentStorageData = extentStorageData;
        }
        
        public XmiProviderLoader Create(ExtentStorageData data) => new XmiProviderLoader(data);
        
        public LoadedProviderInfo LoadProvider(ExtentLoaderConfig configuration, ExtentCreationFlags extentCreationFlags)
        {
            var xmiConfiguration = (XmiStorageLoaderConfig) configuration;

            XDocument xmlDocument;
            if (!File.Exists(xmiConfiguration.filePath) || extentCreationFlags == ExtentCreationFlags.CreateOnly)
            {
                if (extentCreationFlags != ExtentCreationFlags.LoadOnly)
                {
                    xmlDocument = CreateEmptyXmiDocument(xmiConfiguration);
                }
                else
                {
                    throw new InvalidOperationException(
                        $"File not found: {xmiConfiguration.filePath}");
                }
            }
            else
            {
                try
                {
                    xmlDocument = XDocument.Load(xmiConfiguration.filePath);
                }
                catch (Exception exc)
                {
                    Logger.Warn(exc.ToString());
                    xmlDocument = CreateEmptyXmiDocument(xmiConfiguration);
                }
            }

            return new LoadedProviderInfo(new XmiProvider(xmlDocument));
        }

        /// <summary>
        /// Creates an empty Xmi document as given by the configuration
        /// </summary>
        /// <param name="xmiLoaderConfig">Xmi Configuration being used</param>
        /// <returns>Found XDocument</returns>
        private static XDocument CreateEmptyXmiDocument(XmiStorageLoaderConfig xmiLoaderConfig)
        {
            CreateDirectoryIfNecessary(xmiLoaderConfig);

            // We need to create an empty Xmi file... Not the best thing at the moment, but we try it.
            var xmlDocument = new XDocument(
                new XElement(XmiProvider.DefaultRootNodeName));

            // Try to create file, to verify that file access and other activities are given
            xmlDocument.Save(xmiLoaderConfig.filePath);
            return xmlDocument;
        }

        private static void CreateDirectoryIfNecessary(XmiStorageLoaderConfig xmiLoaderConfig)
        {
            // Creates directory if necessary
            var directoryPath = Path.GetDirectoryName(xmiLoaderConfig.filePath)
                                ?? throw new InvalidOperationException("directoryPath is null");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        public void StoreProvider(IProvider extent, ExtentLoaderConfig configuration)
        {
            if (configuration is XmiStorageLoaderConfig xmiConfiguration)
            {
                if (!(extent is XmiProvider xmlExtent))
                    throw new InvalidOperationException("Only XmlUriExtents are supported");

                // Deletes existing file
                if (File.Exists(xmiConfiguration.filePath))
                    File.Delete(xmiConfiguration.filePath);

                // Loads existing file
                using (var fileStream = File.OpenWrite(xmiConfiguration.filePath))
                {
                    xmlExtent.Document.Save(fileStream);
                }
            }
            else
            {
                throw new ArgumentException("Configuration is of an unknown type");
            }
        }

        public string GetLockFilePath(XmiStorageLoaderConfig config)
        {
            if (config.filePath == null || config.filePath != String.Empty)
            {
                
            }
            return config.filePath + ".lock";
        }

        /// <summary>
        /// Defines the timespan for the locking
        /// </summary>
        private readonly TimeSpan _lockingTimeSpan = TimeSpan.FromSeconds(5);

        public bool IsLocked(ExtentLoaderConfig configuration)
        {
            if (!(configuration is XmiStorageLoaderConfig xmiConfiguration))
            {
                return false;
            }

            var path = GetLockFilePath(xmiConfiguration);
            if (File.Exists(path))
            {
                var lastUpdateText = File.ReadAllText(path);
                if (DateTime.TryParse(lastUpdateText, CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out var lastUpdate))
                {
                    if (Math.Abs((DateTime.Now - lastUpdate).Seconds) < _lockingTimeSpan.Seconds)
                    {
                        return true;
                    }
                };
            }

            return false;
        }

        public void Lock(ExtentLoaderConfig configuration)
        {
            if (!(configuration is XmiStorageLoaderConfig xmiConfiguration))
            {
                return;
            }
            
            UpdateLockFile(xmiConfiguration);
            
            Logger.Info("Locking: " + configuration.extentUri);
        }

        private void UpdateLockFile(XmiStorageLoaderConfig xmiConfiguration)
        {
            CreateDirectoryIfNecessary(xmiConfiguration);
            var path = GetLockFilePath(xmiConfiguration);

            var dateTime = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            File.WriteAllText(path, dateTime);
            Logger.Info("Updated Lockfile: " + xmiConfiguration.extentUri);

            Task.Delay(4000).ContinueWith((task) =>
            {
                if (_extentStorageData?.LoadedExtents.Any(x => x.Configuration == xmiConfiguration) == true)
                {
                    UpdateLockFile(xmiConfiguration);
                }
            });
        }

        public void Unlock(ExtentLoaderConfig configuration)
        {
            if (!(configuration is XmiStorageLoaderConfig xmiConfiguration))
            {
                return;
            }
            
            var path = GetLockFilePath(xmiConfiguration);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            
            Logger.Info("Unlocking: " + configuration.extentUri);
        }
    }
}