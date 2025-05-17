using System.Diagnostics;
using System.Reflection;
using Autofac;
using Autofac.Features.ResolveAnything;
using BurnSystems.Logging;
using DatenMeister.BootStrap.PublicSettings;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Implementation.Hooks;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.ChangeEvents;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Runtime.Workspaces.Data;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Core.XmiFiles;
using DatenMeister.DependencyInjection;
using DatenMeister.Extent.Manager.Extents.Configuration;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Forms;
using DatenMeister.Forms.FormFinder;
using DatenMeister.Plugins;
using DatenMeister.Provider.Xmi.Provider.Xml;
using DatenMeister.Types;

namespace DatenMeister.BootStrap
{
    public class Integrator
    {
        private static readonly ClassLogger Logger = new(typeof(Integrator));

        private readonly PluginLoaderSettings _pluginLoaderSettings;
        private string? _pathExtents;

        private string? _pathWorkspaces;
        private PublicIntegrationSettings? _publicSettings;

        /// <summary>
        /// Settings being used for integration
        /// </summary>
        private IntegrationSettings _settings;

        private PluginManager? _pluginManager;
        private DatenMeisterScope? _dmScope;

        public Integrator(IntegrationSettings settings, PluginLoaderSettings pluginLoaderSettings)
        {
            _settings = settings;
            _pluginLoaderSettings = pluginLoaderSettings;
        }

        public string PathWorkspaces
        {
            get => _pathWorkspaces ?? throw new InvalidOperationException("PathWorkspaces is not set");
            private set => _pathWorkspaces = value;
        }

        public string PathExtents
        {
            get => _pathExtents ?? throw new InvalidOperationException("PathExtents is not set");
            private set => _pathExtents = value;
        }

        public static string GetPathToWorkspaces(IntegrationSettings settings)
            => Path.Combine(settings.DatabasePath, "DatenMeister.Workspaces.xml");

        /// <summary>
        /// Calculates the path to the extents
        /// </summary>
        /// <param name="settings">Settings to be set</param>
        /// <returns>The path</returns>
        public static string GetPathToExtents(IntegrationSettings settings)
            => Path.Combine(settings.DatabasePath, "DatenMeister.Extents.xml");

