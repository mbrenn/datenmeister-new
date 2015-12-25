using System;
using System.Collections.Generic;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Runtime
{
    public class ManualExtentStorageToConfigurationMap : IExtentStorageToConfigurationMap
    {
        /// <summary>
        /// Stores the types being used for the mapping
        /// </summary>
        private readonly Dictionary<Type, Type> _mapping = new Dictionary<Type, Type>();

        public void AddMapping(Type typeConfiguration, Type typeExtentStorage)
        {
            _mapping[typeConfiguration] = typeExtentStorage;
        }

        public IExtentStorage CreateFor(ExtentStorageConfiguration configuration)
        {
            Type foundType;
            if (!_mapping.TryGetValue(configuration.GetType(), out foundType))
            {
                throw new InvalidOperationException("ExtentStorage for the given type was not found");
            }

            var createdInstance = Activator.CreateInstance(foundType);
            return createdInstance as IExtentStorage;
        }
    }
}