using System;
using Autofac;
using DatenMeister.Runtime.ExtentStorage.Configuration;

namespace DatenMeister.Runtime.ExtentStorage.Interfaces
{
    /// <summary>
    /// Maps the extent storage type to a configuration type which is used by the logic to find out the best type
    /// which can be used to satisfy a load request. 
    /// </summary>
    public interface IConfigurationToExtentStorageMapper
    {
        /// <summary>
        /// Adds a mapping of a certain type of configuration and declares a function how to create it.
        /// </summary>
        /// <param name="typeConfiguration">Type of the configuration</param>
        /// <param name="factoryExtentStorage">The function which creates the corresponsing extent to the function</param>
        void AddMapping(Type typeConfiguration, Func<ILifetimeScope, IExtentStorage> factoryExtentStorage);

        IExtentStorage CreateFor(ILifetimeScope scope, ExtentStorageConfiguration configuration);
    }
}