using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;

namespace DatenMeister.Core.EMOF.Implementation
{
    public class TemporaryReflectiveCollection : IReflectiveCollection
    {
        protected readonly IEnumerable<object> Values;

        public TemporaryReflectiveCollection()
        {
            Values = new List<object>();
        }

        public TemporaryReflectiveCollection(IEnumerable<object> values)
        {
            Values = values;
        }

        /// <inheritdoc />
        public IEnumerator<object> GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        /// <inheritdoc />
        public bool add(object value)
        {
            (Values as IList<object>)?.Add(value);
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
            return Values.Count();
        }
    }
}