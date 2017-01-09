using System;
using System.Collections;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Common;

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
        private readonly IEnumerable<object> _result;

        public MofReflectiveSequence(MofObject mofObject, string property, IEnumerable<object> result)
        {
            _mofObject = mofObject;
            _property = property;
            _result = result;
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public IEnumerator<object> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool add(object value)
        {
            return _mofObject.ProviderObject.AddToProperty(_property, value);
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
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public int size()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void add(int index, object value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public object get(int index)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void remove(int index)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public object set(int index, object value)
        {
            throw new NotImplementedException();
        }
    }
}