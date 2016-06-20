using System;
using System.Collections.Generic;
using Autofac;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.FactoryMapper
{
    /// <summary>
    /// Maps the type of an extent to a factory function, which creates a factor for the given extent
    /// </summary>
    public class DefaultFactoryMapper : IFactoryMapper
    {
        /// <summary>
        /// Stores the mapping
        /// </summary>
        private readonly Dictionary<Type, Func<ILifetimeScope, IFactory>> _mapping = 
            new Dictionary<Type, Func<ILifetimeScope, IFactory>>(); 

        /// <summary>
        /// Adds a mapping of the type of an extent to the factory
        /// </summary>
        /// <param name="type">Type of the extent</param>
        /// <param name="createFunc">Creation function</param>
        public void AddMapping(Type type, Func<ILifetimeScope, IFactory> createFunc)
        {
            if (_mapping.ContainsKey(type))
            {
                throw new InvalidOperationException($"Type {type} is already included in mapping");
            }

            _mapping[type] = createFunc;
        }

        public void AddMapping(Type type, Type factoryType)
        {
            AddMapping(type, scope => scope.Resolve(factoryType) as IFactory);
        }

        public bool HasMappingForExtentType(Type type)
        {
            return _mapping.ContainsKey(type);
        }

        public IFactory FindFactoryFor(ILifetimeScope scope, Type extentType)
        {
            Func<ILifetimeScope, IFactory> result;

            if (!_mapping.TryGetValue(extentType, out result))
            {
                throw new InvalidOperationException($"No factory define for extenttype '{extentType}'.");
            }

            return result(scope);
        }
    }
}