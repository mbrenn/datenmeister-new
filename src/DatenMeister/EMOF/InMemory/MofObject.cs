using DatenMeister.EMOF.Exceptions;
using DatenMeister.EMOF.Interface.Reflection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatenMeister.EMOF.InMemory
{
    /// <summary>
    /// Describes the InMemory object, representing the Mof Object
    /// </summary>
    public class MofObject : IObject, IObjectExt
    {
        /// <summary>
        /// Gets or sets the guid of the element
        /// </summary>
        public Guid guid
        {
            get;
            private set;
        }

        /// <summary>
        /// Stores the values direct within the memory
        /// </summary>
        private Dictionary<object, object> values = new Dictionary<object, object>();

        public MofObject()
        {
            guid = Guid.NewGuid();
        }

        public bool equals(object other)
        {
            // Just supports class instances
            if (this == other)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object get(object property)
        {
            object result;
            if (values.TryGetValue(property, out result))
            {
                return result;
            }

            throw new MofException("Property not found: " + property.ToString());
        }

        public bool isSet(object property)
        {
            return values.ContainsKey(property);
        }

        public void set(object property, object value)
        {
            values[property] = value;
        }

        public void unset(object property)
        {
            values[property] = null;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            var komma = string.Empty;
            foreach (var pair in values)
            {
                builder.Append($"{komma}{pair.Key.ToString()} = {pair.Value.ToString()}");
                komma = ", ";
            }

            return builder.ToString();
        }

        /// <summary>
        /// Returns an enumeration with all properties which are currently set
        /// </summary>
        /// <returns>Enumeration of all objects</returns>
        public IEnumerable<object> getPropertiesBeingSet()
        {
            foreach (var key in this.values.Keys)
            {
                yield return key;
            }
        }
    }
}
