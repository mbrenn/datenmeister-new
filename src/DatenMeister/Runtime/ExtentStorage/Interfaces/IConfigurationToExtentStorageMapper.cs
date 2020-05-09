﻿using System;
using System.Collections.Generic;
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
        /// <param name="factoryExtentStorage">The function which creates the corresponding extent to the function</param>
        void AddMapping(Type typeConfiguration, Func<ILifetimeScope, IProviderLoader> factoryExtentStorage);

        IProviderLoader CreateFor(ILifetimeScope scope, ExtentLoaderConfig configuration);

        /// <summary>
        /// Checks, if there is already a handler for the given configuration type
        /// </summary>
        /// <param name="typeConfiguration">Type of the configuration</param>
        /// <returns>True, if the mapper is already included</returns>
        bool ContainsConfigurationFor(Type typeConfiguration);

        /// <summary>
        /// Gets an enumeration of configurations
        /// </summary>
        IEnumerable<Type> ConfigurationTypes { get; }
    }
}