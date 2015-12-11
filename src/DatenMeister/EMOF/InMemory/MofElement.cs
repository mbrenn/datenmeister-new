using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.EMOF.InMemory
{
    /// <summary>
    ///     Implements the IElement according to the Mof specification
    /// </summary>
    public class MofElement : MofObject, IElement, IElementExt
    {
        private readonly IElement _container;

        public MofElement()
        {
        }

        public MofElement(IElement container, IElement metaClass) : this()
        {
            _container = container;
            metaclass = metaClass;
        }

        public IElement metaclass { get; private set; }

        public IElement container()
        {
            return _container;
        }

        public IElement getMetaClass()
        {
            return metaclass;
        }

        public void setMetaClass(IElement metaClass)
        {
            metaclass = metaClass;
        }
    }
}