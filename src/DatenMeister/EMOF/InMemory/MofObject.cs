using System;
using System.Collections.Generic;
using System.Text;
using DatenMeister.EMOF.Exceptions;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.EMOF.InMemory
{
    /// <summary>
    ///     Describes the InMemory object, representing the Mof Object
    /// </summary>
    public class MofObject : IObject, IObjectAllProperties, IHasId
    {
        /// <summary>
        ///     Stores the values direct within the memory
        /// </summary>
        private readonly Dictionary<object, object> _values = new Dictionary<object, object>();

        /// <summary>
        /// Stores the extent into which the element is stored
        /// </summary>
        public IExtent Extent { get; set; }

        public MofObject()
        {
            guid = Guid.NewGuid();
        }

        /// <summary>
        ///     Gets or sets the guid of the element
        /// </summary>
        public Guid guid { get; private set; }

        object IHasId.Id => guid;

        public bool equals(object other)
        {
            // Just supports class instances
            if (this == other)
            {
                return true;
            }

            return false;
        }

        public object get(object property)
        {
            object result;
            if (_values.TryGetValue(property, out result))
            {
                return result;
            }

            throw new MofException("Property not found: " + property);
        }

        public bool isSet(object property)
        {
            return _values.ContainsKey(property);
        }

        public void set(object property, object value)
        {
            _values[property] = value;
        }

        public void unset(object property)
        {
            _values.Remove(property);
        }

        /// <summary>
        ///     Returns an enumeration with all properties which are currently set
        /// </summary>
        /// <returns>Enumeration of all objects</returns>
        public IEnumerable<object> getPropertiesBeingSet()
        {
            return _values.Keys;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append($"#{guid} - ");

            var komma = string.Empty;
            foreach (var pair in _values)
            {
                builder.Append($"{komma}{pair.Key} = {pair.Value}");
                komma = ", ";
            }

            return builder.ToString();
        }
    }
}