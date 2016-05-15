using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
using Ninject;

namespace DatenMeister.Integration
{
    public static class Integration
    {
        public static void UseDatenMeister(this StandardKernel kernel, IntegrationSettings settings)
        {
            if (settings == null)
            {
                Debug.WriteLine("No integration settings were given. Loading the default values.");
                settings = new IntegrationSettings();
            }

            var watch = new Stopwatch();
            watch.Start();

            // Defines the factory method for a certain extent type
            var factoryMapper = new DefaultFactoryMapper();
            factoryMapper.PerformAutomaticMappingByAttribute(kernel);
            kernel.Bind<IFactoryMapper>().ToConstant(factoryMapper);

            // Finds the loader for a certain extent type
            var storageMap = new ManualConfigurationToExtentStorageMapper();
            storageMap.PerformMappingForConfigurationOfExtentLoaders(kernel);
            kernel.Bind<IConfigurationToExtentStorageMapper>().ToConstant(storageMap);

            // Workspace collection
            var workspaceCollection = new WorkspaceCollection();
            workspaceCollection.Init();
            kernel.Bind<IWorkspaceCollection>().ToConstant(workspaceCollection);

            // Defines the extent storage data
            var extentStorageData = new ExtentStorageData();
            kernel.Bind<ExtentStorageData>().ToConstant(extentStorageData);
            kernel.Bind<IExtentStorageLoader>().To<ExtentStorageLoader>();

            // Defines the datalayers
            var dataLayers = new DataLayers();
            kernel.Bind<DataLayers>().ToConstant(dataLayers);

            var dataLayerData = new DataLayerData(dataLayers);
            kernel.Bind<DataLayerData>().ToConstant(dataLayerData);
            kernel.Bind<IDataLayerLogic>().To<DataLayerLogic>();

            var dataLayerLogic = kernel.Get<IDataLayerLogic>();
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

            kernel.Bind<IUmlNameResolution>().To<UmlNameResolution>();

            // Boots up the typical DatenMeister Environment
            if (settings.EstablishDataEnvironment)
            {
                EstablishDataEnvironment(kernel);
            }

            watch.Stop();
            Debug.WriteLine($"Elapsed time for boostrap: {watch.Elapsed}");
        }

        private static void EstablishDataEnvironment(StandardKernel kernel)
        {
            // Loading and storing the workspaces
            var workspaceLoader = new WorkspaceLoader(kernel.Get<IWorkspaceCollection>(),
                "App_Data/Database/workspaces.xml");
            workspaceLoader.Load();
            kernel.Bind<WorkspaceLoader>().ToConstant(workspaceLoader);

            // Loading and storing the extents
            var extentLoader = new ExtentStorageConfigurationLoader(
                kernel.Get<ExtentStorageData>(),
                kernel.Get<IExtentStorageLoader>(),
                "App_Data/Database/extents.xml");
            kernel.Bind<ExtentStorageConfigurationLoader>().ToConstant(extentLoader);

            // Now start the plugins
            Helper.StartPlugins(kernel);

            // Loads all extents after all plugins were started
            extentLoader.LoadAllExtents();
        }

        /// <summary>
        /// Stores all data that needs to be stored persistant on the hard drive
        /// This method is typically called at the end of the lifecycle of the applciation
        /// </summary>
        /// <param name="kernel">Kernel to be used to find the appropriate methods</param>
        public static void UnuseDatenMeister(this StandardKernel kernel)
        {
            kernel.Get<WorkspaceLoader>().Store();
            kernel.Get<ExtentStorageConfigurationLoader>().StoreAllExtents();
        }

        public static void PerformAutomaticMappingByAttribute(this DefaultFactoryMapper mapper, StandardKernel kernel)
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
                        mapper.AddMapping(type, () => (IFactory)kernel.Get(factoryAssignmentAttribute.FactoryType));

                        Debug.WriteLine($"Assigned extent type '{type.FullName}' to '{factoryAssignmentAttribute.FactoryType}'");
                    }
                }
            }
        }

        public static void PerformMappingForConfigurationOfExtentLoaders(this ManualConfigurationToExtentStorageMapper map, StandardKernel kernel)
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
                        map.AddMapping(configuredByAttribute.ConfigurationType, () => (IExtentStorage) kernel.Get(type));

                        Debug.WriteLine($"Extent loader '{configuredByAttribute.ConfigurationType}' is configured by '{type.FullName}'");
                    }
                }
            }
        }
    }
}