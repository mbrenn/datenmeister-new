using System;
using System.Collections.Generic;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Runtime.ExtentStorage
{
    public class ManualExtentStorageToConfigurationMap : IExtentStorageToConfigurationMap
    {
        /// <summary>
        /// Stores the types being used for the mapping
        /// </summary>
        private readonly Dictionary<Type, Func<IExtentStorage>> _mapping = new Dictionary<Type, Func<IExtentStorage>>();

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