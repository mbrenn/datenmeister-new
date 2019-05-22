using System;
using System.Reflection;
using Autofac;
using BurnSystems.Logging;
using DatenMeister.Core.Plugins;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Runtime.ExtentStorage
{
    public static class AttributeLoaderForExtentStorage
    {
        private static readonly ClassLogger Logger = new ClassLogger(typeof(AttributeLoaderForExtentStorage));

        /// <summary>
        /// Goes through all loaded types and add the extentstorage mapping as configured by the ConfiguredBy attributes
        /// </summary>
        /// <param name="map"></param>
        public static void LoadAllExtentStorageConfigurationsFromAssembly(this IConfigurationToExtentStorageMapper map)
        {
            foreach (var pair in PluginManager.GetTypesOfAssemblies<ConfiguredByAttribute>())
            {
                var type = pair.Key;
                if (map.ContainsConfigurationFor(pair.Value.ConfigurationType))
                {
                    continue;
                }

                map.MapExtentLoaderType(type);
            }
        }

        public static void MapExtentLoaderType(this IConfigurationToExtentStorageMapper map, Type type)
        {
            foreach (
                var customAttribute in type.GetTypeInfo().GetCustomAttributes(typeof(ConfiguredByAttribute), false))
            {
                if (customAttribute is ConfiguredByAttribute configuredByAttribute)
                {
                    map.AddMapping(configuredByAttribute.ConfigurationType,
                        scope => (IProviderLoader) scope.Resolve(type));

                    Logger.Trace(
                        $"Extent loader '{configuredByAttribute.ConfigurationType.Name}' configures '{type.Name}'");
                }
            }
        }
    }
}