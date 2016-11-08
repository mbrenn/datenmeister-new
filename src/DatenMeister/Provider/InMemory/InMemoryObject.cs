using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatenMeister.Core.EMOF.Exceptions;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.DotNet;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;

namespace DatenMeister.Provider.InMemory
{
    /// <summary>
    ///     Describes the InMemory object, representing the Mof Object
    /// </summary>
    public class InMemoryObject : IObject, IObjectAllProperties, IHasId, IHasExtent, ICanSetId, ISetExtent
    {
        /// <summary>
        /// Stores the list of extents to which this element is stored
        /// </summary>
        private IExtent _extent;

        /// <summary>
        ///     Stores the values direct within the memory
        /// </summary>
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();
       
        public InMemoryObject()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        
        IExtent IHasExtent.Extent => _extent;

        public bool equals(object other)
        {
            // Just supports class instances
            if (this == other)
            {
                return true;
            }

            return false;
        }

        public virtual object get(string property)
        {
            object result;
            if (_values.TryGetValue(property, out result))
            {
                return result;
            }

            throw new MofException("Property not found: " + property);
        }

        public virtual bool isSet(string property)
        {
            return _values.ContainsKey(property);
        }

        public virtual void set(string property, object value)
        {
            var result = ConvertToInMemoryElement(value, this.GetUriExtentOf());

            _values[property] = result;
        }

        

        public virtual void unset(string property)
        {
            _values.Remove(property);
        }

        /// <summary>
        ///     Returns an enumeration with all properties which are currently set
        /// </summary>
        /// <returns>Enumeration of all objects</returns>
        public virtual IEnumerable<string> getPropertiesBeingSet()
        {
            return _values.Keys;
        }

        public override string ToString()
        {
            if (isSet("name"))
            {
                return get("name").ToString();
            }

            var builder = new StringBuilder();
            builder.Append($"#{Id} - ");

            var komma = string.Empty;
            foreach (var pair in _values)
            {
                var key = pair.Key;
                if (key != null)
                {
                    builder.Append($"{komma}{key} = {pair.Value}");
                }
                else
                {
                    builder.Append($"{komma}Prop = {pair.Value}");
                }

                komma = ", ";
            }

            return builder.ToString();
        }

        public void SetExtent(IExtent extent)
        {
            _extent = extent;
        }

        /// <summary>
        /// Converts the stated element into a memoery element
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="localExtent">Defines the uri for which the element is copied</param>
        /// <returns>Converted element</returns>
        public static object ConvertToInMemoryElement(object value, IUriExtent localExtent)
        {
            if (DotNetHelper.IsOfEnumeration(value) && !(value is InMemoryReflectiveSequence))
            {
                return new InMemoryReflectiveSequence(localExtent, (value as IEnumerable<object>).ToList());
            }

            if (DotNetHelper.IsOfMofObject(value))
            {
                var valueAsObject = (IObject)value;
                var extentOfValue = valueAsObject.GetUriExtentOf();
                if (localExtent != null && extentOfValue?.contextURI() == localExtent.contextURI())
                {
                    return valueAsObject;
                }
                if (extentOfValue == null && value is InMemoryObject)
                {
                    // Object does not have an extent until now... So get the ownership
                    (value as ISetExtent)?.SetExtent(localExtent);
                    return valueAsObject;
                }

                // Ok, it is not local, so convert it
                var result = ObjectCopier.Copy(new InMemoryFactory(), (IObject) value);
                ((ISetExtent) result).SetExtent(localExtent);
                return result;
            }

            return value;
        }
    }
}