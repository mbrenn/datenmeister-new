using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Autofac;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Runtime.ExtentStorage
{
    /// <summary>
    /// Maps the extent storage type to a configuration type which is used by the logic to find out the best type
    /// which can be used to satisfy a load request. 
    /// </summary>
    public class ManualConfigurationToExtentStorageMapper : IConfigurationToExtentStorageMapper
    {

        /// <summary>
        /// Stores the types being used for the mapping
        /// </summary>
        private readonly Dictionary<Type, Func<ILifetimeScope, IExtentLoader>> _mapping = new Dictionary<Type, Func<ILifetimeScope, IExtentLoader>>();

        /// <summary>
        /// Checks, if a mapping for the given configuration type exists which configures a specific extet loader
        /// </summary>
        /// <param name="typeConfiguration">Type of the configuration object, inheriting the <c>ExtentLoaderConfig</c></param>
        /// <returns>true, if mapping exists</returns>
        public bool HasMappingFor(Type typeConfiguration)
        {
            return _mapping.ContainsKey(typeConfiguration);
        }
        /// <summary>
        /// Adds the mapping by defining the type of the configuration object and the corresponding ExtentStorageLoader
        /// </summary>
        /// <param name="typeConfiguration">Type of the configuration</param>
        /// <param name="typeExtentStorage">Type of the Extent</param>
        public void AddMapping(Type typeConfiguration, Type typeExtentStorage)
        {
            _mapping[typeConfiguration] = scope => scope.Resolve(typeExtentStorage) as IExtentLoader;
        }

        public void AddMapping(Type typeConfiguration, Func<ILifetimeScope, IExtentLoader> factoryExtentStorage)
        {
            _mapping[typeConfiguration] = factoryExtentStorage;
        }

        public IExtentLoader CreateFor(ILifetimeScope scope, ExtentLoaderConfig configuration)
        {
            if (!_mapping.TryGetValue(configuration.GetType(), out Func<ILifetimeScope, IExtentLoader> foundType))
            {
                throw new InvalidOperationException("ExtentStorage for the given type was not found");
            }
            
            return foundType(scope);
        }

        public static void MapExtentLoaderType(IConfigurationToExtentStorageMapper map, Type type)
        {
            foreach (
                var customAttribute in type.GetTypeInfo().GetCustomAttributes(typeof(ConfiguredByAttribute), false))
            {
                if (customAttribute is ConfiguredByAttribute configuredByAttribute)
                {
                    map.AddMapping(configuredByAttribute.ConfigurationType,
                        scope => (IExtentLoader) scope.Resolve(type));

                    Debug.WriteLine(
                        $"Extent loader '{configuredByAttribute.ConfigurationType.Name}' configures '{type.Name}'");
                }
            }
        }
    }
}