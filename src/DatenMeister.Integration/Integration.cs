using System;
using System.Diagnostics;
using System.IO;
using Autofac;
using Autofac.Features.ResolveAnything;
using DatenMeister.DataLayer;
using DatenMeister.EMOF.InMemory;
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

            var builder = kernel.Build();
            using (var scope = builder.BeginLifetimeScope())
            {
                _settings?.Hooks?.OnStartScope(scope);

                var dataLayerLogic = scope.Resolve<IDataLayerLogic>();
                dataLayers.SetRelationsForDefaultDataLayers(dataLayerLogic);

                // Load the default extents  

                // Performs the bootstrap  
                var paths =
                    new Bootstrapper.FilePaths()
                    {
                        PathPrimitive = Path.Combine(_settings.PathToXmiFiles, "PrimitiveTypes.xmi"),
                        PathUml = Path.Combine(_settings.PathToXmiFiles, "UML.xmi"),
                        PathMof = Path.Combine(_settings.PathToXmiFiles, "MOF.xmi")
                    };

                if (_settings.PerformSlimIntegration)
                {
                    throw new InvalidOperationException("Slim integration is currently not supported");
                }
                else
                {
                    Bootstrapper.PerformFullBootstrap(
                        paths,
                        workspaceCollection.GetWorkspace("UML"),
                        dataLayerLogic,
                        dataLayers.Uml);
                    Bootstrapper.PerformFullBootstrap(
                        paths,
                        workspaceCollection.GetWorkspace("MOF"),
                        dataLayerLogic,
                        dataLayers.Mof);
                }

                // Creates the workspace and extent for the types layer which are belonging to the types  
                var extentTypes = new MofUriExtent("dm:///types");
                var typeWorkspace = workspaceCollection.GetWorkspace("Types");
                typeWorkspace.AddExtent(extentTypes);
                dataLayerLogic.AssignToDataLayer(extentTypes, dataLayers.Types);

                // Boots up the typical DatenMeister Environment  
                if (_settings.EstablishDataEnvironment)
                {
                    EstablishDataEnvironment(builder, scope);
                }
            }

            watch.Stop();
            Debug.WriteLine($"Elapsed time for boostrap: {watch.Elapsed}");

            return builder;
        }

        private void EstablishDataEnvironment(IContainer builder, ILifetimeScope scope)
        {
            var innerContainer = new ContainerBuilder();
            // Loading and storing the workspaces  
            var workspaceLoader = new WorkspaceLoader(scope.Resolve<IWorkspaceCollection>(),
                "App_Data/Database/workspaces.xml");
            workspaceLoader.Load();
            innerContainer.RegisterInstance(workspaceLoader).As<WorkspaceLoader>();

            // Loading and storing the extents  
            var extentLoader = new ExtentStorageConfigurationLoader(
                scope.Resolve<ExtentStorageData>(),
                scope.Resolve<IExtentStorageLoader>(),
                "App_Data/Database/extents.xml");
            innerContainer.RegisterInstance(extentLoader).As<ExtentStorageConfigurationLoader>();

            innerContainer.Update(builder);

            // Now start the plugins  
            _settings?.Hooks?.BeforeLoadExtents(scope);

            // Loads all extents after all plugins were started  
            extentLoader.LoadAllExtents();
        }
    }
}