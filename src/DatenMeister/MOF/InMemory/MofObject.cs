using DatenMeister.MOF.Exceptions;
using DatenMeister.MOF.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.MOF.InMemory
{
    /// <summary>
    /// Describes the InMemory object, representing the Mof Object
    /// </summary>
    public class MofObject : IObject
    {
        /// <summary>
        /// Stores the values direct within the memory
        /// </summary>
        private Dictionary<object, object> values = new Dictionary<object, object>();

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
    }
}
