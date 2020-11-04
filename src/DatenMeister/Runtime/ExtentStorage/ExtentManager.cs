using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Provider;
using DatenMeister.Provider.XMI.EMOF;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Locking;
using DatenMeister.Runtime.Workspaces;
using static DatenMeister.Models._DatenMeister._ExtentLoaderConfigs;

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
    public class ExtentManager
    {
        public const string PackagePathTypesExtentLoaderConfig = "DatenMeister::ExtentLoaderConfigs";

        private readonly ExtentStorageData _extentStorageData;

        private static readonly ClassLogger Logger = new ClassLogger(typeof(ExtentManager));

        /// <summary>
        /// Stores the mapping between configuration types and storage provider
        /// </summary>
        private readonly ConfigurationToExtentStorageMapper _map;

        public IScopeStorage ScopeStorage { get; }

        /// <summary>
        /// Gets the workspace logic for the extent manager
        /// </summary>
        public IWorkspaceLogic WorkspaceLogic { get; }

        private readonly IntegrationSettings _integrationSettings;
        
        /// <summary>
        /// Stores the locking logic. May be null, if no locking is active. 
        /// </summary>
        private readonly LockingLogic? _lockingHandler;

        public ExtentManager(
            IWorkspaceLogic workspaceLogic,
            IScopeStorage scopeStorage)
        {
            ScopeStorage = scopeStorage ?? throw new ArgumentNullException(nameof(scopeStorage));
            _extentStorageData = scopeStorage.Get<ExtentStorageData>() ??
                                 throw new InvalidOperationException("Extent Storage Data not found");
            _integrationSettings = scopeStorage.Get<IntegrationSettings>() ??
                                   throw new InvalidOperationException("IntegrationSettings not found");
            _lockingHandler = _integrationSettings.IsLockingActivated ? new LockingLogic(scopeStorage) : null;

            WorkspaceLogic = workspaceLogic ?? throw new ArgumentNullException(nameof(workspaceLogic));
            _map = ScopeStorage.Get<ConfigurationToExtentStorageMapper>();
        }

        public ExtentStorageData.LoadedExtentInformation LoadExtent(IElement configuration,
            ExtentCreationFlags extentCreationFlags = ExtentCreationFlags.LoadOnly)
        {
            // First, check, if there is already an extent loaded in the internal database with that uri and workspace
            var workspaceId =
                configuration.getOrDefault<string>(_ExtentLoaderConfig.workspaceId);
            var extentUri =
                configuration.getOrDefault<string>(_ExtentLoaderConfig.extentUri);
            
            lock (_extentStorageData.LoadedExtents)
            {
                if (_extentStorageData.LoadedExtents.Any(
                    x => workspaceId == x.Configuration.getOrDefault<string>(_ExtentLoaderConfig.workspaceId)
                         && extentUri == x.Configuration.getOrDefault<string>(_ExtentLoaderConfig.extentUri)))
                {
                    throw new InvalidOperationException(
                        $"There is already the extent loaded with extentUri: {extentUri}");
                }
            }

            var loadedExtentInformation = LoadExtentWithoutAddingInternal(configuration, extentCreationFlags);
            configuration = loadedExtentInformation.Configuration;
            var uriExtent = loadedExtentInformation.Extent;
            if (loadedExtentInformation.IsExtentAddedToWorkspace)
            {
                return loadedExtentInformation;
            }

            // Stores the information into the data container
            lock (_extentStorageData.LoadedExtents)
            {
                _extentStorageData.LoadedExtents.Add(loadedExtentInformation);
            }

            VerifyDatabaseContent();

            // Now adds the extent to the workspace if required
            if (uriExtent != null)
            {
                AddToWorkspaceIfPossible(configuration, uriExtent, true);
                loadedExtentInformation.IsExtentAddedToWorkspace = true;
            }

            return loadedExtentInformation;
        }

        /// <summary>
        /// Imports an extent without adding it into the database.
        /// This is used to perform a temporary loading
        /// </summary>
        /// <param name="configuration">Configuration to be loaded</param>
        /// <returns>Resulting uri extent</returns>
        public ExtentStorageData.LoadedExtentInformation? LoadExtentWithoutAdding(IElement configuration) =>
            LoadExtentWithoutAddingInternal(configuration, ExtentCreationFlags.LoadOnly);

        /// <summary>
        /// Loads the extent according given configuration and returns the information dataset
        /// describing the used loaded configuration
        /// </summary>
        /// <param name="configuration">Configuration being used for the loading</param>
        /// <param name="extentCreationFlags">The flags describing the loading profile</param>
        /// <returns>The configuration information</returns>
        private ExtentStorageData.LoadedExtentInformation LoadExtentWithoutAddingInternal(
            IElement configuration,
            ExtentCreationFlags extentCreationFlags)
        {
            var extentUri =
                configuration.getOrDefault<string>(_ExtentLoaderConfig.extentUri);
            if (extentUri == null || string.IsNullOrEmpty(extentUri))
            {
                throw new InvalidOperationException("No extent uri is given");
            }
            
            var loadedExtentInformation = new ExtentStorageData.LoadedExtentInformation(configuration)
            {
                LoadingState = ExtentLoadingState.Unloaded
            };

            // Checks, if the given URL has a relative path and transforms the path to an absolute path
            // TODO: Do real check including generalizations, but accept it for now
            if (configuration.isSet(_ExtentFileLoaderConfig.filePath))
            {
                var filePath =
                    configuration.getOrDefault<string>(_ExtentFileLoaderConfig.filePath);
                filePath = _integrationSettings.NormalizeDirectoryPath(filePath);
                configuration.set(_ExtentFileLoaderConfig.filePath, filePath);

                if (Directory.Exists(filePath))
                {
                    throw new InvalidOperationException("Given file is a directory name. ");
                }
            }
            
            // Check, if the extent url is a real uri
            if (!Uri.IsWellFormedUriString(extentUri, UriKind.Absolute))
            {
                throw new InvalidOperationException($"Uri of Extent is not well-formed: {extentUri}");
            }

            var extentLoader = CreateProviderLoader(configuration);

            // Ok, now we have the provider. If the provider also supports the locking, check whether it can be locked
            if (extentLoader is IProviderLocking providerLocking && _integrationSettings.IsLockingActivated)
            {
                if (providerLocking.IsLocked(configuration))
                {
                    var filePath =
                        configuration.getOrDefault<string>(_ExtentFileLoaderConfig
                            .filePath) ?? "Unknown";

                    loadedExtentInformation.LoadingState = ExtentLoadingState.Failed;
                    loadedExtentInformation.FailLoadingMessage = $"The provider is locked: {filePath}";
                    return loadedExtentInformation;
                }

                providerLocking.Lock(configuration);
            }

            // Loads the extent
            LoadedProviderInfo loadedProviderInfo;
            try
            {
                loadedProviderInfo = extentLoader.LoadProvider(configuration, extentCreationFlags);
                loadedExtentInformation.LoadingState =
                    _integrationSettings.IsReadOnly
                        ? ExtentLoadingState.LoadedReadOnly
                        : ExtentLoadingState.Loaded;
            }
            catch (Exception e)
            {
                loadedExtentInformation.LoadingState = ExtentLoadingState.Failed;
                loadedExtentInformation.FailLoadingMessage = e.ToString();
                return loadedExtentInformation; 
            }

            // If the extent is already added (for example, the provider loader calls itself LoadExtent due to an indirection), then the resulting event extent will
            if (loadedProviderInfo.IsExtentAlreadyAddedToWorkspace || loadedExtentInformation.IsExtentAddedToWorkspace)
            {
                var newWorkspaceId =
                    loadedProviderInfo.UsedConfig?.getOrDefault<string>(_ExtentLoaderConfig.workspaceId) ?? string.Empty;
                var newExtentUri =
                    loadedProviderInfo.UsedConfig?.getOrDefault<string>(_ExtentLoaderConfig.extentUri) ?? string.Empty;
                var alreadyFoundExtent = (IUriExtent?) WorkspaceLogic.FindExtent(
                    newWorkspaceId,
                    newExtentUri);
                if (alreadyFoundExtent == null)
                {
                    throw new InvalidOperationException("The extent was not found: " +
                                                        newExtentUri);
                }

                loadedExtentInformation.Extent = alreadyFoundExtent;
                loadedExtentInformation.IsExtentAddedToWorkspace = true;
                VerifyDatabaseContent();

                return loadedExtentInformation;
            }

            var loadedProvider = loadedProviderInfo.Provider;

            // Updates the configuration, if it needs to be updated
            // The update can happen, when the Provider Loader just used the initial configuration to 
            // store the permanent one in another database location. 
            loadedExtentInformation.Configuration = loadedProviderInfo.UsedConfig ?? configuration;
            VerifyDatabaseContent();

            Logger.Info($"Loaded extent: {loadedExtentInformation.Configuration}");

            if (loadedProvider == null)
            {
                throw new InvalidOperationException("Extent for configuration could not be loaded");
            }

            var mofUriExtent = new MofUriExtent(loadedProvider, extentUri);
            loadedExtentInformation.Extent = mofUriExtent;
            mofUriExtent.SignalUpdateOfContent(false);

            VerifyDatabaseContent();

            return loadedExtentInformation;
        }

        private IProviderLoader CreateProviderLoader(IElement configuration)
        {
            // Creates the extent loader, being capable to load or store an extent
            var extentLoader = _map.CreateFor(this, configuration);
            return extentLoader;
        }

        /// <summary>
        /// Adds the given extent to the workspace according to the ExtentLoaderConfig. If the
        /// workspace does not exist, it will be created
        /// </summary>
        /// <param name="configuration">Configuration to be used</param>
        /// <param name="loadedExtent">The loaded extent to be added to the workpace</param>
        /// <param name="createEmptyWorkspace">true, if an empty workspace shall be created in case the workspace
        /// does not exist. </param>
        private void AddToWorkspaceIfPossible(IElement configuration, IUriExtent loadedExtent, bool createEmptyWorkspace)
        {
            var workspaceId =
                configuration.getOrDefault<string>(_ExtentLoaderConfig.workspaceId);
            if (WorkspaceLogic != null)
            {
                var workspace = string.IsNullOrEmpty(workspaceId)
                    ? WorkspaceLogic.GetDefaultWorkspace()
                    : WorkspaceLogic.GetWorkspace(workspaceId);

                if (workspace == null)
                {
                    if (createEmptyWorkspace)
                    {
                        WorkspaceLogic.AddWorkspace(
                            workspace = new Workspace(workspaceId, string.Empty));
                    }
                    else
                    {
                        throw new InvalidOperationException($"Workspace {workspaceId} not found");
                    }
                }

                workspace.AddExtentNoDuplicate(WorkspaceLogic, loadedExtent);
            }
            
            VerifyDatabaseContent();
        }

        /// <summary>
        /// Gets the loaded extent information for the given extent
        /// </summary>
        /// <param name="extent">Extent to be queried</param>
        /// <returns>The loaded extent information</returns>
        public ExtentStorageData.LoadedExtentInformation? GetLoadedExtentInformation(IUriExtent extent)
        {
            ExtentStorageData.LoadedExtentInformation? information;
            lock (_extentStorageData.LoadedExtents)
            {
                information = _extentStorageData.LoadedExtents.FirstOrDefault(x =>
                    (x.LoadingState == ExtentLoadingState.Loaded ||
                        x.LoadingState == ExtentLoadingState.LoadedReadOnly)
                    && x.Extent?.@equals(extent) == true);
            }

            return information;
        }

        /// <summary>
        /// Gets an enumeration of the loaded extent information for a certain workspace
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <returns></returns>
        public IEnumerable<ExtentStorageData.LoadedExtentInformation> GetLoadedExtentInformationForWorkspace(
            string workspaceId)
        {
            List<ExtentStorageData.LoadedExtentInformation> list = new List<ExtentStorageData.LoadedExtentInformation>();
            lock (_extentStorageData.LoadedExtents)
            {
                list.AddRange(
                    _extentStorageData.LoadedExtents
                        .Where(loadedExtent =>
                            loadedExtent.Configuration.getOrDefault<string>(_ExtentLoaderConfig.workspaceId) == workspaceId));
            }

            return list;
        }

        /// <summary>
        /// Gets the loading configuration for the given extent or null, if
        /// the extent does not contain a configuration
        /// </summary>
        /// <param name="extent">The extent whose configuration is retrieved</param>
        /// <returns>The configuration</returns>
        public IElement? GetLoadConfigurationFor(IUriExtent extent)
        {
            ExtentStorageData.LoadedExtentInformation? information;
            lock (_extentStorageData.LoadedExtents)
            {
                information = _extentStorageData.LoadedExtents.FirstOrDefault(x =>
                    x.LoadingState == ExtentLoadingState.Loaded &&
                    x.Extent?.@equals(extent) == true);
            }

            return information?.Configuration;
        }

        /// <summary>
        /// Stores the extent according to the used configuration during loading.
        /// If loading was not performed, an exception is thrown.
        /// </summary>
        /// <param name="extent">Extent to be stored</param>
        public void StoreExtent(IExtent extent)
        {
            if (_integrationSettings.IsReadOnly)
            {
                Logger.Info("ExtentManager is read-only. We do not store");
                return;
            }
            
            VerifyDatabaseContent();
            CheckForOpenedManager();

            ExtentStorageData.LoadedExtentInformation? information;
            lock (_extentStorageData.LoadedExtents)
            {
                information = _extentStorageData.LoadedExtents.FirstOrDefault(x =>
                    x.LoadingState == ExtentLoadingState.Loaded && x.Extent == extent);
            }

            if (information == null)
            {
                throw new InvalidOperationException($"The extent '{extent}' was not loaded by this instance");
            }

            Logger.Info($"Writing extent: {information.Configuration}");

            var extentStorage = _map.CreateFor(this, information.Configuration);
            extentStorage.StoreProvider(
                (information.Extent as MofUriExtent ?? throw new InvalidOperationException("Extent was not set")).Provider,
                information.Configuration);

            (extent as MofExtent)?.SignalUpdateOfContent(false);

            VerifyDatabaseContent();
        }

        /// <summary>
        /// Unlocks the defined extent
        /// </summary>
        /// <param name="configuration">Configuration to be evaluated</param>
        public void UnlockProvider(IElement configuration)
        {
            var providerLoader = CreateProviderLoader(configuration);
            if (providerLoader is IProviderLocking providerLocking && _integrationSettings.IsLockingActivated)
            {
                providerLocking.Unlock(configuration);
            }
        }

        /// <summary>
        /// Detaches the extent by removing it from the database of loaded extents.
        /// The extent will also be unlocked. This method does not remove the extent from
        /// the workspaces.
        /// </summary>
        /// <param name="extent"></param>
        /// <param name="doStore">true, if the values shall be stored into the database</param>
        public void DetachExtent(IExtent extent, bool doStore = false)
        {
            lock (_extentStorageData.LoadedExtents)
            {
                CheckForOpenedManager();
                
                if (doStore) StoreExtent(extent);
                
                var information = _extentStorageData.LoadedExtents.FirstOrDefault(x => 
                    x.LoadingState == ExtentLoadingState.Loaded && x.Extent?.@equals(extent) == true);
                if (information != null)
                {
                    _extentStorageData.LoadedExtents.Remove(information);
                    Logger.Info($"Detaching extent: {information.Configuration}");

                    var configuration = information.Configuration;
                    UnlockProvider(configuration);
                }
            }

            VerifyDatabaseContent();
        }

        /// <summary>
        /// Removes the extent by specifying workspace id and uri extent.
        /// The extent will be completely removed from database including WorkspaceLogic
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="extentUri"></param>
        public bool RemoveExtent(string workspaceId, string extentUri)
        {
            var extent = WorkspaceLogic.FindExtent(workspaceId, extentUri);
            if (extent != null)
            {
                RemoveExtent(extent);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Deletes the extent from the extent manager but also from the internal database of loading information.
        /// </summary>
        /// <param name="extent">Extent to be removed</param>
        public void RemoveExtent(IExtent extent)
        {
            lock (_extentStorageData.LoadedExtents)
            {
                var workspace = WorkspaceLogic.GetWorkspaceOfExtent(extent);
                if (workspace == null) return;

                // Removes the loading information of the extent
                DetachExtent(extent);

                // Removes the extent from the workspace
                workspace.RemoveExtent(extent);

                WorkspaceLogic.SendEventForWorkspaceChange(workspace);
            }
            
            VerifyDatabaseContent();
        }

        /// <summary>
        /// Takes the loaded extent information and removes the extent from the workspaces but
        /// also removes the loaded information from the internal extent registration
        /// </summary>
        /// <param name="information">Information to the loaded extents</param>
        public void RemoveExtent(ExtentStorageData.LoadedExtentInformation information)
        {
            lock (_extentStorageData.LoadedExtents)
            {
                var extent = information.Extent;
                if (extent != null)
                {
                    var workspace = WorkspaceLogic.GetWorkspaceOfExtent(extent);
                    if (workspace != null)
                    {
                        // Removes the extent from the workspace
                        workspace.RemoveExtent(extent);
                        WorkspaceLogic.SendEventForWorkspaceChange(workspace);
                    }
                }

                // Removes the loading information of the extent
                _extentStorageData.LoadedExtents.Remove(information);
                VerifyDatabaseContent();
            }
        }

        /// <summary>
        /// Just opens an empty extent manager without having a connection to a extent registration storage date
        /// </summary>
        public void OpenDecoupled()
        {
            lock (_extentStorageData.LoadedExtents)
            {
                // Checks, if loading has already occurred
                if (_extentStorageData.IsOpened)
                {
                    Logger.Warn("The Extent Storage was already opened...");
                }

                _extentStorageData.IsOpened = true;
                _extentStorageData.IsRegistrationOpen = false;
            }
        }

        /// <summary>
        /// Loads all extents and evaluates the extent manager as having loaded the extents
        /// </summary>
        public void LoadAllExtents()
        {
            lock (_extentStorageData.LoadedExtents)
            {
                // Checks, if loading has already occured
                if (_extentStorageData.IsOpened)
                {
                    Logger.Warn("The Extent Storage was already opened...");
                }
                else
                {
                    _lockingHandler?.Lock(_extentStorageData.GetLockPath());
                }

                var failedExtents = new List<string>();
                _extentStorageData.IsOpened = true;
                _extentStorageData.IsRegistrationOpen = true;

                // Stores the last the exception
                Exception? lastException = null;

                var configurationLoader = new ExtentConfigurationLoader(ScopeStorage, this);
                List<IElement>? loaded = null;
                try
                {
                    loaded = configurationLoader.GetConfigurationFromFile();
                }
                catch (Exception exc)
                {
                    Logger.Fatal("Exception during loading of Extents: " + exc.Message);

                    failedExtents.Add("Extent Configuration Database");
                    _extentStorageData.FailedLoading = true;
                    _extentStorageData.FailedLoadingException = exc;
                    _extentStorageData.FailedLoadingExtents = failedExtents;

                    throw new LoadingExtentsFailedException(failedExtents);
                }

                if (loaded == null)
                {
                    return;
                }
                while (loaded.Count > 0)
                {
                    var configuration = loaded[0];
                    loaded.RemoveAt(0);

                    // Check, if given workspace can be loaded or whether references are still in list
                    if (IsMetaWorkspaceInLoaderList(
                        configuration.getOrDefault<string>(_ExtentLoaderConfig
                            .workspaceId), loaded))
                    {
                        // If yes, put current workspace to the end
                        loaded.Add(configuration);
                    }
                    else
                    {
                        try
                        {
                            var extent = LoadExtent(configuration);

                            var element =
                                ((configuration.getOrDefault<IElement>("metadata") as MofElement)?.ProviderObject as
                                    XmiProviderObject)?.XmlNode;

                            if (element != null && extent.Extent != null)
                            {
                                ((MofExtent) extent.Extent).LocalMetaElementXmlNode = element;
                            }
                        }
                        catch (Exception exc)
                        {
                            Logger.Warn(
                                $"Loading extent of " +
                                $"{configuration.getOrDefault<string>(_ExtentLoaderConfig.extentUri)} " +
                                $"failed: {exc.Message}");
                            failedExtents.Add(configuration.getOrDefault<string>(_ExtentLoaderConfig.extentUri));
                            lastException = exc;
                        }
                    }
                }

                foreach (var configuration in loaded)
                {
                    try
                    {
                        var extent = LoadExtent(configuration);

                        var element =
                            ((configuration.getOrDefault<IElement>("metadata") as MofElement)?.ProviderObject as
                                XmiProviderObject)?.XmlNode;

                        if (element != null && extent.Extent != null)
                        {
                            ((MofExtent) extent.Extent).LocalMetaElementXmlNode = element;
                        }
                    }
                    catch (Exception exc)
                    {
                        Logger.Warn(
                            $"Loading extent of {configuration.getOrDefault<string>(_ExtentLoaderConfig.extentUri)} failed: {exc.Message}");
                        failedExtents.Add(configuration.getOrDefault<string>(_ExtentLoaderConfig.extentUri));
                        lastException = exc;
                    }
                }

                // If one of the extents failed, the exception will be thrown
                if (failedExtents.Count > 0 || _extentStorageData.FailedLoading)
                {
                    Logger.Warn("Storing of extents is disabled due to failed loading");
                    _extentStorageData.FailedLoading = true;
                    _extentStorageData.FailedLoadingException = lastException;
                    _extentStorageData.FailedLoadingExtents = failedExtents;

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
        /// <param name="configuration">Configuration being used</param>
        /// <returns>True, if the loading should be inhibited because one
        /// of the metaitems are still in</returns>
        private bool IsMetaWorkspaceInLoaderList(string workspaceId, IEnumerable<IElement> configuration)
        {
            return IsMetaWorkspaceInList(
                workspaceId,
                configuration.Select(x => x.getOrDefault<string>(_ExtentLoaderConfig.workspaceId))
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
            if (_integrationSettings.IsReadOnly)
            {
                Logger.Info("ExtentManager is read-only. We do not store");
                return;
            }
            
            VerifyDatabaseContent();
            CheckForOpenedManager();
                
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
                    if (info.LoadingState == ExtentLoadingState.Loaded && info.Extent != null)
                    {
                        StoreExtent(info.Extent);
                    }
                }
                catch (Exception exc)
                {
                    Logger.Error(
                        $"Error during storing of extent: {info.Configuration.getOrDefault<string>(_ExtentLoaderConfig.extentUri)}: {exc.Message}");
                }
            }
        }

        /// <summary>
        /// Unloads all extents from the internal registration and releases the lock to the extent files and
        /// extent registration
        /// </summary>
        /// <param name="doStore">true, if all extents shall be stored</param>
        public void UnloadManager(bool doStore = false)
        {
            lock (_extentStorageData.LoadedExtents)
            {
                if (!CheckForOpenedManager())
                {
                    Logger.Info("Unload Manager was called when extent manager is not laoded");
                    return;
                }

                if (doStore && !_integrationSettings.IsReadOnly)
                {
                    StoreAllExtents();
                }

                Logger.Info("Unloading and unlocking");
                var copy = _extentStorageData.LoadedExtents.ToList();

                foreach (var info in copy)
                {
                    UnlockProvider(info.Configuration);
                }

                if (_extentStorageData.IsRegistrationOpen)
                {
                    if (!_integrationSettings.IsReadOnly)
                    {
                        var configurationLoader = new ExtentConfigurationLoader(ScopeStorage, this);
                        configurationLoader.StoreConfiguration();
                    }

                    _extentStorageData.LoadedExtents.Clear();

                    if (_extentStorageData.IsOpened)
                    {
                        _lockingHandler?.Unlock(_extentStorageData.GetLockPath());
                        _extentStorageData.IsOpened = false;
                    }
                }
            }
        }

        private bool CheckForOpenedManager()
        {
            lock (_extentStorageData.LoadedExtents)
            {
                if (!_extentStorageData.IsOpened)
                {
                    Logger.Warn("The manager is not opened, but we do it eitherways");
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Verifies the content of the database.
        /// The check evaluates whether two loaded extents map to the same extentUri within the configuration
        /// </summary>
        private void VerifyDatabaseContent()
        {
            lock (_extentStorageData)
            {
                var list = new List<VerifyDatabaseEntry>();
                foreach (var entry in _extentStorageData.LoadedExtents)
                {
                    var found = list.FirstOrDefault(
                        x => x.Workspace ==
                             entry.Configuration.getOrDefault<string>(_ExtentLoaderConfig.workspaceId)
                             && x.ExtentUri ==
                             entry.Configuration.getOrDefault<string>(_ExtentLoaderConfig.extentUri));

                    if (found != null)
                    {
                        throw new InvalidOperationException(
                            "Database integrity is not given anymore: Duplicated extent: " +
                            found.ExtentUri);
                    }

                    list.Add(
                        new VerifyDatabaseEntry(
                            entry.Configuration.getOrDefault<string>(_ExtentLoaderConfig.workspaceId),
                            entry.Configuration.getOrDefault<string>(_ExtentLoaderConfig.extentUri)
                        ));
                }
            }
        }

        /// <summary>
        /// Returns a flag whether the extent contains the IsModified flag, indicating that the
        /// data is not stored to the disc or other permanent database
        /// </summary>
        /// <param name="extent">Extent to be evaluated</param>
        /// <returns>true, if the extent is modified</returns>
        public static bool IsExtentModified(IExtent extent) => 
            (extent as MofExtent)?.IsModified == true;

        private class VerifyDatabaseEntry
        {
            public VerifyDatabaseEntry(string workspace, string extentUri)
            {
                Workspace = workspace;
                ExtentUri = extentUri;
            }

            public string Workspace { get; set; }
            
            public string ExtentUri { get; set; }
        }
        
        /// <summary>
        /// Gets the provider capabilities of the provider behind the given extent
        /// </summary>
        /// <param name="extent">Extent to be evaluated</param>
        public static ProviderCapability GetProviderCapabilities(IExtent extent)
        {
            var asExtent = extent as MofExtent;
            return asExtent?.Provider.GetCapabilities() ??
                ProviderCapabilities.None;
        }
    }
}