using System;
using System.Diagnostics;
using System.IO;
using Autofac;
using Autofac.Features.ResolveAnything;
using DatenMeister.DataLayer;
using DatenMeister.EMOF.Attributes;
using DatenMeister.EMOF.InMemory;
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

namespace DatenMeister.Integration
{
    public class Integration
    {
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

            // Defines the datalayers  
            var dataLayers = new DataLayers();
            kernel.RegisterInstance(dataLayers).As<DataLayers>();

            var dataLayerData = new DataLayerData(dataLayers);
            kernel.RegisterInstance(dataLayerData).As<DataLayerData>();
            kernel.RegisterType<DataLayerLogic>().As<IDataLayerLogic>();

            // Adds the name resolution  
            kernel.RegisterType<UmlNameResolution>().As<IUmlNameResolution>();

            // Adds the complete .Net-Type handling
            var dotNetTypeLookup = new DotNetTypeLookup();
            kernel.RegisterInstance(dotNetTypeLookup).As<IDotNetTypeLookup>();

            var builder = kernel.Build();
            using (var scope = builder.BeginLifetimeScope())
            {
                // Is used by .Net Provider to include the mappings for extent storages and factory types
                _settings?.Hooks?.OnStartScope(scope);

                var dataLayerLogic = scope.Resolve<IDataLayerLogic>();
                dataLayers.SetRelationsForDefaultDataLayers(dataLayerLogic);

                // Load the default extents  

                // Performs the bootstrap  
                var paths =
                    new Bootstrapper.FilePaths
                    {
                        PathPrimitive = Path.Combine(_settings.PathToXmiFiles, "PrimitiveTypes.xmi"),
                        PathUml = Path.Combine(_settings.PathToXmiFiles, "UML.xmi"),
                        PathMof = Path.Combine(_settings.PathToXmiFiles, "MOF.xmi")
                    };

                if (_settings.PerformSlimIntegration)
                {
                    throw new InvalidOperationException("Slim integration is currently not supported");
                }

                Debug.Write("Bootstrapping MOF and UML...");
                Bootstrapper.PerformFullBootstrap(
                    paths,
                    workspaceCollection.GetWorkspace(WorkspaceNames.Uml),
                    dataLayerLogic,
                    dataLayers.Uml);
                Bootstrapper.PerformFullBootstrap(
                    paths,
                    workspaceCollection.GetWorkspace(WorkspaceNames.Mof),
                    dataLayerLogic,
                    dataLayers.Mof);
                Debug.WriteLine(" Done");

                // Creates the workspace and extent for the types layer which are belonging to the types  
                var mofFactory = new MofFactory();
                var extentTypes = new MofUriExtent(Locations.UriTypes);
                var typeWorkspace = workspaceCollection.GetWorkspace(WorkspaceNames.Types);
                typeWorkspace.AddExtent(extentTypes);
                dataLayerLogic.AssignToDataLayer(extentTypes, dataLayers.Types);

                // Adds the module for form and fields
                var fields = new _FormAndFields();
                IntegrateFormAndFields.Assign(
                    dataLayerLogic.Get<_UML>(dataLayers.Uml),
                    mofFactory,
                    extentTypes.elements(),
                    fields,
                    dotNetTypeLookup);

                var viewLogic = new ViewLogic(workspaceCollection, dotNetTypeLookup);
                viewLogic.Integrate();

                // Boots up the typical DatenMeister Environment  
                if (_settings.EstablishDataEnvironment)
                {
                    EstablishDataEnvironment(builder, scope);
                }
            }

            watch.Stop();
            Debug.WriteLine($"Elapsed time for bootstrap: {watch.Elapsed}");

            return builder;
        }

        private const string PathWorkspaces = "App_Data/Database/workspaces.xml";

        private const string PathExtents = "App_Data/Database/extents.xml";

        private void EstablishDataEnvironment(IContainer builder, ILifetimeScope scope)
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
    }
}