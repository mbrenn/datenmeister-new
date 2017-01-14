using System;
using System.Collections.Generic;
using System.Text;
using DatenMeister.Core.EMOF.Exceptions;
using DatenMeister.Core.EMOF.Implementation;

namespace DatenMeister.Provider.InMemory
{
    /// <summary>
    ///     Describes the InMemory object, representing the Mof Object
    /// </summary>
    public class InMemoryObject : IProviderObject
    {
        public IProvider Provider { get; }

        /// <summary>
        /// Creates an empty mof object that can be used to identify a specific object. All content will be stored within the InMemoryObject
        /// </summary>
        /// <returns>The created object as MofObject</returns>
        public static MofObject CreateEmpty()
        {
            var inner = new InMemoryObject(null);
            return new MofObject(inner, null);
        }

        /// <summary>
        ///     Stores the values direct within the memory
        /// </summary>
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

        /// <inheritdoc />
        public string MetaclassUri { get; set; }

        /// <summary>
        /// Initializes a new instance of the InMemoryObject.
        /// </summary>
        /// <param name="metaclassUri">Uri of the metaclass</param>
        public InMemoryObject(IProvider provider, string metaclassUri = null)
        {
            Provider = provider;
            Id = Guid.NewGuid().ToString();
            MetaclassUri = metaclassUri;
        }

        internal static void CheckValue ( object value)
        {
            if (value is MofReflectiveSequence || value is MofObject)
            {
                throw new InvalidOperationException($"Value is of type {value.GetType()}");
            }
        }

        public string Id { get; set; }

        /// <summary>
        /// Gets the property of the element or returns an exception, if the property could not be found
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public object GetProperty(string property)
        {
            object result;
            if (_values.TryGetValue(property, out result))
            {
                return result;
            }

            throw new MofException("Property not found: " + property);
        }

        public bool IsPropertySet(string property)
        {
            return _values.ContainsKey(property);
        }

        public void SetProperty(string property, object value)
        {
            CheckValue(value);
            _values[property] = value;
        }

        public bool DeleteProperty(string property)
        {
            return _values.Remove(property);
        }

        /// <summary>
        ///     Returns an enumeration with all properties which are currently set
        /// </summary>
        /// <returns>Enumeration of all objects</returns>
        public IEnumerable<string> GetProperties()
        {
            return _values.Keys;
        }

        public override string ToString()
        {
            if (IsPropertySet("name"))
            {
                return GetProperty("name").ToString();
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

        /// <inheritdoc />
        public bool AddToProperty(string property, object value, int index)
        {
            CheckValue(value);
            var result = GetListOfProperty(property);

            if (index == -1)
            {
                result.Add(value);
            }
            else
            {
                result.Insert(index, value);
            }
            
            return true;
        }

        private List<object> GetListOfProperty(string property)
        {
            List<object> result = null;
            if (_values.ContainsKey(property))
            {
                result = _values[property] as List<object>;
            }

            if (result == null)
            {
                result = new List<object>();
            }
            return result;
        }

        /// <inheritdoc />
        public bool RemoveFromProperty(string property, object value)
        {
            CheckValue(value);
            var result = GetListOfProperty(property);
            return result.Remove(value);
        }

        /// <inheritdoc />
        public void EmptyListForProperty(string property)
        {
            _values[property] = new List<object>();
        }
    }
}