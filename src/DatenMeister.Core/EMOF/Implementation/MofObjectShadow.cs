#nullable enable 

using System;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Defines a MOF object which is created on the fly to reference
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

        public bool equals(object? other)
            => MofObject.AreEqual(this, other as IObject);

        public object? get(string property) => null;

        public void set(string property, object? value)
        {
            throw new NotImplementedException("This is just a shadow object which cannot store data");
        }

        public bool isSet(string property) => false;

        public void unset(string property)
        {
            throw new NotImplementedException("This is just a shadow object which cannot store data");
        }

        public IElement? metaclass => getMetaClass();

        public IElement? getMetaClass() => null;

        public IElement? container() => null;

        public override string ToString()
            => $"Shadow: {Uri}";

        public override int GetHashCode()
        {
            return Uri.GetHashCode();
        }
    }
}