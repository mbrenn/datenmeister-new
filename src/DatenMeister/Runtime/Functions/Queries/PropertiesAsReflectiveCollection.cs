using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Functions.Queries
{
    /// <summary>
    /// Returns the properties of the given value as a reflective collection.
    /// Only the values are returned which are inside a reflective collection, so properties with
    /// multiplicity of one are not enumerated.
    /// </summary>
    public class PropertiesAsReflectiveCollection : IReflectiveCollection, IHasExtent
    {
        private readonly IObject _value;

        /// <summary>
        /// Defines the names of the properties to be parsed
        /// </summary>
        private readonly ICollection<string> _propertyNames;
        
        private List<Tuple<string, object>> _propertyValues = new List<Tuple<string, object>>();
        
        public PropertiesAsReflectiveCollection(IObject value)
        {
            _value = value;
            _propertyNames = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the PropertiesAsReflectiveCollection
        /// </summary>
        /// <param name="value">The element being reflected</param>
        /// <param name="propertyName">The property being enumerated</param>
        public PropertiesAsReflectiveCollection(IObject value, string? propertyName) : this(value)
        {
            if (propertyName != null)
            {
                _propertyNames = new[] {propertyName};
            }
        }

        public PropertiesAsReflectiveCollection(IObject value, ICollection<string> propertyNames) : this(value)
        {
            _propertyNames = propertyNames;
        }

        public IEnumerator<object> GetEnumerator()
        {
            var result = new List<object>();
            
            lock (_propertyValues)
            {
                _propertyValues.Clear();
                if (_value is IObjectAllProperties objectWithProperties)
                {
                    var propertyNames = _propertyNames;
                    if (propertyNames == null || propertyNames.Count == 0)
                    {
                        propertyNames = objectWithProperties.getPropertiesBeingSet().ToList();
                    }

                    foreach (var property in propertyNames
                        .Where(property => _value.isSet(property)))
                    {
                        if (!(_value.get(property)
                            is IReflectiveCollection valueAsCollection))
                        {
                            continue;
                        }

                        foreach (var child in valueAsCollection)
                        {
                            if (child == null) continue;

                            result.Add(child);
                            
                            _propertyValues.Add(
                                new Tuple<string, object>(property, child));
                        }
                    }
                }
            }
            
            return result.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool add(object value) => throw new NotImplementedException();

        public bool addAll(IReflectiveSequence value) => throw new NotImplementedException();

        public void clear()
        {
            throw new NotImplementedException();
        }

        public bool remove(object? value)
        {
            lock (_propertyValues)
            {
                var foundTuple = _propertyValues.FirstOrDefault(x => x.Item2.Equals(value));
                if (foundTuple == null)
                {
                    return false;
                }

                _value.getOrDefault<IReflectiveCollection>(foundTuple.Item1).remove(foundTuple.Item2);
                return true;
            }
        }

        public int size() => this.Count();

        /// <summary>
        /// Gets the extent associated to the parent extent
        /// </summary>
        public IExtent? Extent
        {
            get
            {
                lock (_propertyValues)
                {
                    return (_value as IHasExtent)?.Extent;
                }
            }
        }
    }
}