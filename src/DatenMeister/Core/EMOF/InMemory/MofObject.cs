﻿using System;
using System.Collections.Generic;
using System.Text;
using DatenMeister.Core.EMOF.Exceptions;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.EMOF.InMemory
{
    /// <summary>
    ///     Describes the InMemory object, representing the Mof Object
    /// </summary>
    public class MofObject : IObject, IObjectAllProperties, IHasId, IObjectKnowsExtent, ICanSetId, ISetKnownExtents
    {
        /// <summary>
        /// Stores the list of extents to which this element is stored
        /// </summary>
        private readonly HashSet<IExtent> _extents = new HashSet<IExtent>();

        /// <summary>
        ///     Stores the values direct within the memory
        /// </summary>
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();
       
        public MofObject()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        
        IEnumerable<IExtent> IObjectKnowsExtent.Extents
        {
            get
            {
                lock (_extents)
                {
                    foreach (var extent in _extents)
                    {
                        yield return extent;
                    }
                }
            }
        }

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
            _values[property] = value;
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

        public void AddToExtent(IExtent extent)
        {
            lock (_extents)
            {
                _extents.Add(extent);
            }
        }

        public void RemoveFromExtent(IExtent extent)
        {
            lock (_extents)
            {
                _extents.Remove(extent);
            }
        }
    }
}