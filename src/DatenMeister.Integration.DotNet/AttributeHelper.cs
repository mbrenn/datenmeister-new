using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Autofac;
using DatenMeister.Core.EMOF.Attributes;
using DatenMeister.Core.EMOF.Interface.Reflection;
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
                        var factoryType = factoryAssignmentAttribute.FactoryType;
                        if (factoryType == null)
                        {
                            throw new InvalidOperationException(
                                $"FactoryType is null. Is the attribute at type {type.FullName} correctly set?");
                        }
                        mapper.AddMapping(
                            type,
                            scope =>
                            {
                                try
                                {
                                    return (IFactory) scope.Resolve(factoryType);
                                }
                                catch (Exception exc)
                                {
                                    throw new IOException(
                                        $"Exception thrown during creation of {factoryType.Name}",
                                        exc);
                                }
                            });

                        Debug.WriteLine($"Assigned extent type '{type.Name}' to '{factoryType.Name}'");
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
                        map.AddMapping(configuredByAttribute.ConfigurationType,
                            scope => (IExtentStorage)scope.Resolve(type));

                        Debug.WriteLine(
                            $"Extent loader '{configuredByAttribute.ConfigurationType.Name}' configures '{type.Name}'");
                    }
                }
            }
        }
    }
}