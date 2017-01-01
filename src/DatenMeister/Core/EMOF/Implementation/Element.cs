using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider;

namespace DatenMeister.Core.EMOF.Implementation
{
    public class Element : Object, IElement
    {
        /// <inheritdoc />
        public IElement metaclass { get; }

        public Element(IProviderObject providedObject, IElement metaclass) : base (providedObject)
        {
            this.metaclass = metaclass;
        }
        

        /// <inheritdoc />
        public IElement getMetaClass()
        {
            return metaclass;
        }

        /// <inheritdoc />
        public IElement container()
        {
            throw new System.NotImplementedException();
        }
    }
}