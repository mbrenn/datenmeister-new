#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DatenMeister.Provider.DotNet
{
    public class DotNetProviderObject : IProviderObject
    {
        private readonly object _value;

        /// <summary>
        /// Gets the native value of the given element
        /// </summary>
        /// <returns></returns>
        public object GetNativeValue() => _value;

        /// <summary>
        /// Gets the provider as DotNetProvider object
        /// </summary>
        public DotNetProvider Provider { get; }

        /// <summary>
        /// Gets the provider for the interface
        /// </summary>
        IProvider IProviderObject.Provider => Provider;

        /// <summary>
        /// Stores the type of the value
        /// </summary>
        private readonly Type _type;

        /// <inheritdoc />
        public string? MetaclassUri { get; set; } 

        /// <summary>
        /// Gets or sets the id of the object of the DotNetProviderObject
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Initializes a new instance of the DotNetElement class.
        /// </summary>
        /// <param name="provider">The Dotnet Provider storing the items</param>
        /// <param name="value">Value to be set</param>
        /// <param name="metaClassUri">metaclass to be set to the object</param>
        public DotNetProviderObject(DotNetProvider provider, object value, string? metaClassUri = null)
        {
            Provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _value = value ?? throw new ArgumentNullException(nameof(value));
            
            // Gets the metaclass uri if explicitly given, otherwise look up into the types
            MetaclassUri = metaClassUri ?? Provider.TypeLookup.ToElement(value.GetType());
            _type = value.GetType();

            Id = provider.TypeLookup.GetId(value);
        }

        /// <inheritdoc />
        public bool IsPropertySet(string property) =>
            _type.GetProperty(property) != null;

        /// <inheritdoc />
        public object? GetProperty(string property)
        {
            var result = GetValueOfProperty(property);
            return Provider.CreateDotNetElementIfNecessary(result);
        }

        private object? GetValueOfProperty(string property)
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
        public void SetProperty(string property, object? value)
        {
            var member = _type.GetProperty(property);
            if (member == null)
            {
                throw new InvalidOperationException($"Property not known '{property}'.");
            }

            member.SetValue(_value, DotNetProviderExtensions.ConvertToNative(value));
        }

        public IList? GetPropertyAsList(string property) =>
            GetValueOfProperty(property) as IList;

        /// <inheritdoc />
        public bool AddToProperty(string property, object value, int index = -1)
        {
            if (value == null) return false;
            
            var list = GetPropertyAsList(property);
            if (list == null)
            {
                EmptyListForProperty(property);
                list = GetPropertyAsList(property);
            }

            return list!.Add(DotNetProviderExtensions.ConvertToNative(value)) != -1;
        }

        /// <inheritdoc />
        public bool RemoveFromProperty(string property, object value)
        {
            if (value == null) return false;
            
            var list = GetPropertyAsList(property);
            if (list == null)
            {
                EmptyListForProperty(property);
                list = GetPropertyAsList(property);
            }

            var newValue = DotNetProviderExtensions.ConvertToNative(value);
            if (newValue == null) return false;

            var result = list!.Contains(newValue);
            list.Remove(newValue);
            return result;
        }

        public bool HasContainer() =>
            false;

        public IProviderObject? GetContainer() =>
            null;

        public void SetContainer(IProviderObject? value)
        {
        }

        /// <inheritdoc />
        public void EmptyListForProperty(string property)
        {
            SetProperty(property, new List<object>());
        }
    }
}