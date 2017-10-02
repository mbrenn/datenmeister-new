using System;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Implementation;

namespace DatenMeister.Provider.DotNet
{
    /// <summary>
    /// Implements a lookup from MOF element to dotnet type and vice
    /// versa. This class is used to figure out how the mapping between 
    /// DotNet types and MOF elements is performed, so the correct type
    /// is always created
    /// </summary>
    internal class DotNetTypeLookup : IDotNetTypeLookup
    {
        private readonly MofExtent _extent;

        /// <summary>
        /// Defines a cache between all objects and their id 
        /// </summary>
        private readonly Dictionary<object, string> _cacheObjectToId = 
            new Dictionary<object, string>();

        private readonly Dictionary<string, Type> _elementsToTypes =
            new Dictionary<string, Type>();

        /// <summary>
        /// Stores a mapping between a .Net Type and the guid being used within the extent
        /// </summary>
        private readonly Dictionary<Type, string> _typesToElememts = 
            new Dictionary<Type, string>();

        public DotNetTypeLookup(MofExtent extent)
        {
            _extent = extent;
        }

        /// <summary>
        /// Adds an association between type and element
        /// </summary>
        /// <param name="element">Element to be added</param>
        /// <param name="type">Type to be added</param>
        public void Add(string element, Type type)
        {
            if (_elementsToTypes.ContainsKey(element)
                || _typesToElememts.ContainsKey(type))
            {
                throw new InvalidOperationException("Type or element was already associated");
            }
            _elementsToTypes[element] = type;
            _typesToElememts[type] = element;
        }

        public string ToElement(Type type)
        {
            _typesToElememts.TryGetValue(type, out var result);
            return result;
        }

        /// <inheritdoc />
        public Type ToType(string element)
        {
            _elementsToTypes.TryGetValue(element, out var result);
            return result;
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