using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using BurnSystems.Logging;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Runtime.ExtentStorage
{
    /// <summary>
    /// Maps the extent storage type to a configuration type which is used by the logic to find out the best type
    /// which can be used to satisfy a load request.
    /// </summary>
    public class ConfigurationToExtentStorageMapper
    {
        /// <summary>
        /// Stores the configuration information
        /// </summary>
        private class ConfigurationInfo
        {
            /// <summary>
            /// The function to create the provider loader
            /// </summary>
            public Func<ILifetimeScope?, IProviderLoader?> Function { get; set; }

            /// <summary>
            /// The connected type
            /// </summary>
            public Type ConnectedType { get; set; }

            public ConfigurationInfo(Func<ILifetimeScope?, IProviderLoader?> function, Type connectedType)
            {
                Function = function;
                ConnectedType = connectedType;
            }
        }

        private static readonly ClassLogger Logger = new ClassLogger(typeof(ConfigurationToExtentStorageMapper));

        /// <summary>
        /// Stores the types being used for the mapping
        /// </summary>
        private readonly Dictionary<string, ConfigurationInfo> _mapping = new Dictionary<string, ConfigurationInfo>();

        /// <summary>
        /// Checks, if a mapping for the given configuration type exists which configures a specific extet loader
        /// </summary>
        /// <param name="typeConfiguration">Type of the configuration object, inheriting the <c>ExtentLoaderConfig</c></param>
        /// <returns>true, if mapping exists</returns>
        public bool HasMappingFor(Type typeConfiguration)
        {
            lock (_mapping)
            {
                return _mapping.ContainsKey(typeConfiguration.FullName ??
                                            throw new ArgumentNullException(nameof(typeConfiguration) + ".FullName"));
            }
        }

        /// <summary>
        /// Adds the mapping by defining the type of the configuration object and the corresponding ExtentStorageLoader
        /// </summary>
        /// <param name="typeConfiguration">Type of the configuration</param>
        /// <param name="typeExtentStorage">Type of the Extent</param>
        public void AddMapping(Type typeConfiguration, Type typeExtentStorage)
        {
            lock (_mapping)
            {
                var fullName = typeConfiguration.FullName;
                if (fullName == null) return;
                _mapping[fullName] =
                    new ConfigurationInfo(scope =>
                        {
                            if (scope == null) throw new ArgumentException(nameof(scope));
                            return scope.Resolve(typeExtentStorage) as IProviderLoader;
                        },
                        typeConfiguration);
            }
        }

        public void AddMapping(Type typeConfiguration, Func<ILifetimeScope?, IProviderLoader?> factoryExtentStorage)
        {
            lock (_mapping)
            {
                var fullName = typeConfiguration.FullName;
                if (fullName == null) return;
                _mapping[fullName] =
                    new ConfigurationInfo(factoryExtentStorage, typeConfiguration);
            }
        }

        public IProviderLoader CreateFor(ILifetimeScope? scope, ExtentLoaderConfig configuration)
        {
            lock (_mapping)
            {
                if (!_mapping.TryGetValue(configuration.GetType().FullName, out var foundType))
                {
                    Logger.Error(
                        $"ExtentStorage for the given type was not found:  {configuration.GetType().FullName}");
                    throw new InvalidOperationException(
                        $"ExtentStorage for the given type was not found:  {configuration.GetType().FullName}");
                }

                var result = foundType.Function(scope);
                if (result == null)
                {
                    throw new InvalidOperationException("Converter return a null provider");
                }

                return result;
            }
        }

        public bool ContainsConfigurationFor(Type typeConfiguration)
        {
            lock (_mapping)
            {
                return _mapping.ContainsKey(typeConfiguration.FullName);
            }
        }

        public IEnumerable<Type> ConfigurationTypes
        {
            get
            {
                lock (_mapping)
                {
                    return _mapping.Values.Select(x => x.ConnectedType);
                }
            }
        }
    }
}