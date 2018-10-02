using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Implements a reflective sequence as given by the MOF specification. 
    /// The sequence needs to be correlated to a Mof Object
    /// </summary>
    public class MofReflectiveSequence : IReflectiveSequence, IHasExtent
    {
        private readonly string _property;

        /// <summary>
        /// Gets the mof object being assigned to the 
        /// </summary>
        public MofObject MofObject { get; }

        public MofReflectiveSequence(MofObject mofObject, string property)
        {
            MofObject = mofObject;
            _property = property;
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public IEnumerator<object> GetEnumerator()
        {
            var result = GetPropertyAsEnumerable();
            foreach (var item in result)
            {
                yield return MofObject.ConvertToMofObject(MofObject, _property, item);
            }
        }

        /// <summary>
        /// Gets the given property as an enumerable
        /// </summary>
        /// <returns>Enumerable which was retrieved</returns>
        private IEnumerable<object> GetPropertyAsEnumerable()
        {
            if (MofObject.ProviderObject.IsPropertySet(_property))
            {
                return (IEnumerable<object>) MofObject.ProviderObject.GetProperty(_property);
            }

            return Array.Empty<object>();
        }

        /// <inheritdoc />
        public bool add(object value)
        {
            var valueToBeAdded = MofExtent.ConvertForSetting(MofObject, value);
            return MofObject.ProviderObject.AddToProperty(_property, valueToBeAdded);
        }

        /// <inheritdoc />
        public bool addAll(IReflectiveSequence value)
        {
            bool? result = null;

            foreach (var element in value)
            {
                if (result == null)
                {
                    result = add(element);
                }
                else
                {
                    result |= add(element);
                }
            }

            return result == true;
        }

        /// <inheritdoc />
        public void clear()
        {
            MofObject.ProviderObject.EmptyListForProperty(_property);
        }

        /// <inheritdoc />
        public bool remove(object value)
        {
            if (value is MofObject valueAsMofObject)
            {
                var asProviderObject = valueAsMofObject.ProviderObject;
                return MofObject.ProviderObject.RemoveFromProperty(_property, asProviderObject);
            }
            else
            {
                return MofObject.ProviderObject.RemoveFromProperty(_property, value);
            }
        }

        /// <inheritdoc />
        public int size()
        {
            return GetPropertyAsEnumerable().Count();
        }

        /// <inheritdoc />
        public void add(int index, object value)
        {
            var valueToBeAdded = MofExtent.ConvertForSetting(MofObject, value);
            MofObject.ProviderObject.AddToProperty(_property, valueToBeAdded, index);
        }

        /// <inheritdoc />
        public object get(int index)
        {
            var providerObject =  GetPropertyAsEnumerable().ElementAt(index);
            return MofObject.ConvertToMofObject(MofObject, _property, providerObject);
        }

        /// <inheritdoc />
        public void remove(int index)
        {
            MofObject.ProviderObject.RemoveFromProperty(
                _property,
                ((IEnumerable<object>) MofObject.ProviderObject.GetProperty(_property)).ElementAt(index));
        }

        /// <inheritdoc />
        public object set(int index, object value)
        {
            var valueToBeRemoved = GetPropertyAsEnumerable().ElementAt(index);
            MofObject.ProviderObject.RemoveFromProperty(_property, valueToBeRemoved);
            add(index, value);

            return MofObject.ConvertToMofObject(MofObject, _property, valueToBeRemoved);
        }

        /// <inheritdoc />
        public IExtent Extent => MofObject.Extent;
    }
}