        public async Task<IDatenMeisterScope> UseDatenMeister(ContainerBuilder kernel)
        {
            if (_pluginManager != null)
            {
                throw new InvalidOperationException("The Integrator has already been used");
            }
            
            MofExtent.GlobalSlimUmlEvaluation = true;

            var scopeStorage = new ScopeStorage();
            kernel.RegisterInstance(scopeStorage).As<IScopeStorage>();

            PrepareSettings();
            if (_publicSettings != null)
            {
                scopeStorage.Add(_publicSettings);
            }

            scopeStorage.Add(_settings);

            kernel.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            // Creates the database path for the DatenMeister.
            // and avoids to have a non-rooted path because it will lead to double creation of assemblies
            if (!Path.IsPathRooted(_settings.DatabasePath))
            {
                var assembly = Assembly.GetEntryAssembly() ??
                               throw new InvalidOperationException("Entry assembly is null");

                var assemblyDirectoryName = Path.GetDirectoryName(assembly.Location) ??
                                            throw new InvalidOperationException("Assembly Directory Name is null");

                _settings.DatabasePath = Path.Combine(assemblyDirectoryName, _settings.DatabasePath);
            }

            if (!Directory.Exists(_settings.DatabasePath))
            {
                Directory.CreateDirectory(_settings.DatabasePath);
            }
            
            Logger.Info($"Database path: {_settings.DatabasePath}");

            // Performs the initialization
            var watch = new Stopwatch();
            watch.Start();

            // Finds the loader for a certain extent type
            var storageMap = new ProviderToProviderLoaderMapper();
            scopeStorage.Add(storageMap);

            // Defines the extent storage data
            var extentStorageData = new ExtentStorageData
            {
                FilePath = PathExtents
            };
            scopeStorage.Add(extentStorageData);
            kernel.RegisterType<ExtentManager>().As<ExtentManager>();

            // Workspaces
            var workspaceData = WorkspaceLogic.InitDefault();
            scopeStorage.Add(workspaceData);
            kernel.RegisterType<WorkspaceLogic>().As<IWorkspaceLogic>();

            // Assigns the workspacelogic to the temporary extent
            InMemoryProvider.TemporaryExtent.AssociateWorkspaceLogic(new WorkspaceLogic(scopeStorage));

            var extentSettings = new ExtentSettings();
            scopeStorage.Add(extentSettings);

            // Extent Manager
            var mapper = new ProviderToProviderLoaderMapper();
            scopeStorage.Add(mapper);
            mapper.AddMapping(
                _DatenMeister.TheOne.ExtentLoaderConfigs.__InMemoryLoaderConfig,
                manager => new InMemoryProviderLoader());
            mapper.AddMapping(
                _DatenMeister.TheOne.ExtentLoaderConfigs.__XmlReferenceLoaderConfig,
                manager => new XmlReferenceLoader());
            
            // First resolve hooks
            ResolveHookContainer.AddDefaultHooks(scopeStorage);

            // Create the change manager
            var changeEventManager = new ChangeEventManager();
            scopeStorage.Add(changeEventManager);

            // Loading and storing the workspaces
            var workspaceLoadingConfiguration = new WorkspaceLoaderConfig(PathWorkspaces);
            scopeStorage.Add(workspaceLoadingConfiguration);
            kernel.RegisterType<WorkspaceLoader>().As<WorkspaceLoader>();
            kernel.RegisterType<ExtentConfigurationLoader>().As<ExtentConfigurationLoader>();

            // Adds the view finder
            kernel.RegisterType<FormFinder>().As<FormFinder>();

            var pluginManager = new PluginManager();
            _pluginManager = pluginManager;
            scopeStorage.Add(_pluginManager);

            var pluginLoader = _pluginLoaderSettings.PluginLoader;
            var defaultPluginSettings = _settings.AdditionalSettings.TryGet<DefaultPluginSettings>();
            if (defaultPluginSettings != null && pluginLoader is DefaultPluginLoader defaultPluginLoader)
                defaultPluginLoader.Settings = defaultPluginSettings;

            pluginLoader.LoadAssembliesFromFolder(
                Path.GetDirectoryName(typeof(DatenMeisterScope).Assembly.Location)
                ?? throw new InvalidOperationException("Path is null"));

            Logger.Debug("Building Dependency Injector");
            var builder = kernel.Build();
            var scope = builder.BeginLifetimeScope();
            _dmScope = new DatenMeisterScope(scope);

            // Creates the content
            await _pluginManager.StartPlugins(_dmScope, pluginLoader, PluginLoadingPosition.BeforeBootstrapping);

            // Load the default extents
            // Performs the bootstrap
            var paths =
                new Bootstrapper.FilePaths
                {
                    LoadFromEmbeddedResources = string.IsNullOrEmpty(_settings.PathToXmiFiles),
                    PathPrimitive =
                        _settings.PathToXmiFiles == null
                            ? null
                            : Path.Combine(_settings.PathToXmiFiles, "PrimitiveTypes.xmi"),
                    PathUml =
                        _settings.PathToXmiFiles == null ? null : Path.Combine(_settings.PathToXmiFiles, "UML.xmi"),
                    PathMof =
                        _settings.PathToXmiFiles == null ? null : Path.Combine(_settings.PathToXmiFiles, "MOF.xmi")
                };

            var workspaceLogic = scope.Resolve<IWorkspaceLogic>();

            var umlWatch = new Stopwatch();
            umlWatch.Start();

            Logger.Debug("Bootstrapping MOF and UML...");
            var mofTask = Task.Run(() =>
                Bootstrapper.PerformFullBootstrap(
                    paths,
                    workspaceLogic.GetWorkspace(WorkspaceNames.WorkspaceMof) ??
                    throw new InvalidOperationException("Workspace for MOF is not found"),
                    workspaceLogic,
                    workspaceData.Mof,
                    _settings.PerformSlimIntegration ? BootstrapMode.SlimMof : BootstrapMode.Mof));
            var umlTask = Task.Run(() =>
                Bootstrapper.PerformFullBootstrap(
                    paths,
                    workspaceLogic.GetWorkspace(WorkspaceNames.WorkspaceUml) ??
                    throw new InvalidOperationException("Workspace for UML is not found"),
                    workspaceLogic,
                    workspaceData.Uml,
                    _settings.PerformSlimIntegration ? BootstrapMode.SlimUml : BootstrapMode.Uml));
            Task.WaitAll(mofTask, umlTask);

            umlWatch.Stop();

            Logger.Info($" Bootstrapping Done: {Math.Floor(umlWatch.Elapsed.TotalMilliseconds)} ms");

            await _pluginManager.StartPlugins(_dmScope, pluginLoader, PluginLoadingPosition.AfterBootstrapping);

            // Creates the workspace and extent for the types layer which are belonging to the types
            var localTypeSupport = scope.Resolve<LocalTypeSupport>();
            var internalUserExtent = localTypeSupport.InternalTypes;

            PackageMethods.ImportByStream(
                XmiResources.GetDatenMeisterTypesStream(),
                null,
                internalUserExtent,
                "DatenMeister");

            var formMethods = scope.Resolve<FormMethods>();
            PackageMethods.ImportByStream(
                XmiResources.GetDatenMeisterFormsStream(),
                null,
                formMethods.GetInternalFormExtent(),
                "DatenMeister");

            // Deactivates the global slim evaluation since we now have all necessary types imported. 
            MofExtent.GlobalSlimUmlEvaluation = false;

            // Finally loads the plugin
            await _pluginManager.StartPlugins(_dmScope, pluginLoader, PluginLoadingPosition.AfterInitialization);

            // Boots up the typical DatenMeister Environment by loading the data
            var extentManager = scope.Resolve<ExtentManager>();
            if (_settings.EstablishDataEnvironment)
            {
                var workspaceLoader = scope.Resolve<WorkspaceLoader>();
                workspaceLoader.Load();

                // Loads all extents after all plugins were started
                try
                {
                    await extentManager.LoadAllExtents();
                }
                catch (LoadingExtentsFailedException)
                {
                    Logger.Info("Failure of loading extents will lead to a read-only application");

                    if (Debugger.IsAttached && ExtentConfigurationLoader.BreakOnFailedWorkspaceLoading)
                    {
                        Debugger.Break();
                    }

                    if (_settings.AllowNoFailOfLoading)
                    {
                        throw;
                    }
                }
            }

            // Finally loads the plugin
            await _pluginManager.StartPlugins(_dmScope, pluginLoader, PluginLoadingPosition.AfterLoadingOfExtents);

            ResetUpdateFlagsOfExtent(workspaceLogic);

            watch.Stop();
            Logger.Debug($"Elapsed time for bootstrap: {watch.Elapsed}");

            return _dmScope;
        }

