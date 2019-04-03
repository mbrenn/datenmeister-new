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
                while (loaded.Count > 0)
                {
                    var tuple = loaded[0];
                    var (extentLoaderConfig, xElement) = tuple;
                    loaded.RemoveAt(0);

                    // Check, if given workspace can be loaded or whether references are still in list
                    if (IsMetaWorkspaceInLoaderList(extentLoaderConfig.workspaceId, loaded))
                    {
                        // If yes, put current workspace to the end
                        loaded.Add(tuple);
                    }
                    else
                    {
                        try
                        {
                            var extent = LoadExtent(extentLoaderConfig, false);
                            if (xElement != null)
                            {
                                ((MofExtent)extent).LocalMetaElementXmlNode = xElement;
                            }
                        }
                        catch (Exception exc)
                        {
                            Logger.Warn($"Loading extent of {extentLoaderConfig.extentUri} failed: {exc.Message}");
                            failedExtents.Add(extentLoaderConfig.extentUri);
                        }
                    }
                }

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
        /// Checks if a loading request is still open for one
        /// of the metaworkspaces of <c>workspaceId</c>. This ensures that
        /// the type definitions are loaded before the actual data
        /// </summary>
        /// <param name="workspaceId">Id of the workspace to be checked</param>
        /// <param name="loaded">List of items still to be loaded</param>
        /// <returns>True, if the loading should be inhibited because one
        /// of the metaitems are still in</returns>
        private bool IsMetaWorkspaceInLoaderList(string workspaceId, List<Tuple<ExtentLoaderConfig, XElement>> loaded)
        {
            return IsMetaWorkspaceInList(
                workspaceId,
                loaded.Select(x => x.Item1.workspaceId));
        }

        /// <summary>
        /// Gets or sets the value whether the metaworkspaces of workspaceid
        /// is found in the workspaceList
        /// </summary>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <param name="workspaceList">List of the workspaces which are checked
        /// against the metaworkspaces of workspace id</param>
        /// <returns>true, if the workspace is found</returns>
        private bool IsMetaWorkspaceInList(
            string workspaceId, 
            IEnumerable<string> workspaceList)
        {
            var workspace = _workspaceLogic.GetWorkspace(workspaceId);
            if (string.IsNullOrEmpty(workspaceId) || workspace == null)
            {
                // If workspace is not known, accept it
                return false;
            }
            
            var metaWorkspaces = workspace.MetaWorkspaces;
            foreach (var metaWorkspace in metaWorkspaces)
            {
                if (metaWorkspace.id == workspaceId)
                {
                    // Skip the self reference
                    continue;
                }

                if (workspaceList.Any(x => x == metaWorkspace.id))
                {
                    return true;
                }

                if (IsMetaWorkspaceInList(metaWorkspace.id, workspaceList))
                {
                    return true;
                }
            }

            return false;
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