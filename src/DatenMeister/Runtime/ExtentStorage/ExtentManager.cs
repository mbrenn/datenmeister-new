using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Autofac;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Integration;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Runtime.ExtentStorage
{
    /// <summary>
    /// This logic handles the loading and storing of extents automatically. 
    /// This loader is responsible to retrieve an extent by the given ExtentLoaderConfig
    /// and storing it afterwards at the same location
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ExtentManager : IExtentManager
    {
        public const string PackagePathTypesExtentLoaderConfig = "DatenMeister::ExtentLoaderConfig";

        private readonly ExtentStorageData _extentStorageData;

        private static readonly ClassLogger Logger = new ClassLogger(typeof(ExtentManager));

        /// <summary>
        /// Stores the mapping between configuration types and storage provider
        /// </summary>
        private readonly IConfigurationToExtentStorageMapper _map;

        private readonly ILifetimeScope _diScope;

        private readonly IWorkspaceLogic _workspaceLogic;

        private readonly IntegrationSettings _integrationSettings;

        public ExtentManager(
            ExtentStorageData data, 
            IConfigurationToExtentStorageMapper map,
            ILifetimeScope diScope, 
            IWorkspaceLogic workspaceLogic,
            IntegrationSettings integrationSettings)
        {
            _extentStorageData = data ?? throw new ArgumentNullException(nameof(data));
            _map = map ?? throw new ArgumentNullException(nameof(map));
            _workspaceLogic = workspaceLogic ?? throw new ArgumentNullException(nameof(workspaceLogic));
            _integrationSettings = integrationSettings ?? throw new ArgumentNullException(nameof(integrationSettings));
            _diScope = diScope;
        }

        /// <summary>
        /// Loads the extent by using the extent storage by using the configuration and finding
        /// the correct storage engine 
        /// </summary>
        /// <param name="configuration">Configuration being used to load</param>
        /// <param name="createAlsoEmpty">true, if also empty extents will be created, if the file does not exist</param>
        /// <returns>The loaded extent</returns>
        public IUriExtent LoadExtent(ExtentLoaderConfig configuration, bool createAlsoEmpty = false)
        {
            // Check, if the extent url is a real uri
            if (!Uri.IsWellFormedUriString(configuration.extentUri, UriKind.Absolute))
            {
                throw new InvalidOperationException($"Uri of Extent is not well-formed: {configuration.extentUri}");
            }

            // Checks, if the given URL has a relative path and transforms the path to an absolute path
            if (configuration is ExtentFileLoaderConfig fileConfiguration)
            {
                if (!Path.IsPathRooted(fileConfiguration.filePath))
                {
                    fileConfiguration.filePath = Path.Combine(_integrationSettings.DatabasePath, fileConfiguration.filePath);
                }
            }

            // Creates the extent loader, being capable to load or store an extent
            var extentLoader = _map.CreateFor(_diScope, configuration);
        
            // Loads the extent
            var loadedProviderInfo = extentLoader.LoadProvider(configuration, createAlsoEmpty);

            // If the extent is already added (for example, the provider loader calls itself LoadExtent due to an indirection), then the resulting event extent will 
            if (loadedProviderInfo.IsExtentAlreadyAddedToWorkspace)
            {
                return (IUriExtent) _workspaceLogic.FindExtent(loadedProviderInfo.UsedConfig.workspaceId, loadedProviderInfo.UsedConfig.extentUri);
            }

            var loadedProvider = loadedProviderInfo.Provider;
            configuration = loadedProviderInfo.UsedConfig ?? configuration; // Updates the configuration, if it needs to be updated

            Logger.Info($"Loading extent: {configuration}");

            if (loadedProvider == null)
            {
                throw new InvalidOperationException("Extent for configuration could not be loaded");
            }

            var uriExtent = new MofUriExtent(loadedProvider, configuration.extentUri);
            AddToWorkspaceIfPossible(configuration, uriExtent);

            // Stores the information into the data container
            var info = new ExtentStorageData.LoadedExtentInformation
            {
                Configuration = configuration,
                Extent = uriExtent
            };

            lock (_extentStorageData.LoadedExtents)
            {
                _extentStorageData.LoadedExtents.Add(info);
            }

            return uriExtent;
        }

        /// <summary>
        /// Adds the given extent to the workspace according to the ExtentLoaderConfig. If the
        /// workspace does not exist, it will be created
        /// </summary>
        /// <param name="configuration">Configuration to be used</param>
        /// <param name="loadedExtent">The loaded extent to be added to the workpace</param>
        private void AddToWorkspaceIfPossible(ExtentLoaderConfig configuration, IUriExtent loadedExtent)
        {
            if (_workspaceLogic != null)
            {
                var workspace = string.IsNullOrEmpty(configuration.workspaceId)
                    ? _workspaceLogic.GetDefaultWorkspace()
                    : _workspaceLogic.GetWorkspace(configuration.workspaceId);

                if (workspace == null)
                {
                    throw new InvalidOperationException($"Workspace {configuration.workspaceId} not found");
                }

                workspace.AddExtentNoDuplicate(_workspaceLogic, loadedExtent);
            }
        }

        /// <summary>
        /// Gets the loading configuration for the given extent or null, if 
        /// the extent does not contain a configuration
        /// </summary>
        /// <param name="extent">The extent whose configuration is retrieved</param>
        /// <returns>The configuration</returns>
        public ExtentLoaderConfig GetLoadConfigurationFor(IUriExtent extent)
        {
            ExtentStorageData.LoadedExtentInformation information;
            lock (_extentStorageData.LoadedExtents)
            {
                information = _extentStorageData.LoadedExtents.FirstOrDefault(x => x.Extent == extent);
            }

            return information?.Configuration;
        }

        /// <summary>
        /// Creates the storage type definitions
        /// </summary>
        public void CreateStorageTypeDefinitions()
        {
            if (_diScope == null) throw new InvalidOperationException("diScope == null");

            lock (_extentStorageData.LoadedExtents)
            {
                _diScope.Resolve<LocalTypeSupport>().AddInternalTypes(_map.ConfigurationTypes, PackagePathTypesExtentLoaderConfig);
            }
        }

        /// <summary>
        /// Stores the extent according to the used configuration during loading. 
        /// If loading was not performed, an exception is thrown. 
        /// </summary>
        /// <param name="extent">Extent to be stored</param>
        public void StoreExtent(IExtent extent)
        {
            ExtentStorageData.LoadedExtentInformation information;
            lock (_extentStorageData.LoadedExtents)
            {
                information = _extentStorageData.LoadedExtents.FirstOrDefault(x => x.Extent == extent);
            }

            if (information == null)
            {
                throw new InvalidOperationException($"The extent '{extent}' was not loaded by this instance");
            }

            Logger.Info($"Writing extent: {information.Configuration}");

            var extentStorage = _map.CreateFor(_diScope, information.Configuration);
            extentStorage.StoreProvider(((MofUriExtent) information.Extent).Provider, information.Configuration);
        }

        /// <summary>
        /// Detaches the extent by removing it from the database of loaded extents
        /// </summary>
        /// <param name="extent"></param>
        public void DetachExtent(IExtent extent)
        {
            lock (_extentStorageData.LoadedExtents)
            {
                var information = _extentStorageData.LoadedExtents.FirstOrDefault(x => x.Extent == extent);
                if (information != null)
                {
                    _extentStorageData.LoadedExtents.Remove(information);
                    Logger.Info($"Detaching extent: {information.Configuration}");
                }
            }
        }

        /// <summary>
        /// Deletes the extent from the extent manager but also from the internal database of loading information. 
        /// </summary>
        /// <param name="extent">Extent to be removed</param>
        public void DeleteExtent(IExtent extent)
        {
            lock (_extentStorageData.LoadedExtents)
            {
                var workspace = _workspaceLogic.GetWorkspaceOfExtent(extent);

                // Removes the loading information of the extent
                DetachExtent(extent);
                
                // Removes the extent from the workspace
                workspace.RemoveExtent(extent);

                _workspaceLogic.SendEventForWorkspaceChange(workspace);
            }
        }


        /// <summary>
        /// Loads all extents
        /// </summary>
        public void LoadAllExtents()
        {
            lock (_extentStorageData.LoadedExtents)
            { 
                var configurationLoader = new ExtentConfigurationLoader(_extentStorageData, this, _map);
                List<Tuple<ExtentLoaderConfig, XElement>> loaded = null;
                try
                {
                    loaded = configurationLoader.GetConfigurationFromFile();

                }
                catch (Exception exc)
                {
                    Logger.Warn("Exception during loading of Extents: " + exc.Message);
                }

                if (loaded == null)
                {
                    return;
                }

                var failedExtents = new List<string>();
                foreach (var (extentLoaderConfig, xElement) in loaded)
                {
                    try
                    {
                        var extent = LoadExtent(extentLoaderConfig, false);
                        if (xElement != null)
                        {
                            ((MofExtent) extent).LocalMetaElementXmlNode = xElement;
                        }
                    }
                    catch (Exception exc)
                    {
                        Logger.Warn($"Loading extent of {extentLoaderConfig.extentUri} failed: {exc.Message}");
                        failedExtents.Add(extentLoaderConfig.extentUri);
                    }
                }

                // If one of the extents failed, the exception will be thrown
                if (failedExtents.Count > 0 || _extentStorageData.FailedLoading)
                {
                    Logger.Warn("Storing of extents is disabled due to failed loading");
                    _extentStorageData.FailedLoading = true;
                    throw new LoadingExtentsFailedException(failedExtents);
                }
            }
        }

        /// <summary>
        /// Stores all extents 
        /// </summary>
        public void StoreAllExtents()
        {
            List<ExtentStorageData.LoadedExtentInformation> copy;

            lock (_extentStorageData.LoadedExtents)
            {
                if (_extentStorageData.FailedLoading)
                {
                    Logger.Warn("Storing of extents is disabled due to failed loading");
                    return;
                }

                Logger.Info("Writing all extents");
                copy = _extentStorageData.LoadedExtents.ToList();
            }

            foreach (var info in copy)
            {
                try
                {
                    StoreExtent(info.Extent);
                }
                catch (Exception exc)
                {
                    Logger.Error($"Error during storing of extent: {info.Configuration.extentUri}: {exc.Message}");
                }
            }
        }
    }
}