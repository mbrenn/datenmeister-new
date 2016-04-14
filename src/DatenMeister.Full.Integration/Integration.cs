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
using DatenMeister.Uml.Helper;
using DatenMeister.XMI.UmlBootstrap;
using Ninject;

namespace DatenMeister.Full.Integration
{
    public static class Integration
    {
        public static void UseDatenMeister(this StandardKernel kernel, string pathToXmiFiles = "App_Data")
        {
            var watch = new Stopwatch();
            watch.Start();

            var factoryMapper = new DefaultFactoryMapper();
            factoryMapper.PerformAutomaticMappingByAttribute(kernel);
            kernel.Bind<IFactoryMapper>().ToConstant(factoryMapper);

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
            // Load the primitivetypes
            var primitiveTypes = new _PrimitiveTypes();
            kernel.Bind<_PrimitiveTypes>().ToConstant(primitiveTypes);

            // Performs the bootstrap
            var paths =
                new Bootstrapper.FilePaths()
                {
                    PathPrimitive = Path.Combine(pathToXmiFiles, "PrimitiveTypes.xmi"),
                    PathUml = Path.Combine(pathToXmiFiles, "UML.xmi"),
                    PathMof = Path.Combine(pathToXmiFiles, "MOF.xmi")
                };

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

            // Creates the workspace and extent for the types layer which are belonging to the types
            var extentTypes = new MofUriExtent("dm:///types");
            var typeWorkspace = workspaceCollection.GetWorkspace("Types");
            typeWorkspace.AddExtent(extentTypes);
            dataLayerLogic.AssignToDataLayer(extentTypes, dataLayers.Types);

            kernel.Bind<IUmlNameResolution>().To<UmlNameResolution>();

            watch.Stop();
            Debug.WriteLine($"Elapsed time for boostrap: {watch.Elapsed}");
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