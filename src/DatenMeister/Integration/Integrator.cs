using System;
using System.Diagnostics;
using System.IO;
using Autofac;
using Autofac.Features.ResolveAnything;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Plugins;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Modules.UserManagement;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Provider.ManagementProviders.Model;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Runtime.Workspaces.Data;
using DatenMeister.Uml;
using DatenMeister.Uml.Helper;
using WorkspaceData = DatenMeister.Runtime.Workspaces.WorkspaceData;

namespace DatenMeister.Integration
{
    public class Integrator
    {
        public string PathWorkspaces { get; }
        public string PathExtents { get; }

        public static string GetPathToWorkspaces(IntegrationSettings settings)
        {
            return Path.Combine(settings.DatabasePath, "DatenMeister.Workspaces.xml");
        }

        /// <summary>
        /// Calculates the path to the extents
        /// </summary>
        /// <param name="settings">Settings to be set</param>
        /// <returns>The path</returns>
        public static string GetPathToExtents(IntegrationSettings settings)
        {
            return Path.Combine(settings.DatabasePath, "DatenMeister.Extents.xml");
        }

        private IntegrationSettings _settings;

        public Integrator(IntegrationSettings settings)
        {
            _settings = settings;

            PathWorkspaces = GetPathToWorkspaces(settings);
            PathExtents = GetPathToExtents(settings);
        }

        public IContainer UseDatenMeister(ContainerBuilder kernel)
        {
            if (_settings == null)
            {
                Debug.WriteLine("No integration settings were given. Loading the default values.");
                _settings = new IntegrationSettings();
            }

            kernel.RegisterInstance(_settings).As<IntegrationSettings>();
            kernel.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            // Creates the database path for the DatenMeister
            if (!Directory.Exists(_settings.DatabasePath))
            {
                Directory.CreateDirectory(_settings.DatabasePath);
            }

            var watch = new Stopwatch();
            watch.Start();

            // Finds the loader for a certain extent type  
            var storageMap = new ManualConfigurationToExtentStorageMapper();
            kernel.RegisterInstance(storageMap).As<IConfigurationToExtentStorageMapper>();

            // Defines the extent storage data  
            var extentStorageData = new ExtentStorageData
            {
                FilePath = PathExtents
            };
            kernel.RegisterInstance(extentStorageData).As<ExtentStorageData>();
            kernel.RegisterType<ExtentManager>().As<IExtentManager>();

            // Workspaces
            var workspaceData = WorkspaceLogic.InitDefault();
            kernel.RegisterInstance(workspaceData).As<WorkspaceData>();
            kernel.RegisterType<WorkspaceLogic>().As<IWorkspaceLogic>();
            
            // Loading and storing the workspaces
            var workspaceLoadingConfiguration = new WorkspaceLoaderConfig
            {
                filepath = PathWorkspaces
            };

            kernel.RegisterInstance(workspaceLoadingConfiguration).As<WorkspaceLoaderConfig>();
            kernel.RegisterType<WorkspaceLoader>().As<WorkspaceLoader>();

            kernel.RegisterType<ExtentConfigurationLoader>().As<ExtentConfigurationLoader>();

            // Adds the view finder
            kernel.RegisterType<ViewFinderImpl>().As<IViewFinder>();

            var pluginManager = new PluginManager();
            kernel.RegisterInstance(pluginManager).As<PluginManager>();

            Modules.ZipExample.Integrate.Into(kernel);

            var builder = kernel.Build();
            using (var scope = builder.BeginLifetimeScope())
            {
                Provider.CSV.Integrate.Into(scope);
                Provider.XMI.Integrate.Into(scope);

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
                Debug.Write("Bootstrapping MOF and UML...");
                Bootstrapper.PerformFullBootstrap(
                    paths,
                    workspaceLogic.GetWorkspace(WorkspaceNames.NameMof),
                    workspaceLogic,
                    workspaceData.Mof,
                    _settings.PerformSlimIntegration ? BootstrapMode.SlimMof : BootstrapMode.Mof);
                Bootstrapper.PerformFullBootstrap(
                    paths,
                    workspaceLogic.GetWorkspace(WorkspaceNames.NameUml),
                    workspaceLogic,
                    workspaceData.Uml,
                    _settings.PerformSlimIntegration ? BootstrapMode.SlimUml : BootstrapMode.Uml);
                umlWatch.Stop();

                Debug.WriteLine($" Done: {Math.Floor(umlWatch.Elapsed.TotalMilliseconds)} ms");

                // Creates the workspace and extent for the types layer which are belonging to the types  
                var localTypeSupport = scope.Resolve<LocalTypeSupport>();
                localTypeSupport.Initialize();
                var typeWorkspace = workspaceLogic.GetTypesWorkspace();
                var mofFactory = new MofFactory(localTypeSupport.InternalTypes);
                var packageMethods = scope.Resolve<PackageMethods>();

                // Adds the module for form and fields
                var fields = new _FormAndFields();
                typeWorkspace.Set(fields);
                IntegrateFormAndFields.Assign(
                    workspaceData.Uml.Get<_UML>(),
                    mofFactory,
                    packageMethods.GotoPackage(localTypeSupport.InternalTypes.elements(), "DatenMeister::Forms"),
                    fields,
                    (MofUriExtent) localTypeSupport.InternalTypes);

                // Adds the module for managementprovider
                var managementProvider = new _ManagementProvider();
                typeWorkspace.Set(fields);
                IntegrateManagementProvider.Assign(
                    workspaceData.Uml.Get<_UML>(),
                    mofFactory,
                    packageMethods.GotoPackage(localTypeSupport.InternalTypes.elements(), "DatenMeister::Management"),
                    managementProvider,
                    (MofUriExtent) localTypeSupport.InternalTypes);

                // Adds the views and their view logic
                scope.Resolve<ViewLogic>().Integrate();
                
                // Includes the extent for the helping extents
                ManagementProviderHelper.Initialize(
                    workspaceLogic);

                // Boots up the typical DatenMeister Environment  
                if (_settings.EstablishDataEnvironment)
                {
                    LoadsWorkspacesAndExtents(scope);
                    scope.Resolve<UserLogic>().Initialize();
                }

                // Performs the integration into the DatenMeister
                Modules.ZipExample.Integrate.Into(scope);

                // Finally loads the plugin
                pluginManager.StartPlugins(scope);

                // After the plugins are loaded, check the extent storage types and create the corresponding internal management types
                var extentManager = scope.Resolve<IExtentManager>();
                extentManager.CreateStorageTypeDefinitions();
            }

            watch.Stop();
            Debug.WriteLine($"Elapsed time for bootstrap: {watch.Elapsed}");

            return builder;
        }

        /// <summary>
        /// Loads the workspaces from memory and the extents according to the files.
        /// </summary>
        /// <param name="scope">Dependency injection container</param>
        private void LoadsWorkspacesAndExtents(ILifetimeScope scope)
        {
            var workspaceLoader = scope.Resolve<WorkspaceLoader>();
            workspaceLoader.Load();

            // Loads all extents after all plugins were started  
            scope.Resolve<ExtentConfigurationLoader>().LoadAllExtents();
        }
    }
}