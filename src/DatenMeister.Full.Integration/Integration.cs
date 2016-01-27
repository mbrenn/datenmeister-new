using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using DatenMeister.EMOF.Attributes;
using DatenMeister.Runtime;
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
        public static void FillForDatenMeister(this StandardKernel kernel)
        {
            // Do the full load
            Helper.LoadAllReferenceAssemblies();
            
            var factoryMapper = new DefaultFactoryMapper();
            factoryMapper.PerformAutomaticMappingByAttribute();
            kernel.Bind<IFactoryMapper>().ToConstant(factoryMapper);

            var storageMap = new ManualConfigurationToExtentStorageMapper();
            storageMap.PerformMappingForConfigurationOfExtentLoaders();
            kernel.Bind<IConfigurationToExtentStorageMapper>().ToConstant(storageMap);

            // Workspace collection
            var workspaceCollection = new WorkspaceCollection();
            workspaceCollection.Init();
            kernel.Bind<IWorkspaceCollection>().ToConstant(workspaceCollection);

            // Defines the extent storage data
            var extentStorageData = new ExtentStorageData();
            kernel.Bind<ExtentStorageData>().ToConstant(extentStorageData);
            kernel.Bind<IExtentStorageLogic>().To<ExtentStorageLogic>();

            // Load the default extents
            // Load the primitivetypes
            var primitiveTypes = new _PrimitiveTypes();
            kernel.Bind<_PrimitiveTypes>().ToConstant(primitiveTypes);

            var strapper = Bootstrapper.PerformFullBootstrap("App_Data/PrimitiveTypes.xmi", "App_Data/UML.xmi", "App_Data/MOF.xmi");
            var metaWorkspace = workspaceCollection.GetWorkspace("Meta");
            metaWorkspace.AddExtent(strapper.PrimitiveInfrastructure);
            metaWorkspace.AddExtent(strapper.MofInfrastructure);
            metaWorkspace.AddExtent(strapper.UmlInfrastructure);

            kernel.Bind<IUmlNameResolution>().To<UmlNameResolution>();
        }

        public static void PerformAutomaticMappingByAttribute(this DefaultFactoryMapper mapper)
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
                        mapper.AddMapping(type, factoryAssignmentAttribute.FactoryType);

                        Debug.WriteLine($"Assigned extent type '{type.FullName}' to '{factoryAssignmentAttribute.FactoryType}'");
                    }
                }
            }
        }

        public static void PerformMappingForConfigurationOfExtentLoaders(this ManualConfigurationToExtentStorageMapper map)
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
                        map.AddMapping(configuredByAttribute.ConfigurationType, type);

                        Debug.WriteLine($"Extent loader '{configuredByAttribute.ConfigurationType}' is configured by '{type.FullName}'");
                    }
                }
            }
        }
    }
}