using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;

namespace DatenMeister.Provider.DotNet
{
    /// <summary>
    /// Implements a lookup from MOF element to dotnet type and vice
    /// versa. This class is used to figure out how the mapping between 
    /// DotNet types and MOF elements is performed, so the correct type
    /// is always created
    /// </summary>
    public class DotNetTypeLookup : IDotNetTypeLookup
    {
        /// <summary>
        /// Defines a cache between all objects and their id 
        /// </summary>
        private readonly Dictionary<object, string> _cacheObjectToId = 
            new Dictionary<object, string>();

        private readonly Dictionary<IElement, Type> _elementsToTypes =
            new Dictionary<IElement, Type>();

        private readonly Dictionary<Type, IElement> _typesToElememts = 
            new Dictionary<Type, IElement>();

        /// <summary>
        /// Adds an association between type and element
        /// </summary>
        /// <param name="element">Element to be added</param>
        /// <param name="type">Type to be added</param>
        public void Add(IElement element, Type type)
        {
            if (_elementsToTypes.ContainsKey(element)
                || _typesToElememts.ContainsKey(type))
            {
                throw new InvalidOperationException("Type or element was already associated");
            }
            _elementsToTypes[element] = type;
            _typesToElememts[type] = element;
        }

        public IElement ToElement(Type type)
        {
            IElement result;
            _typesToElememts.TryGetValue(type, out result);
            return result;
        }

        public Type ToType(IElement element)
        {
            Type result;
            _elementsToTypes.TryGetValue(element, out result);
            return result;
        }

        public Type ToType(string elementUri)
        {
            foreach (var pair in _elementsToTypes)
            {
                if (pair.Key.GetUri() == elementUri)
                {
                    return pair.Value;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the id of a certain element
        /// </summary>
        /// <param name="value">Value to be queried</param>
        /// <returns>The returned id</returns>
        public string GetId(object value)
        {
            lock (_cacheObjectToId)
            {
                string id;
                if (!_cacheObjectToId.TryGetValue(value, out id))
                {
                    id = Guid.NewGuid().ToString();
                    _cacheObjectToId[value] = id;
                }

                return id;
            }
        }
    }
}