using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Autofac;
using Autofac.Features.ResolveAnything;
using DatenMeister.DataLayer;
using DatenMeister.EMOF.Attributes;
using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.FactoryMapper;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Runtime.Workspaces.Data;
using DatenMeister.Uml;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Integration
{
    public static class Integration
    {
        public static IContainer UseDatenMeister(this ContainerBuilder kernel, IntegrationSettings settings)
        {
            if (settings == null)
            {
                Debug.WriteLine("No integration settings were given. Loading the default values.");
                settings = new IntegrationSettings();
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
                factoryMapper.PerformAutomaticMappingByAttribute(scope);
                storageMap.PerformMappingForConfigurationOfExtentLoaders(scope);

                var dataLayerLogic = scope.Resolve<IDataLayerLogic>();
                dataLayers.SetRelationsForDefaultDataLayers(dataLayerLogic);

                // Load the default extents

                // Performs the bootstrap
                var paths =
                    new Bootstrapper.FilePaths()
                    {
                        PathPrimitive = Path.Combine(settings.PathToXmiFiles, "PrimitiveTypes.xmi"),
                        PathUml = Path.Combine(settings.PathToXmiFiles, "UML.xmi"),
                        PathMof = Path.Combine(settings.PathToXmiFiles, "MOF.xmi")
                    };

                if (settings.PerformSlimIntegration)
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
                if (settings.EstablishDataEnvironment)
                {
                    EstablishDataEnvironment(builder, scope);
                }
            }

            watch.Stop();
            Debug.WriteLine($"Elapsed time for boostrap: {watch.Elapsed}");

            return builder;
        }

        private static void EstablishDataEnvironment(IContainer builder, ILifetimeScope scope)
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
            Helper.StartPlugins(scope);

            // Loads all extents after all plugins were started
            extentLoader.LoadAllExtents();
        }

        /// <summary>
        /// Stores all data that needs to be stored persistant on the hard drive
        /// This method is typically called at the end of the lifecycle of the applciation
        /// </summary>
        /// <param name="scope">Kernel to be used to find the appropriate methods</param>
        public static void UnuseDatenMeister(this ILifetimeScope scope)
        {
            scope.Resolve<WorkspaceLoader>().Store();
            scope.Resolve<ExtentStorageConfigurationLoader>().StoreAllExtents();
        }

        public static void PerformAutomaticMappingByAttribute(this DefaultFactoryMapper mapper, ILifetimeScope scope)
        {
            // Map extent types to factory
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes());
            foreach (var type in types)
            {
                foreach (
                    var customAttribute in type.GetCustomAttributes(typeof (AssignFactoryForExtentTypeAttribute), false))
                {
                    var factoryAssignmentAttribute = customAttribute as AssignFactoryForExtentTypeAttribute;
                    if (factoryAssignmentAttribute != null)
                    {
                        // TODO: We cannot use scope here. It might already be disposed
                        mapper.AddMapping(type, () => (IFactory)scope.Resolve(factoryAssignmentAttribute.FactoryType));

                        Debug.WriteLine($"Assigned extent type '{type.FullName}' to '{factoryAssignmentAttribute.FactoryType}'");
                    }
                }
            }
        }

        public static void PerformMappingForConfigurationOfExtentLoaders(this ManualConfigurationToExtentStorageMapper map, ILifetimeScope scope)
        {
                // Map configurations to extent loader
                var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes());
            foreach (var type in types)
            {
                foreach (
                    var customAttribute in type.GetCustomAttributes(typeof(ConfiguredByAttribute), false))
                {
                    var configuredByAttribute = customAttribute as ConfiguredByAttribute;
                    if (configuredByAttribute != null)
                    {
                        // TODO: We cannot use scope here. It might already be disposed
                        map.AddMapping(configuredByAttribute.ConfigurationType,
                            () => (IExtentStorage) scope.Resolve(type));

                        Debug.WriteLine(
                            $"Extent loader '{configuredByAttribute.ConfigurationType}' is configured by '{type.FullName}'");
                    }
                }
            }
        }
    }
}