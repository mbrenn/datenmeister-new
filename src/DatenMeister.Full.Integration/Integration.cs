using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DatenMeister.EMOF.Attributes;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.FactoryMapper;

namespace DatenMeister.Full.Integration
{
    public static class Integration
    {
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

        public static void PerformMappingForConfigurationOfExtentLoaders(this ManualExtentStorageToConfigurationMap map)
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