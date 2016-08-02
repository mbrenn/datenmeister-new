using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Provider.DotNet
{
    public class DotNetElement : IElement, IHasId, IObjectKnowsExtent, ICanSetId, IObjectAllProperties
    {
        /// <summary>
        /// Stores the list of extents to which this element is stored
        /// </summary>
        private readonly HashSet<IExtent> _extents = new HashSet<IExtent>();

        private readonly IDotNetTypeLookup _typeLookup;

        private readonly object _value;

        /// <summary>
        /// Gets the native value of the given element
        /// </summary>
        /// <returns></returns>
        public object GetNativeValue() => _value;

        /// <summary>
        /// Stores the type of the value
        /// </summary>
        private readonly Type _type;

        private IElement _container;
        
        public DotNetElement(IDotNetTypeLookup typeLookup, object value, IElement mofType, IElement container = null)
        {
            Debug.Assert(mofType != null, "type != null");
            Debug.Assert(value != null, "value != null");
            _typeLookup = typeLookup;
            _value = value;
            metaclass = mofType;
            _type = value.GetType();

            Id = Guid.NewGuid().ToString();
            _container = container;
        }

        public bool equals(object other)
        {
            var element = other as DotNetElement;
            if (element != null)
            {
                return element._value == _value;
            }

            return false;
        }

        public object get(string property)
        {
            var member = _type.GetProperty(property,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            if (member == null)
            {
                throw new InvalidOperationException($"Property not known '{property}'.");
            }

            var result = member.GetValue(_value);
            return _typeLookup.CreateDotNetElementIfNecessary(result);
        }

        public void set(string property, object value)
        {
            var member = _type.GetProperty(property);
            if (member == null)
            {
                throw new InvalidOperationException($"Property not known '{property}'.");
            }

            member.SetValue(_value, Extensions.ConvertToNative(value));
        }

        public bool isSet(string property)
        {
            return _type.GetProperty(property) != null;

        }

        public void unset(string property)
        {
            set(property, null);
        }

        public IElement metaclass { get; private set; }

        public IElement getMetaClass() => metaclass;

        public IElement container() => _container;

        public string Id { get; set; }

        public IEnumerable<IExtent> Extents => _extents;
        
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

        /// <summary>
        /// Transfers the information about extent ownership from one element to another
        /// </summary>
        /// <param name="other">The other element from which the extents shall be transfered</param>
        public void TransferExtents(DotNetElement other)
        {
            foreach (var extent in other._extents)
            {
                AddToExtent(extent);
            }
        }

        /// <summary>
        /// Gets all properties that can be set
        /// </summary>
        /// <returns>Enumeration of properties</returns>
        public IEnumerable<string> getPropertiesBeingSet()
        {
            var members = _type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            return members.Select(x => x.Name);
        }

        /// <summary>
        /// Sets the container manually 
        /// </summary>
        /// <param name="owner">Container to be set</param>
        internal void SetContainer(DotNetElement owner)
        {
            _container = owner;
        }
    }
}