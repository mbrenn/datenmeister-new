﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Provider.DotNet
{
    public class DotNetProviderObject : IProviderObject
    {
        private readonly IDotNetTypeLookup _typeLookup;

        private readonly object _value;

        /// <summary>
        /// Gets the native value of the given element
        /// </summary>
        /// <returns></returns>
        public object GetNativeValue() => _value;

        public IProvider Provider { get; }

        /// <summary>
        /// Stores the type of the value
        /// </summary>
        private readonly Type _type;

        /// <inheritdoc />
        public string MetaclassUri { get; set; }

        public string Id { get; set; }

        /// <summary>
        /// Initializes a new instance of the DotNetElement class. 
        /// </summary>
        /// <param name="provider">The Dotnet Provider storing the items</param>
        /// <param name="typeLookup">Typelookup to be used to create element</param>
        /// <param name="value">Value to be set</param>
        /// <param name="metaClassUri">metaclass to be set to the object</param>
        public DotNetProviderObject(DotNetProvider provider, IDotNetTypeLookup typeLookup, object value, string metaClassUri)
        {
            Debug.Assert(metaClassUri != null, "type != null");
            Debug.Assert(value != null, "value != null");
            Provider = provider;
            _typeLookup = typeLookup;
            _value = value;
            MetaclassUri = metaClassUri;
            _type = value.GetType();

            Id = typeLookup.GetId(value);
        }

        /// <inheritdoc />
        public bool IsPropertySet(string property)
        {
            return _type.GetProperty(property) != null;
        }

        /// <inheritdoc />
        public object GetProperty(string property)
        {
            var result = GetValueOfProperty(property);
            return _typeLookup.CreateDotNetElementIfNecessary(result, this);
        }

        private object GetValueOfProperty(string property)
        {
            var member = _type.GetProperty(property,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            if (member == null)
            {
                throw new InvalidOperationException($"Property not known '{property}'.");
            }

            var result = member.GetValue(_value);
            return result;
        }

        /// <inheritdoc />
        public IEnumerable<string> GetProperties()
        {
            var members = _type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            return members.Select(x => x.Name);
        }

        /// <inheritdoc />
        public bool DeleteProperty(string property)
        {
            SetProperty(property, null);
            return true;
        }

        /// <inheritdoc />
        public void SetProperty(string property, object value)
        {
            var member = _type.GetProperty(property);
            if (member == null)
            {
                throw new InvalidOperationException($"Property not known '{property}'.");
            }

            member.SetValue(_value, Extensions.ConvertToNative(value));
        }

        public IList GetPropertyAsList(string property)
        {
            return GetValueOfProperty(property) as IList;
        }

        /// <inheritdoc />
        public bool AddToProperty(string property, object value, int index = -1)
        {
            var list = GetPropertyAsList(property);
            if (list == null)
            {
                EmptyListForProperty(property);
                list = GetPropertyAsList(property);
            }

            return list.Add(value) != -1;
        }

        /// <inheritdoc />
        public bool RemoveFromProperty(string property, object value)
        {
            var list = GetPropertyAsList(property);
            if (list == null)
            {
                EmptyListForProperty(property);
                list = GetPropertyAsList(property);
            }

            var result = list.Contains(value);
            list.Remove(value);
            return result;
        }

        /// <inheritdoc />
        public void EmptyListForProperty(string property)
        {
            SetProperty(property, new List<object>());
        }
    }
}