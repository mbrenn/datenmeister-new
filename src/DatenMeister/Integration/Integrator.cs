using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Autofac;
using Autofac.Features.ResolveAnything;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.UserManagement;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.DotNet;
using DatenMeister.Provider.InMemory;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Provider.XMI.ExtentStorage;
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
        private const string PathWorkspaces = "App_Data/Database/workspaces.xml";

        private const string PathExtents = "App_Data/Database/extents.xml";

        private const string PathUserTypes = "App_Data/Database/usertypes.xml";

        private IntegrationSettings _settings;

        public Integrator(IntegrationSettings settings)
        {
            _settings = settings;
        }

        public IContainer UseDatenMeister(ContainerBuilder kernel)
        {
            if (_settings == null)
            {
                Debug.WriteLine("No integration settings were given. Loading the default values.");
                _settings = new IntegrationSettings();
            }

            kernel.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

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

            // Adds the name resolution  
            kernel.RegisterType<UmlNameResolution>().As<IUmlNameResolution>();

            
            // Loading and storing the workspaces
            var workspaceLoadingConfiguration = new WorkspaceLoaderConfig
            {
                Filepath = PathWorkspaces
            };

            kernel.RegisterInstance(workspaceLoadingConfiguration).As<WorkspaceLoaderConfig>();
            kernel.RegisterType<WorkspaceLoader>().As<WorkspaceLoader>();

            kernel.RegisterType<ExtentConfigurationLoader>().As<ExtentConfigurationLoader>();

            // Adds the view finder
            kernel.RegisterType<ViewFinderImpl>().As<IViewFinder>();

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
                var extentTypes = new MofUriExtent(
                    new InMemoryProvider(), 
                    WorkspaceNames.UriInternalTypes);
                var mofFactory = new MofFactory(extentTypes);
                var typeWorkspace = workspaceLogic.GetWorkspace(WorkspaceNames.NameTypes);
                workspaceLogic.AddExtent(typeWorkspace, extentTypes);

                // Adds the module for form and fields
                var fields = new _FormAndFields();
                typeWorkspace.Set(fields);
                IntegrateFormAndFields.Assign(
                    workspaceData.Uml.Get<_UML>(),
                    mofFactory,
                    extentTypes.elements(),
                    fields,
                    extentTypes);

                // Adds the views and their view logic
                scope.Resolve<ViewLogic>().Integrate();
                scope.Resolve<ViewDefinitions>().AddToViewDefinition();
                
                // Includes the extent for the helping extents
                Provider.HelpingExtents.ManagementProviderHelper.Initialize(workspaceLogic);


                // Boots up the typical DatenMeister Environment  
                if (_settings.EstablishDataEnvironment)
                {
                    LoadsWorkspacesAndExtents(scope);
                    scope.Resolve<UserLogic>().Initialize();
                }

                CreatesUserTypeExtent(scope);
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

        /// <summary>
        /// Creates the user type extent storing the types for the user. 
        /// If the extent is already existing, debugs the number of found extents
        /// </summary>
        /// <param name="scope">Dependency injection container</param>
        private static void CreatesUserTypeExtent(ILifetimeScope scope)
        {
            var workspaceCollection = scope.Resolve<IWorkspaceLogic>();

            // Creates the user types, if not existing
            var foundExtent = workspaceCollection.FindExtent(WorkspaceNames.UriUserTypes);
            if (foundExtent == null)
            {
                Debug.WriteLine("Creates the extent for the user types");
                // Creates the extent for user types
                var loader = scope.Resolve<ExtentManager>();
                var storageConfiguration = new XmiStorageConfiguration
                {
                    ExtentUri = WorkspaceNames.UriUserTypes,
                    Path = PathUserTypes,
                    Workspace = WorkspaceNames.NameTypes,
                    DataLayer = "Types"
                };

                foundExtent = loader.LoadExtent(storageConfiguration, true);
            }
            else
            {
                var numberOfTypes = foundExtent.elements().Count();
                Debug.WriteLine($"Loaded the extent for user types, containing of {numberOfTypes} types");
            }

            foundExtent.SetExtentType("Uml.Classes");
        }
    }
}