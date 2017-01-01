using System.Collections;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Common;

namespace DatenMeister.Core.EMOF.Implementation
{
    public class ExtentReflectiveSequence : IReflectiveSequence
    {
        private readonly Extent _extent;

        public ExtentReflectiveSequence(Extent extent)
        {
            _extent = extent;
        }

        /// <inheritdoc />
        public IEnumerator<object> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public bool add(object value)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void add(int index, object value)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public object get(int index)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void remove(int index)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public object set(int index, object value)
        {
            throw new System.NotImplementedException();
        }
    }
}