using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Autofac;
using DatenMeister.Core.EMOF.Attributes;
using DatenMeister.Core.EMOF.Interface.Reflection;

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

        public static void MapFactoryType(IFactoryMapper mapper, Type type)
        {
            foreach (
                var customAttribute in type.GetTypeInfo().GetCustomAttributes(typeof(AssignFactoryForExtentTypeAttribute), false))
            {
                var factoryAssignmentAttribute = customAttribute as AssignFactoryForExtentTypeAttribute;
                if (factoryAssignmentAttribute != null)
                {
                    var factoryType = factoryAssignmentAttribute.FactoryType;
                    if (factoryType == null)
                    {
                        throw new InvalidOperationException(
                            $"FactoryType is null. Is the attribute at type {type.FullName} correctly set?");
                    }
                    mapper.AddMapping(
                        type,
                        scope =>
                        {
                            try
                            {
                                return (IFactory) scope.Resolve(factoryType);
                            }
                            catch (Exception exc)
                            {
                                throw new IOException(
                                    $"Exception thrown during creation of {factoryType.Name}",
                                    exc);
                            }
                        });

                    Debug.WriteLine($"Assigned extent type '{type.Name}' to '{factoryType.Name}'");
                }
            }
        }
    }
}