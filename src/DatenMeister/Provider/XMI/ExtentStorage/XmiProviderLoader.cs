using System;
using System.IO;
using System.Xml.Linq;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Runtime.Locking;

namespace DatenMeister.Provider.XMI.ExtentStorage
{
    public class XmiProviderLoader : IProviderLoader, IProviderLocking
    {
        private static readonly ClassLogger Logger = new ClassLogger(typeof(XmiProviderLoader));

        public IWorkspaceLogic? WorkspaceLogic { get; set; }

        public IScopeStorage? ScopeStorage { get; set; }

        /// <summary>
        /// XmiStorageLoaderConfig
        /// </summary>
        /// <param name="configuration">XmiStorageLoaderConfig</param>
        /// <param name="extentCreationFlags"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public LoadedProviderInfo LoadProvider(
            IElement configuration,
            ExtentCreationFlags extentCreationFlags)
        {
            var filePath =
                configuration.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.filePath);

            XDocument xmlDocument;
            if (!File.Exists(filePath) || extentCreationFlags == ExtentCreationFlags.CreateOnly)
            {
                if (extentCreationFlags != ExtentCreationFlags.LoadOnly)
                {
                    xmlDocument = CreateEmptyXmiDocument(configuration);
                }
                else
                {
                    throw new InvalidOperationException(
                        $"File not found: {filePath}");
                }
            }
            else
            {
                try
                {
                    xmlDocument = XDocument.Load(filePath);
                }
                catch (Exception exc)
                {
                    Logger.Warn(exc.ToString());
                    xmlDocument = CreateEmptyXmiDocument(configuration);
                }
            }

            return new LoadedProviderInfo(new XmiProvider(xmlDocument));
        }

        /// <summary>
        /// Creates an empty Xmi document as given by the configuration
        /// </summary>
        /// <param name="configuration">Xmi Configuration being used. XmiStorageLoaderConfig</param>
        /// <returns>Found XDocument</returns>
        private static XDocument CreateEmptyXmiDocument(IElement configuration)
        {
            var filePath = 
                configuration.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.filePath);
            
            CreateDirectoryIfNecessary(configuration);

            // We need to create an empty Xmi file... Not the best thing at the moment, but we try it.
            var xmlDocument = new XDocument(
                new XElement(XmiProvider.DefaultRootNodeName));

            // Try to create file, to verify that file access and other activities are given
            xmlDocument.Save(filePath);
            return xmlDocument;
        }

        
        /// <summary>
        /// XmiStorageLoaderConfig
        /// </summary>
        /// <param name="configuration">configuration as XmiStorage </param>
        private static void CreateDirectoryIfNecessary(IElement configuration)
        {
            var filePath = 
                configuration.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.filePath);
            // Creates directory if necessary
            var directoryPath = Path.GetDirectoryName(filePath)
                                ?? throw new InvalidOperationException("directoryPath is null");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        public void StoreProvider(IProvider extent, IElement configuration)
        {
            var filePath =
                configuration.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.filePath);

            if (!(extent is XmiProvider xmlExtent))
                throw new InvalidOperationException("Only XmlUriExtents are supported");

            // Deletes existing file
            if (File.Exists(filePath))
                File.Delete(filePath);

            // Loads existing file
            using (var fileStream = File.OpenWrite(filePath))
            {
                xmlExtent.Document.Save(fileStream);
            }
        }

        /// <summary>
        /// Gets the locking path for the provider
        /// </summary>
        /// <param name="config">Configuration storing the locking path</param>
        /// <returns></returns>
        public string GetLockFilePath(IElement config)
        {
            var filePath =
                config.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.filePath);
            if (string.IsNullOrEmpty(filePath))
            {
                throw new InvalidOperationException(
                    "The locking path could not be retrieved because the configuration is empty. ");
            }

            return filePath + ".lock";
        }

        public bool IsLocked(IElement configuration)
        {
            var lockingLogic =
                new LockingLogic(ScopeStorage ?? throw new InvalidOperationException("ScopeStorage == null"));

            var path = GetLockFilePath(configuration);
            return lockingLogic.IsLocked(path);
        }

        public void Lock(IElement configuration)
        {
            var lockingLogic =
                new LockingLogic(ScopeStorage ?? throw new InvalidOperationException("ScopeStorage == null"));
           

            var path = GetLockFilePath(configuration);
            lockingLogic.Lock(path);
        }

        public void Unlock(IElement configuration)
        {
            var lockingLogic =
                new LockingLogic(ScopeStorage ?? throw new InvalidOperationException("ScopeStorage == null"));

            var path = GetLockFilePath(configuration);
            lockingLogic.Unlock(path);
        }
    }
}