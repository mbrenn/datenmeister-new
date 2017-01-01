using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider;

namespace DatenMeister.Core.EMOF.Implementation
{
    public class Object : IObject
    {
        private readonly IProviderObject _providedObject;

        public Object(IProviderObject providedObject)
        {
            _providedObject = providedObject;
        }

        /// <inheritdoc />
        public bool @equals(object other)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public object get(string property)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void set(string property, object value)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public bool isSet(string property)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void unset(string property)
        {
            throw new System.NotImplementedException();
        }
    }
}