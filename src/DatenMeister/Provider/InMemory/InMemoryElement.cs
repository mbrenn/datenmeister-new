using System;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Provider.InMemory
{
    /// <summary>
    ///     Implements the IElement according to the Mof specification
    /// </summary>
    [Obsolete]
    public class InMemoryElement : IElement
    {
        /// <inheritdoc />
        public bool @equals(object other)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public object get(string property)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void set(string property, object value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool isSet(string property)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void unset(string property)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IElement metaclass { get; }

        /// <inheritdoc />
        public IElement getMetaClass()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IElement container()
        {
            throw new NotImplementedException();
        }
    }
}