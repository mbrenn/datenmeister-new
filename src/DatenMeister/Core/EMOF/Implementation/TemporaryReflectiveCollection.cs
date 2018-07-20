using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;

namespace DatenMeister.Core.EMOF.Implementation
{
    public class TemporaryReflectiveCollection : IReflectiveCollection
    {
        protected IEnumerable<object> Values;

        /// <summary>
        /// Gets or sets a value whether the temporary collection is read-only and hinders adding new items
        /// </summary>
        protected  bool IsReadOnly { get; set; }


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

        /// <summary>
        /// Checks whether this reflective collection is read-only and throws an exception if yes
        /// </summary>
        private void CheckForReadOnly()
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException("The temporary reflective collection is read-only");
            }
        }

        /// <inheritdoc />
        public bool add(object value)
        {
            CheckForReadOnly();
            (Values as IList<object>)?.Add(value);
            return true;
        }

        /// <inheritdoc />
        public bool addAll(IReflectiveSequence value)
        {
            CheckForReadOnly();
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void clear()
        {
            CheckForReadOnly();
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public bool remove(object value)
        {
            CheckForReadOnly();
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public int size()
        {
            return Values.Count();
        }
    }
}