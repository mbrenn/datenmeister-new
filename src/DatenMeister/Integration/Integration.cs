using System;
using System.Diagnostics;
using System.IO;
using Autofac;
using Autofac.Features.ResolveAnything;
using DatenMeister.Core;
using DatenMeister.Core.DataLayer;
using DatenMeister.Core.EMOF.Attributes;
using DatenMeister.Core.EMOF.InMemory;
using DatenMeister.Core.Filler;
using DatenMeister.CSV.Runtime.Storage;
using DatenMeister.Models.Forms;
using DatenMeister.Models.Modules.ViewFinder;
using DatenMeister.Provider.DotNet;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.FactoryMapper;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Runtime.Workspaces.Data;
using DatenMeister.Uml;
using DatenMeister.Uml.Helper;
using DatenMeister.XMI.EMOF;
using DatenMeister.XMI.ExtentStorage;
using WorkspaceData = DatenMeister.Core.DataLayer.WorkspaceData;

namespace DatenMeister.Integration
{
    public class Integration
    {

        private const string PathWorkspaces = "App_Data/Database/workspaces.xml";

        private const string PathExtents = "App_Data/Database/extents.xml";

        private const string PathUserTypes = "App_Data/Database/usertypes.xml";

        private IntegrationSettings _settings;

        public Integration(IntegrationSettings settings)
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

            // Defines the factory method for a certain extent type  
            var factoryMapper = new DefaultFactoryMapper();
            kernel.RegisterInstance(factoryMapper).As<IFactoryMapper>();

            // Finds the loader for a certain extent type  
            var storageMap = new ManualConfigurationToExtentStorageMapper();
            kernel.RegisterInstance(storageMap).As<IConfigurationToExtentStorageMapper>();

            // Workspace collection  
            var workspaceCollection = new WorkspaceCollection();
            workspaceCollection.Init();
            kernel.RegisterInstance(workspaceCollection).As<IWorkspaceCollection>();

            // Defines the extent storage data  
            var extentStorageData = new ExtentStorageData();
            kernel.RegisterInstance(extentStorageData).As<ExtentStorageData>();
            kernel.RegisterType<ExtentStorageLoader>().As<IExtentStorageLoader>();
            
            WorkspaceData dataLayerData;
            var workspaces = WorkspaceLogic.InitDefault(out dataLayerData);
            kernel.RegisterInstance(dataLayerData).As<WorkspaceData>();
            kernel.RegisterType<WorkspaceLogic>().As<IWorkspaceLogic>();
            kernel.RegisterInstance(workspaces).As<Workspaces>();

            // Adds the name resolution  
            kernel.RegisterType<UmlNameResolution>().As<IUmlNameResolution>();

            // Adds the complete .Net-Type handling
            var dotNetTypeLookup = new DotNetTypeLookup();
            kernel.RegisterInstance(dotNetTypeLookup).As<IDotNetTypeLookup>();

            var builder = kernel.Build();

