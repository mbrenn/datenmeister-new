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
        protected bool IsReadOnly { get; set; }

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
            => Values.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
            => Values.GetEnumerator();

        /// <summary>
        /// Checks whether this reflective collection is read-only and throws an exception if yes
        /// </summary>
        private void CheckForReadOnly()
        {
            if (IsReadOnly)
                throw new InvalidOperationException("The temporary reflective collection is read-only");
        }

        /// <inheritdoc />
        public virtual bool add(object value)
        {
            CheckForReadOnly();
            (Values as IList<object>)?.Add(value);
            return true;
        }

        /// <inheritdoc />
        public virtual bool addAll(IReflectiveSequence value)
        {
            CheckForReadOnly();
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual void clear()
        {
            CheckForReadOnly();

            if (Values.GetType().IsArray)
            {
                Values = new object[] { };
            }
            else
            {
                (Values as IList)?.Clear();
            }
        }

        /// <inheritdoc />
        public virtual bool remove(object value)
        {
            CheckForReadOnly();
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual int size()
            => Values.Count();
    }
}