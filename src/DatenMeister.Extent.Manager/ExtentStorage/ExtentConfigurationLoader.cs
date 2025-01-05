using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Extent.Manager.ExtentStorage
{
    /// <summary>
    /// This loader is used to store and load the extent storage out of a file.
    /// In addition, it will also use the ExtentManager class to load the actual data
    /// of the extents
    /// </summary>
    public partial class ExtentConfigurationLoader
    {
        private static readonly ClassLogger Logger = new(typeof(ExtentConfigurationLoader));

        private readonly IScopeStorage _scopeStorage;

        public ExtentConfigurationLoader(
            IScopeStorage scopeStorage,
            ExtentManager extentManager)
        {
            _scopeStorage = scopeStorage;
            ExtentManager = extentManager;
            ExtentStorageData = scopeStorage.Get<ExtentStorageData>();
        }

        /// <summary>
        /// Gets the information about the loaded extents,
        /// and filepath where to look after
        /// </summary>
        private ExtentStorageData ExtentStorageData { get; }

        /// <summary>
        /// Gets the extent manager being used to actual load an extent
        /// </summary>
        private ExtentManager ExtentManager { get; }

        /// <summary>
        /// Loads the configuration of the extents and returns the configuration
        /// </summary>
        /// <returns>A touple of the extentloader config element
        /// and the xml node to the metaclass</returns>
        public List<IElement> GetConfigurationFromFile()
        {
            var path = ExtentStorageData.FilePath;
            var loaded = new List<IElement>();
            if (!File.Exists(path))
            {
                Logger.Info($"File for Extent not found: {path}");

                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
            }
            else
            {
                Logger.Info($"Loading extent configuration from file: {path}");

                var document = XDocument.Load(path);
                var version = document.Root?.Attribute("Version")?.Value;
                if (string.IsNullOrEmpty(version))
                {
                    throw new InvalidOperationException(
                        $"Unfortunately, we have an old version and need to rebuild the extent configuration: {path}");
                }

                var xmiProvider = new XmiProvider(document);
                var extent = new MofUriExtent(xmiProvider, _scopeStorage)
                {
                    Workspace = ExtentManager.WorkspaceLogic.GetDataWorkspace()
                };

                foreach (var element in extent.elements().OfType<IElement>())
                {
                    loaded.Add(element);
                }
            }

            return loaded;
        }

        /// <summary>
        /// Stores the configuration of the extents into the given file
        /// </summary>
        public void StoreConfiguration()
        {
            // Skip saving, if loading has failed
            if (ExtentStorageData.FailedLoading)
            {
                Logger.Warn(
                    "No extents are stored due to the failure during loading. This prevents unwanted data loss due to a missing extent.");
                return;
            }

            var xmiProvider = new XmiProvider();
            var extentConfigurations = new MofUriExtent(xmiProvider, _scopeStorage);
            var factory = new MofFactory(extentConfigurations);

            var path = ExtentStorageData.FilePath;

            foreach (var loadingInformation in ExtentStorageData.LoadedExtents)
            {
                var copiedConfiguration = ObjectCopier.Copy(factory, loadingInformation.Configuration);
                extentConfigurations.elements().add(
                    copiedConfiguration
                    ?? throw new InvalidOperationException("Configuration is not set"));

                // Stores the .Net datatype to allow restore of the right element
                if (loadingInformation.Extent is MofExtent loadedExtent &&
                    !loadedExtent.Provider.GetCapabilities().StoreMetaDataInExtent)
                {
                    copiedConfiguration.set("metadata", loadedExtent.GetMetaObject());
                }
            }

            xmiProvider.Document.Root!.Add(new XAttribute("Version", "1.0"));
            xmiProvider.Document.Save(path);
        }
    }
}