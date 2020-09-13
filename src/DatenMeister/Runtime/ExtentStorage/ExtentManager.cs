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
using DatenMeister.Provider;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Locking;
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
    public class ExtentManager
    {
        public const string PackagePathTypesExtentLoaderConfig = "DatenMeister::ExtentLoaderConfig";

        private readonly ExtentStorageData _extentStorageData;

        private static readonly ClassLogger Logger = new ClassLogger(typeof(ExtentManager));

        /// <summary>
        /// Stores the mapping between configuration types and storage provider
        /// </summary>
        private readonly ConfigurationToExtentStorageMapper _map;

        private readonly ILifetimeScope? _diScope;

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
            _extentStorageData = scopeStorage?.Get<ExtentStorageData>() ?? throw new InvalidOperationException("Extent Storage Data not found");
            _integrationSettings = scopeStorage.Get<IntegrationSettings>() ?? throw new InvalidOperationException("IntegrationSettings not found");
            _lockingHandler = _integrationSettings.IsLockingActivated ? new LockingLogic(scopeStorage) : null;

            WorkspaceLogic = workspaceLogic ?? throw new ArgumentNullException(nameof(workspaceLogic));
            _map = ScopeStorage.Get<ConfigurationToExtentStorageMapper>();
        }

        /// <summary>
        /// Loads the extent by using the extent storage by using the configuration and finding
        /// the correct storage engine
        /// </summary>
        /// <param name="configuration">Configuration being used to load</param>
        /// <param name="extentCreationFlags">The flags for the creation</param>
        /// <returns>The loaded extent</returns>
        public ExtentStorageData.LoadedExtentInformation LoadExtent(ExtentLoaderConfig configuration, ExtentCreationFlags extentCreationFlags = ExtentCreationFlags.LoadOnly)
        {
            // First, check, if there is already an extent loaded in the internal database with that uri and workspace

            lock (_extentStorageData.LoadedExtents)
            {
                if (_extentStorageData.LoadedExtents.Any(
                    x => configuration.workspaceId == x.Configuration.workspaceId
                         && configuration.extentUri == x.Configuration.extentUri))
                {
                    throw new InvalidOperationException(
                        $"There is already the extent loaded with extenturi: {configuration.extentUri}");
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
                AddToWorkspaceIfPossible(configuration, uriExtent);
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
        public ExtentStorageData.LoadedExtentInformation? LoadExtentWithoutAdding(ExtentLoaderConfig configuration) =>
            LoadExtentWithoutAddingInternal(configuration, ExtentCreationFlags.LoadOnly);

        /// <summary>
        /// Loads the extent according given configuration and returns the information dataset
        /// describing the used loaded configuration
        /// </summary>
        /// <param name="configuration">Configuration being used for the loading</param>
        /// <param name="extentCreationFlags">The flags describing the loading profile</param>
        /// <returns>The configuration information</returns>
        private ExtentStorageData.LoadedExtentInformation LoadExtentWithoutAddingInternal(ExtentLoaderConfig configuration, ExtentCreationFlags extentCreationFlags)
        {
            var loadedExtentInformation = new ExtentStorageData.LoadedExtentInformation(configuration)
            {
                LoadingState = ExtentLoadingState.Unloaded
            };

            // Checks, if the given URL has a relative path and transforms the path to an absolute path
            if (configuration is ExtentFileLoaderConfig fileConfiguration)
            {
                if (fileConfiguration.filePath != null && !Path.IsPathRooted(fileConfiguration.filePath))
                {
                    fileConfiguration.filePath = Path.Combine(_integrationSettings.DatabasePath, fileConfiguration.filePath);
                }

                if (Directory.Exists(fileConfiguration.filePath))
                {
                    throw new InvalidOperationException("Given file is a directory name. ");
                }
            }
            
            // Check, if the extent url is a real uri
            if (!Uri.IsWellFormedUriString(configuration.extentUri, UriKind.Absolute))
            {
                throw new InvalidOperationException($"Uri of Extent is not well-formed: {configuration.extentUri}");
            }

            var extentLoader = CreateProviderLoader(configuration);

            // Ok, now we have the provider. If the provider also supports the locking, check whether it can be locked
            if (extentLoader is IProviderLocking providerLocking && _integrationSettings.IsLockingActivated)
            {
                if (providerLocking.IsLocked(configuration))
                {
                    var asFilePath = configuration as ExtentFileLoaderConfig;
                    var filePath = asFilePath?.filePath ?? string.Empty;

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
                loadedExtentInformation.LoadingState = ExtentLoadingState.Loaded;
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
                var workspaceId = loadedProviderInfo.UsedConfig?.workspaceId ?? string.Empty;
                var extentUri = loadedProviderInfo.UsedConfig?.extentUri ?? string.Empty;
                var alreadyFoundExtent = (IUriExtent?) WorkspaceLogic.FindExtent(
                    workspaceId,
                    extentUri);
                if (alreadyFoundExtent == null)
                {
                    throw new InvalidOperationException("The extent was not found: " +
                                                        extentUri);
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

            var mofUriExtent = new MofUriExtent(loadedProvider, configuration.extentUri);
            loadedExtentInformation.Extent = mofUriExtent;
            mofUriExtent.SignalUpdateOfContent(false);

            VerifyDatabaseContent();

            return loadedExtentInformation;
        }

        private IProviderLoader CreateProviderLoader(ExtentLoaderConfig configuration)
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
                    x.LoadingState == ExtentLoadingState.Loaded &&
                    x.Extent?.@equals(extent) == true);
            }

            return information;
        }


        /// <summary>
        /// Gets the loading configuration for the given extent or null, if
        /// the extent does not contain a configuration
        /// </summary>
        /// <param name="extent">The extent whose configuration is retrieved</param>
        /// <returns>The configuration</returns>
        public ExtentLoaderConfig? GetLoadConfigurationFor(IUriExtent extent)
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

                var fullNameSet = new HashSet<string>();
                var distinctList = new List<Type>();
                foreach (var item in list.AsEnumerable().Reverse())
                {
                    if (fullNameSet.Contains(item.FullName))
                    {
                        continue;
                    }

                    distinctList.Add(item);
                    fullNameSet.Add(item.FullName);
                }

                _diScope.Resolve<LocalTypeSupport>().AddInternalTypes(
                    distinctList, 
                    PackagePathTypesExtentLoaderConfig);
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
        public void UnlockProvider(ExtentLoaderConfig configuration)
        {
            var providerLoader = CreateProviderLoader(configuration);
            if (providerLoader is IProviderLocking providerLocking && _integrationSettings.IsLockingActivated)
            {
                providerLocking.Unlock(configuration);
            }
        }

        /// <summary>
        /// Detaches the extent by removing it from the database of loaded extents.
        /// The extent will also be unlocked
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
                
                _extentStorageData.IsOpened = true;
                _extentStorageData.IsRegistrationOpen = true;

                // Stores the last the exception
                Exception? lastException = null;
                
                var configurationLoader = new ExtentConfigurationLoader(ScopeStorage, this);
                List<Tuple<ExtentLoaderConfig, XElement>>? loaded = null;
                try
                {
                    loaded = configurationLoader.GetConfigurationFromFile();
                }
                catch (Exception exc)
                {
                    Logger.Warn("Exception during loading of Extents: " + exc.Message);
                    lastException = exc;
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
                            if (xElement != null && extent.Extent != null)
                            {
                                ((MofExtent) extent.Extent).LocalMetaElementXmlNode = xElement;
                            }
                        }
                        catch (Exception exc)
                        {
                            Logger.Warn($"Loading extent of {extentLoaderConfig.extentUri} failed: {exc.Message}");
                            failedExtents.Add(extentLoaderConfig.extentUri);
                            lastException = exc;
                        }
                    }
                }

                foreach (var (extentLoaderConfig, xElement) in loaded)
                {
                    try
                    {
                        var extent = LoadExtent(extentLoaderConfig);
                        if (xElement != null && extent.Extent != null)
                        {
                            ((MofExtent) extent.Extent).LocalMetaElementXmlNode = xElement;
                        }
                    }
                    catch (Exception exc)
                    {
                        Logger.Warn($"Loading extent of {extentLoaderConfig.extentUri} failed: {exc.Message}");
                        failedExtents.Add(extentLoaderConfig.extentUri);
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
                    Logger.Error($"Error during storing of extent: {info.Configuration.extentUri}: {exc.Message}");
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

                if (doStore)
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
                    var configurationLoader = new ExtentConfigurationLoader(ScopeStorage, this);
                    configurationLoader.StoreConfiguration();

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
                        x => x.Workspace == entry.Configuration.workspaceId
                             && x.ExtentUri == entry.Configuration.extentUri);

                    if (found != null)
                    {
                        throw new InvalidOperationException(
                            "Database integrity is not given anymore: Duplicated extent: " +
                            found.ExtentUri);
                    }

                    list.Add(
                        new VerifyDatabaseEntry(
                            entry.Configuration.workspaceId,
                            entry.Configuration.extentUri
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