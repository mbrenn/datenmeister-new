using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.EMOF.InMemory
{
    /// <summary>
    ///     Implements the IElement according to the Mof specification
    /// </summary>
    public class MofElement : MofObject, IElement, IElementSetMetaClass
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

        public virtual IElement metaclass { get; private set; }

        public virtual IElement container()
        {
            return _container;
        }

        public virtual IElement getMetaClass()
        {
            return metaclass;
        }

        public virtual void setMetaClass(IElement metaClass)
        {
            metaclass = metaClass;
        }
    }
}