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
            public Func<ExtentManager, IProviderLoader> Function { get; set; }

            /// <summary>
            /// The connected type
            /// </summary>
            public Type ConnectedType { get; set; }

            public ConfigurationInfo(Func<ExtentManager, IProviderLoader> function, Type connectedType)
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

        public void AddMapping(Type typeConfiguration, Func<ExtentManager, IProviderLoader> factoryExtentStorage)
        {
            lock (_mapping)
            {
                var fullName = typeConfiguration.FullName;
                if (fullName == null) return;
                _mapping[fullName] =
                    new ConfigurationInfo(factoryExtentStorage, typeConfiguration);
            }
        }

        public IProviderLoader CreateFor(ExtentManager extentManager, ExtentLoaderConfig configuration)
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

                var result = foundType.Function(extentManager);
                result.WorkspaceLogic = extentManager.WorkspaceLogic;
                result.ScopeStorage = extentManager.ScopeStorage;
                
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