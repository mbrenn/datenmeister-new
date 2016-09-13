using System;
using System.Diagnostics;
using System.Linq;
using Autofac;
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
                DefaultFactoryMapper.MapFactoryType(mapper, type);
            }
        }

        public static void PerformMappingForConfigurationOfExtentLoaders(
            this ManualConfigurationToExtentStorageMapper map)
        {
            // Map configurations to extent loader  
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes());
            foreach (var type in types)
            {
                ManualConfigurationToExtentStorageMapper.MapExtentLoaderType(map, type);
            }
        }
    }
}