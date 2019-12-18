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
    /// Defines the possible extent creation methods for the extent manager.
    /// Per default, the option load-only is chosen per default
    /// </summary>
    public enum ExtentCreationFlags
    {
        /// <summary>
        /// Tries to load the extent. If the extent cannot be loaded, the extent is NOT created
        /// </summary>
        LoadOnly,

        /// <summary>
        /// Tries to load the extent. If the extent cannot be loaded, a new extent is created
        /// </summary>
        LoadOrCreate,

        /// <summary>
        /// Creates a new extent by overwriting the existing one
        /// </summary>
        CreateOnly
    }

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

        /// <summary>
        /// Gets the workspace logic for the extent manager
        /// </summary>
        public IWorkspaceLogic WorkspaceLogic { get; }

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
            WorkspaceLogic = workspaceLogic ?? throw new ArgumentNullException(nameof(workspaceLogic));
            _integrationSettings = integrationSettings ?? throw new ArgumentNullException(nameof(integrationSettings));
            _diScope = diScope;
        }

        /// <summary>
        /// Loads the extent by using the extent storage by using the configuration and finding
        /// the correct storage engine
        /// </summary>
        /// <param name="configuration">Configuration being used to load</param>
        /// <param name="extentCreationFlags">The flags for the creation</param>
        /// <returns>The loaded extent</returns>
        public IUriExtent LoadExtent(ExtentLoaderConfig configuration, ExtentCreationFlags extentCreationFlags = ExtentCreationFlags.LoadOnly)
        {
            var (uriExtent, isAlreadyAdded) = LoadExtentWithoutAddingInternal(ref configuration, extentCreationFlags);
            if (isAlreadyAdded)
            {
                return uriExtent;
            }

            AddToWorkspaceIfPossible(configuration, uriExtent);

            // Stores the information into the data container
            var info = new ExtentStorageData.LoadedExtentInformation(uriExtent, configuration);

            lock (_extentStorageData.LoadedExtents)
            {
                _extentStorageData.LoadedExtents.Add(info);
            }
            
            VerifyDatabaseContent();

            return uriExtent;
        }

        /// <summary>
        /// Imports an extent without adding it intot he database.
        /// This is used to perform a temporary loading
        /// </summary>
        /// <param name="configuration">Configuration to be loaded</param>
        /// <returns>Resulting uri extent</returns>
        public IUriExtent LoadExtentWithoutAdding(ExtentLoaderConfig configuration) =>
            LoadExtentWithoutAddingInternal(ref configuration, ExtentCreationFlags.LoadOnly).Item1;

        private (IUriExtent, bool) LoadExtentWithoutAddingInternal(ref ExtentLoaderConfig configuration, ExtentCreationFlags extentCreationFlags)
        {
            // Checks, if the given URL has a relative path and transforms the path to an absolute path
            if (configuration is ExtentFileLoaderConfig fileConfiguration)
            {
                if (!Path.IsPathRooted(fileConfiguration.filePath))
                {
                    fileConfiguration.filePath = Path.Combine(_integrationSettings.DatabasePath, fileConfiguration.filePath);
                }

                if (Directory.Exists((fileConfiguration.filePath)))
                {
                    throw new InvalidOperationException("Given file is a directory name. ");
                }
            }
            
            // Check, if the extent url is a real uri
            if (!Uri.IsWellFormedUriString(configuration.extentUri, UriKind.Absolute))
            {
                throw new InvalidOperationException($"Uri of Extent is not well-formed: {configuration.extentUri}");
            }

            // Creates the extent loader, being capable to load or store an extent
            var extentLoader = _map.CreateFor(_diScope, configuration);

            // Loads the extent
            var loadedProviderInfo = extentLoader.LoadProvider(configuration, extentCreationFlags);

            // If the extent is already added (for example, the provider loader calls itself LoadExtent due to an indirection), then the resulting event extent will
            if (loadedProviderInfo.IsExtentAlreadyAddedToWorkspace)
            {
                return (
                    (IUriExtent) WorkspaceLogic.FindExtent(loadedProviderInfo.UsedConfig.workspaceId,
                        loadedProviderInfo.UsedConfig.extentUri),
                    true);
            }

            var loadedProvider = loadedProviderInfo.Provider;
            configuration =
                loadedProviderInfo.UsedConfig ?? configuration; // Updates the configuration, if it needs to be updated

            Logger.Info($"Loading extent: {configuration}");

            if (loadedProvider == null)
            {
                throw new InvalidOperationException("Extent for configuration could not be loaded");
            }

            var extent = new MofUriExtent(loadedProvider, configuration.extentUri);
            extent.SignalUpdateOfContent(false);
            
            VerifyDatabaseContent();
            
            return (extent, false);
        }

        /// <summary>
        /// Adds the given extent to the workspace according to the ExtentLoaderConfig. If the
        /// workspace does not exist, it will be created
        /// </summary>
        /// <param name="configuration">Configuration to be used</param>
        /// <param name="loadedExtent">The loaded extent to be added to the workpace</param>
        private void AddToWorkspaceIfPossible(ExtentLoaderConfig configuration, IUriExtent loadedExtent)
        {
            if (WorkspaceLogic != null)
            {
                var workspace = string.IsNullOrEmpty(configuration.workspaceId)
                    ? WorkspaceLogic.GetDefaultWorkspace()
                    : WorkspaceLogic.GetWorkspace(configuration.workspaceId);

                if (workspace == null)
                {
                    throw new InvalidOperationException($"Workspace {configuration.workspaceId} not found");
                }

                workspace.AddExtentNoDuplicate(WorkspaceLogic, loadedExtent);
            }
            
            VerifyDatabaseContent();
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
                information = _extentStorageData.LoadedExtents.FirstOrDefault(x => x.Extent.@equals(extent));
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
                var list = new List<Type>();

                // Captures the generalizations
                foreach (var type in _map.ConfigurationTypes)
                {
                    var baseType = type;
                    while (baseType != null && typeof(object) != baseType)
                    {
                        list.Add(baseType);
                        baseType = baseType.BaseType;
                    }
                }

                list = list.AsEnumerable().Reverse().Distinct().ToList();

                _diScope.Resolve<LocalTypeSupport>().AddInternalTypes(list, PackagePathTypesExtentLoaderConfig);
            }
        }

        /// <summary>
        /// Stores the extent according to the used configuration during loading.
        /// If loading was not performed, an exception is thrown.
        /// </summary>
        /// <param name="extent">Extent to be stored</param>
        public void StoreExtent(IExtent extent)
        {
            VerifyDatabaseContent();
            
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
            
            (extent as MofExtent)?.SignalUpdateOfContent(false);
            
            VerifyDatabaseContent();
        }

        /// <summary>
        /// Detaches the extent by removing it from the database of loaded extents
        /// </summary>
        /// <param name="extent"></param>
        public void DetachExtent(IExtent extent)
        {
            lock (_extentStorageData.LoadedExtents)
            {
                var information = _extentStorageData.LoadedExtents.FirstOrDefault(x => x.Extent.@equals(extent));
                if (information != null)
                {
                    _extentStorageData.LoadedExtents.Remove(information);
                    Logger.Info($"Detaching extent: {information.Configuration}");
                }
            }

            VerifyDatabaseContent();
        }

        /// <summary>
        /// Deletes the extent from the extent manager but also from the internal database of loading information.
        /// </summary>
        /// <param name="extent">Extent to be removed</param>
        public void DeleteExtent(IExtent extent)
        {
            lock (_extentStorageData.LoadedExtents)
            {
                var workspace = WorkspaceLogic.GetWorkspaceOfExtent(extent);

                // Removes the loading information of the extent
                DetachExtent(extent);

                // Removes the extent from the workspace
                workspace.RemoveExtent(extent);

                WorkspaceLogic.SendEventForWorkspaceChange(workspace);
            }
            
            VerifyDatabaseContent();
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
                            var extent = LoadExtent(extentLoaderConfig);
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
                }

                foreach (var (extentLoaderConfig, xElement) in loaded)
                {
                    try
                    {
                        var extent = LoadExtent(extentLoaderConfig);
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
            
            VerifyDatabaseContent();
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
                loaded.Select(x => x.Item1.workspaceId)
                    .ToArray());
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
            ICollection<string> workspaceList)
        {
            var workspace = WorkspaceLogic.GetWorkspace(workspaceId);
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
            VerifyDatabaseContent();
                
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

        /// <summary>
        /// Verifies the content of the database
        /// </summary>
        private void VerifyDatabaseContent()
        {
            lock (_extentStorageData)
            {
                var list = new List<VerifyDatabaseEntry>();
                foreach (var entry in _extentStorageData.LoadedExtents)
                {
                    var found = list.FirstOrDefault(
                        x => x.Workspace == entry.Configuration.workspaceId
                             && x.ExtentUri == entry.Configuration.extentUri);

                    if (found != null)
                    {
                        throw new InvalidOperationException("Database integrity is not given anymore");
                    }
                    
                    list.Add(new VerifyDatabaseEntry
                    {
                        Workspace = entry.Configuration.workspaceId,
                        ExtentUri = entry.Configuration.extentUri
                    });
                }
            }
        }

        private class VerifyDatabaseEntry
        {
            public string Workspace { get; set; }
            
            public string ExtentUri { get; set; }
        }
    }
}