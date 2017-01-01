using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.EMOF.Implementation
{
    public class Element : Object, IElement
    {
        /// <inheritdoc />
        public IElement metaclass { get; }

        /// <inheritdoc />
        public IElement getMetaClass()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public IElement container()
        {
            throw new System.NotImplementedException();
        }
    }
}