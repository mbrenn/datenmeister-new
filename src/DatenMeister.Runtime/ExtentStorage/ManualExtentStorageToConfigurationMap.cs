﻿using System;
using System.Collections.Generic;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Runtime.ExtentStorage
{
    /// <summary>
    /// Maps the extent storage type to a configuration type which is used by the logic to find out the best type
    /// which can be used to satisfy a load request. 
    /// </summary>
    public class ManualExtentStorageToConfigurationMap : IExtentStorageToConfigurationMap
    {
        /// <summary>
        /// Checks, if a mapping for the given configuration type exists which configures a specific extet loader
        /// </summary>
        /// <param name="typeConfiguration">Type of the configuration object, inheriting the <c>ExtentStorageConfiguration</c></param>
        /// <returns>true, if mapping exists</returns>
        public bool HasMappingFor(Type typeConfiguration)
        {
            return _mapping.ContainsKey(typeConfiguration);
        }

        /// <summary>
        /// Stores the types being used for the mapping
        /// </summary>
        private readonly Dictionary<Type, Func<IExtentStorage>> _mapping = new Dictionary<Type, Func<IExtentStorage>>();

        public void AddMapping(Type typeConfiguration, Type typeExtentStorage)
        {
            _mapping[typeConfiguration] = () => Activator.CreateInstance(typeExtentStorage) as IExtentStorage;
        }

        public void AddMapping(Type typeConfiguration, Func<IExtentStorage> typeExtentStorage)
        {
            _mapping[typeConfiguration] = typeExtentStorage;
        }

        public IExtentStorage CreateFor(ExtentStorageConfiguration configuration)
        {
            Func<IExtentStorage> foundType;
            if (!_mapping.TryGetValue(configuration.GetType(), out foundType))
            {
                throw new InvalidOperationException("ExtentStorage for the given type was not found");
            }
            
            return foundType();
        }
    }
}