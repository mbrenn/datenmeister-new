using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;

namespace DatenMeister.Core.EMOF.Implementation
{
    public class TemporaryReflectiveCollection : IReflectiveCollection
    {
        private readonly IEnumerable<object> _values;

        public TemporaryReflectiveCollection()
        {
            _values = new List<object>();
        }
        public TemporaryReflectiveCollection(IEnumerable<object> values )
        {
            _values = values;
        }

        /// <inheritdoc />
        public IEnumerator<object> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        /// <inheritdoc />
        public bool add(object value)
        {
            (_values as IList<object>)?.Add(value);
            return true;
        }

        /// <inheritdoc />
        public bool addAll(IReflectiveSequence value)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void clear()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public bool remove(object value)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public int size()
        {
            return _values.Count();
        }
    }
}