            using (var scope = builder.BeginLifetimeScope())
            {
                Core.EMOF.Integrate.Into(scope);
                CSV.Integrate.Into(scope);
                XMI.Integrate.Into(scope);

                // Is used by .Net Provider to include the mappings for extent storages and factory types
                _settings?.Hooks?.OnStartScope(scope);

                var dataLayerLogic = scope.Resolve<IWorkspaceLogic>();

                // Load the default extents  

                // Performs the bootstrap  
                var paths =
                    new Bootstrapper.FilePaths
                    {
                        LoadFromEmbeddedResources = string.IsNullOrEmpty(_settings.PathToXmiFiles),
                        PathPrimitive = _settings.PathToXmiFiles == null ? null : Path.Combine(_settings.PathToXmiFiles, "PrimitiveTypes.xmi"),
                        PathUml = _settings.PathToXmiFiles == null ? null : Path.Combine(_settings.PathToXmiFiles, "UML.xmi"),
                        PathMof = _settings.PathToXmiFiles == null ? null : Path.Combine(_settings.PathToXmiFiles, "MOF.xmi")
                    };

                if (_settings.PerformSlimIntegration)
                {
                    throw new InvalidOperationException("Slim integration is currently not supported");
                }

                var umlWatch = new Stopwatch();
                umlWatch.Start();
                Debug.Write("Bootstrapping MOF and UML...");
                Bootstrapper.PerformFullBootstrap(
                    paths,
                    workspaceCollection.GetWorkspace(WorkspaceNames.Mof),
                    dataLayerLogic,
                    workspaces.Mof,
                    BootstrapMode.Mof);
                Bootstrapper.PerformFullBootstrap(
                    paths,
                    workspaceCollection.GetWorkspace(WorkspaceNames.Uml),
                    dataLayerLogic,
                    workspaces.Uml,
                    BootstrapMode.Uml);
                umlWatch.Stop();
                Debug.WriteLine($" Done: {umlWatch.Elapsed.Milliseconds} ms");

                // Creates the workspace and extent for the types layer which are belonging to the types  
                var mofFactory = new MofFactory();
                var extentTypes = new MofUriExtent(Locations.UriInternalTypes);
                var typeWorkspace = workspaceCollection.GetWorkspace(WorkspaceNames.Types);
                typeWorkspace.AddExtent(extentTypes);
                dataLayerLogic.AssignToDataLayer(extentTypes, workspaces.Types);


                // Adds the module for form and fields
                var fields = new _FormAndFields();
                dataLayerLogic.Set(workspaces.Data, fields);
                IntegrateFormAndFields.Assign(
                    dataLayerLogic.Get<_UML>(workspaces.Uml),
                    mofFactory,
                    extentTypes.elements(),
                    fields,
                    dotNetTypeLookup);

                var viewLogic = new ViewLogic(workspaceCollection, dataLayerLogic);
                viewLogic.Integrate();

                // Boots up the typical DatenMeister Environment  
                if (_settings.EstablishDataEnvironment)
                {
                    LoadsWorkspacesAndExtents(builder, scope);
                }

                CreatesUserTypeExtent(scope);
            }

            watch.Stop();
            Debug.WriteLine($"Elapsed time for bootstrap: {watch.Elapsed}");

            return builder;
        }

        private void LoadsWorkspacesAndExtents(IContainer builder, ILifetimeScope scope)
        {
            var innerContainer = new ContainerBuilder();
            // Loading and storing the workspaces  
            var workspaceLoader = new WorkspaceLoader(scope.Resolve<IWorkspaceCollection>(),
                PathWorkspaces);
            workspaceLoader.Load();
            innerContainer.RegisterInstance(workspaceLoader).As<WorkspaceLoader>();

            // Loading and storing the extents  
            var extentLoader = new ExtentStorageConfigurationLoader(
                scope.Resolve<ExtentStorageData>(),
                scope.Resolve<IExtentStorageLoader>(),
                PathExtents);
            innerContainer.Register(c => new ExtentStorageConfigurationLoader(
                    c.Resolve<ExtentStorageData>(),
                    c.Resolve<IExtentStorageLoader>(),
                    PathExtents))
                .As<ExtentStorageConfigurationLoader>();

            innerContainer.Update(builder);

            // Now start the plugins  
            _settings?.Hooks?.BeforeLoadExtents(scope);

            // Loads all extents after all plugins were started  
            extentLoader.LoadAllExtents();
        }

        private static void CreatesUserTypeExtent(ILifetimeScope scope)
        {
            var workspaceCollection = scope.Resolve<IWorkspaceCollection>();

            // Creates the user types, if not existing
            if (workspaceCollection.FindExtent(Locations.UriUserTypes) == null)
            {
                Debug.WriteLine("Creates the extent for the user types");
                // Creates the extent for user types
                var loader = scope.Resolve<ExtentStorageLoader>();
                var storageConfiguration = new XmiStorageConfiguration
                {
                    ExtentUri = Locations.UriUserTypes,
                    Path = PathUserTypes,
                    Workspace = WorkspaceNames.Types,
                    DataLayer = "Types"
                };

                loader.LoadExtent(storageConfiguration, true);
            }
        }
    }
}