using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Implements a reflective sequence as given by the MOF specification. 
    /// The sequence needs to be correlated to a Mof Object
    /// </summary>
    internal class MofReflectiveSequence : IReflectiveSequence
    {
        private readonly MofObject _mofObject;
        private readonly string _property;

        public MofReflectiveSequence(MofObject mofObject, string property)
        {
            _mofObject = mofObject;
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
            var result =GetPropertyAsEnumerable();
            foreach (var item in result)
            {
                yield return MofObject.ConvertToMofObject(_mofObject, _property, item);
            }
        }

        /// <summary>
        /// Gets the given property as an enumerable
        /// </summary>
        /// <returns>Enumerable which was retrieved</returns>
        private IEnumerable<object> GetPropertyAsEnumerable()
        {
            return (IEnumerable<object>) _mofObject.ProviderObject.GetProperty(_property);
        }

        /// <inheritdoc />
        public bool add(object value)
        {
            var valueToBeAdded = MofExtent.ConvertForSetting(_mofObject, value);
            return _mofObject.ProviderObject.AddToProperty(_property, valueToBeAdded);
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
            _mofObject.ProviderObject.EmptyListForProperty(_property);
        }

        /// <inheritdoc />
        public bool remove(object value)
        {
            if (value is MofObject)
            {
                var asProviderObject = (value as MofObject).ProviderObject;
                return _mofObject.ProviderObject.RemoveFromProperty(_property, asProviderObject);
            }
            else
            {
                return _mofObject.ProviderObject.RemoveFromProperty(_property, value);
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
            var valueToBeAdded = MofExtent.ConvertForSetting(_mofObject, value);
            _mofObject.ProviderObject.AddToProperty(_property, valueToBeAdded, index);
        }

        /// <inheritdoc />
        public object get(int index)
        {
            var providerObject =  GetPropertyAsEnumerable().ElementAt(index);
            return MofObject.ConvertToMofObject(_mofObject, _property, providerObject);
        }

        /// <inheritdoc />
        public void remove(int index)
        {
            _mofObject.ProviderObject.RemoveFromProperty(
                _property,
                ((IEnumerable<object>) _mofObject.ProviderObject.GetProperty(_property)).ElementAt(index));
        }

        /// <inheritdoc />
        public object set(int index, object value)
        {
            var valueToBeRemoved = GetPropertyAsEnumerable().ElementAt(index);
            _mofObject.ProviderObject.RemoveFromProperty(_property, valueToBeRemoved);
            add(index, value);

            return MofObject.ConvertToMofObject(_mofObject, _property, valueToBeRemoved);
        }
    }
}