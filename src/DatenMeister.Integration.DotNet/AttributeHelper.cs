using System;
using System.Linq;
using DatenMeister.Runtime.ExtentStorage;

namespace DatenMeister.Integration.DotNet
{
    public static class AttributeHelper
    {
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