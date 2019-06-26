using System;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Defines a mofobject which is created on the fly to reference
    /// to a specific object which could not be looked up. This supports usages
    /// of DatenMeister with typed instances but without having the full MOF database in memory
    /// </summary>
    public class MofObjectShadow : IElement, IKnowsUri
    {
        /// <summary>
        /// Gets the uri, which describes the given element
        /// </summary>
        public string Uri { get; }

        public MofObjectShadow(string uri)
        {
            Uri = uri;
        }

        public bool @equals(object other)
        {
            return MofObject.AreEqual(this, other as IObject);
        }

        public object get(string property)
        {
            return null;
        }

        public void set(string property, object value)
        {
            throw new NotImplementedException("This is just a shadow object which cannot store data");
        }

        public bool isSet(string property)
        {
            return false;
        }

        public void unset(string property)
        {
            throw new NotImplementedException("This is just a shadow object which cannot store data");
        }

        public IElement metaclass => getMetaClass();

        public IElement getMetaClass()
        {
            return null;
        }

        public IElement container()
        {
            return null;
        }

        public override string ToString()
        {
            return $"Shadow: {Uri}";
        }
    }
}