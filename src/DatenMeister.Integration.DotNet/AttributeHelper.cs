using System;
using System.Diagnostics;
using System.Linq;
using DatenMeister.EMOF.Attributes;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.FactoryMapper;

namespace DatenMeister.Integration.DotNet
{
    public static class AttributeHelper
    {
        public static void PerformAutomaticMappingByAttribute(this DefaultFactoryMapper mapper)
        {
            // Map extent types to factory  
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes());
            foreach (var type in types)
            {
                foreach (
                    var customAttribute in type.GetCustomAttributes(typeof(AssignFactoryForExtentTypeAttribute), false))
                {
                    var factoryAssignmentAttribute = customAttribute as AssignFactoryForExtentTypeAttribute;
                    if (factoryAssignmentAttribute != null)
                    {
                        // TODO: We cannot use scope here. It might already be disposed  
                        mapper.AddMapping(
                            type,
                            () => (IFactory)Activator.CreateInstance(factoryAssignmentAttribute.FactoryType));

                        Debug.WriteLine($"Assigned extent type '{type.FullName}' to '{factoryAssignmentAttribute.FactoryType}'");
                    }
                }
            }
        }

        public static void PerformMappingForConfigurationOfExtentLoaders(
            this ManualConfigurationToExtentStorageMapper map)
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
                            () => (IExtentStorage)Activator.CreateInstance(type));

                        Debug.WriteLine(
                            $"Extent loader '{configuredByAttribute.ConfigurationType}' is configured by '{type.FullName}'");
                    }
                }
            }
        }
    }
}