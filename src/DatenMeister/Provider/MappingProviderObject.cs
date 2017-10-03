﻿using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Proxies;

namespace DatenMeister.Provider
{
    public class MappingProviderObject<T> : IProviderObject
    {
        public MappingProviderObject(T value, IProvider provider, string id, string metaclassUri = null)
        {
            Value = value;
            Provider = provider;
            Id = id;
            MetaclassUri = metaclassUri;
        }

        public IProvider Provider { get; }

        public string Id { get; set; }

        public string MetaclassUri { get; set; }

        private T Value { get; }

        /// <summary>
        /// Stores the mappings
        /// </summary>
        private readonly Dictionary<string, MappingProperty> _mappings = new Dictionary<string, MappingProperty>();

        /// <summary>
        /// Adds the mapping for a property
        /// </summary>
        /// <param name="propertyName">Name of the property to be set</param>
        /// <param name="getFunction">Get function to be used</param>
        /// <param name="setFunction">Set function to be used</param>
        public void AddMapping(string propertyName, Func<T, object> getFunction, Action<T, object> setFunction)
        {
            _mappings[propertyName] = 
                new MappingProperty(
                    getFunction, 
                    setFunction);
        }

        
        /// <summary>
        /// Gets a value indicating whether the property is set
        /// </summary>
        /// <param name="property">Property to be set</param>
        /// <returns>true, if property is set</returns>
        public bool IsPropertySet(string property)
        {
            return _mappings.ContainsKey(property);
        }

        /// <summary>
        /// Gets the property
        /// </summary>
        /// <param name="property">Property to be set</param>
        /// <returns>Value to be set</returns>
        public object GetProperty(string property)
        {   
            _mappings.TryGetValue(property, out var result);

            var itemResult = result?.GetFunction(Value);
            if (DotNetHelper.IsOfEnumeration(itemResult))
            {
                var itemAsEnumerable = (IEnumerable<object>) itemResult;
                return new TemporaryReflectiveCollection(itemAsEnumerable);
            }

            return itemResult;
        }

        public IEnumerable<string> GetProperties()
        {
            return _mappings.Select(x => x.Key);
        }

        public bool DeleteProperty(string property)
        {
            var exists = _mappings.TryGetValue(property, out var result);
            result?.SetFunction(Value, null);
            return exists;
        }

        public void SetProperty(string property, object value)
        {
            _mappings.TryGetValue(property, out var result);
            result?.SetFunction(Value, value);
        }

        public void EmptyListForProperty(string property)
        {
            var result = GetProperty(property) as ICollection<object>;
            result?.Clear();
        }

        public bool AddToProperty(string property, object value, int index = -1)
        {
            var result = GetProperty(property) as ICollection<object>;
            result?.Add(value);
            return true;
        }

        public bool RemoveFromProperty(string property, object value)
        {
            var result = GetProperty(property) as ICollection<object>;
            return result?.Remove(value) == true;
        }

        /// <summary>
        /// Defines the class to define the properties
        /// </summary>
        private class MappingProperty
        {
            public MappingProperty(Func<T, object> getFunction, Action<T, object> setFunction)
            {
                GetFunction = getFunction;
                SetFunction = setFunction;
            }

            public Func<T, object> GetFunction { get; }
            public Action<T, object> SetFunction { get; }
        }
    }
}