        /// <summary>
        /// Performs a shutdown. This method just calls the plugins currently
        /// </summary>
        public async Task UnuseDatenMeister()
        {
            if (_pluginManager == null || _dmScope == null)
            {
                throw new InvalidOperationException("The Integrator has not been started by UseDatenMeister");
            }

            await _pluginManager.StartPlugins(
                _dmScope,
                _pluginLoaderSettings.PluginLoader,
                PluginLoadingPosition.AfterShutdownStarted);
        }

        /// <summary>
        /// Goes through each extent and resets the update flag for all extents with providers
        /// indicating that they are just a temporary extent
        /// </summary>
        /// <param name="workspaceLogic">The workspace logic to be used</param>
        private static void ResetUpdateFlagsOfExtent(IWorkspaceLogic workspaceLogic)
        {
            // After the complete bootstrapping is done, the Update flags for the TemporaryExtents will be removed
            foreach (var workspace in workspaceLogic.Workspaces)
            {
                foreach (var extent in workspace.extent.OfType<MofExtent>())
                {
                    if (extent.Provider.GetCapabilities()?.IsTemporaryStorage == true)
                    {
                        extent.SignalUpdateOfContent(false);
                    }
                }
            }
        }

        /// <summary>
        /// Prepares the settings by looking into the public settings which may reside within the file
        /// </summary>
        private void PrepareSettings()
        {
            if (_settings == null)
            {
                Logger.Info("No integration settings were given. Loading the default values.");
                _settings = new IntegrationSettings();
            }

            // Checks whether a public setting is available
            try
            {
                var path = Assembly.GetEntryAssembly()?.Location;
                if (path != null)
                {
                    _publicSettings = PublicSettingHandler.LoadSettingsFromDirectory(Path.GetDirectoryName(path) ??
                        throw new InvalidOperationException("Path is null"), out _);
                    if (_publicSettings != null)
                    {
                        if (_publicSettings.databasePath != null && !string.IsNullOrEmpty(_publicSettings.databasePath))
                        {
                            Logger.Info($"Overwriting database path to {_publicSettings.databasePath}");

                            _settings.DatabasePath = _publicSettings.databasePath;
                        }

                        if (_publicSettings.windowTitle != null && !string.IsNullOrEmpty(_publicSettings.windowTitle))
                        {
                            _settings.WindowTitle = _publicSettings.windowTitle;
                        }

                        _settings.IsReadOnly = _publicSettings.isReadOnly;
                    }
                }
            }
            catch (Exception exc)
            {
                Logger.Warn($"Error during loading of public settings {exc.Message}");
            }

            PathWorkspaces = GetPathToWorkspaces(_settings);
            PathExtents = GetPathToExtents(_settings);
        }
    }
}