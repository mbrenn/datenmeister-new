using System;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Provider.CSV.Runtime;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Provider.Xml;

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
            public IElement ConnectedMetaClass { get; set; }

            /// <summary>
            /// Initializes a new instance of the ConfigurationInfo class
            /// </summary>
            /// <param name="function">Function to be hadnled</param>
            /// <param name="connectedMetaClass">Connected meta class</param>
            public ConfigurationInfo(Func<ExtentManager, IProviderLoader> function, IElement connectedMetaClass)
            {
                Function = function;
                ConnectedMetaClass = connectedMetaClass;
            }

            public override string ToString()
            {
                if (ConnectedMetaClass == null)
                {
                    return base.ToString() ?? "To String did not work";
                }

                return NamedElementMethods.GetFullName(ConnectedMetaClass);
            }
        }

        private static readonly ClassLogger Logger = new ClassLogger(typeof(ConfigurationToExtentStorageMapper));

        /// <summary>
        /// Stores the types being used for the mapping
        /// </summary>
        private readonly List<ConfigurationInfo> _mapping = new List<ConfigurationInfo>();

        public void AddMapping(IElement typeConfigurationClass, Func<ExtentManager, IProviderLoader> factoryExtentStorage)
        {
            lock (_mapping)
            {
                _mapping.Add(
                    new ConfigurationInfo(factoryExtentStorage, typeConfigurationClass));
            }
        }

        public IProviderLoader CreateFor(ExtentManager extentManager, IElement configuration)
        {
            lock (_mapping)
            {
                var metaClass = configuration.getMetaClass()
                                ?? throw new InvalidOperationException("MetaClass of configuration is not set");

                var found = _mapping.FirstOrDefault(x => x.ConnectedMetaClass.@equals(metaClass));
                
                if (found == null)
                {
                    Logger.Error(
                        $"ExtentStorage for the given type was not found:  {NamedElementMethods.GetFullName(metaClass)}");
                    throw new InvalidOperationException(
                        $"ExtentStorage for the given type was not found:  {NamedElementMethods.GetFullName(metaClass)}");
                }

                var result = found.Function(extentManager);
                result.WorkspaceLogic = extentManager.WorkspaceLogic;
                result.ScopeStorage = extentManager.ScopeStorage;
                return result;
            }
        }

        public bool ContainsConfigurationFor(IElement typeConfiguration)
        {
            lock (_mapping)
            {
                return _mapping.Any(x=>x.ConnectedMetaClass.@equals(typeConfiguration));
            }
        }

        public IEnumerable<IElement> ConfigurationMetaClasses
        {
            get
            {
                lock (_mapping)
                {
                    return _mapping.Select(x=>x.ConnectedMetaClass).ToList();
                }
            }
        }

        public static ConfigurationToExtentStorageMapper GetDefaultMapper()
        {
            var result = new ConfigurationToExtentStorageMapper();
            result.AddMapping(_DatenMeister.TheOne.ExtentLoaderConfigs.__InMemoryLoaderConfig, manager => new InMemoryProviderLoader());
            result.AddMapping(_DatenMeister.TheOne.ExtentLoaderConfigs.__CsvExtentLoaderConfig, manager => new CsvProviderLoader());
            result.AddMapping(_DatenMeister.TheOne.ExtentLoaderConfigs.__XmiStorageLoaderConfig, manager => new XmiProviderLoader());
            result.AddMapping(_DatenMeister.TheOne.ExtentLoaderConfigs.__XmlReferenceLoaderConfig, manager => new XmlReferenceLoader());
            return result;
        }
    }
}