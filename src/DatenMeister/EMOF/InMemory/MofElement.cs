using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.EMOF.InMemory
{
    /// <summary>
    /// Implements the IElement according to the Mof specification
    /// </summary>
    public class MofElement : MofObject, IElement
    {
        IElement _container;

        IElement _metaclass;

        public IElement metaclass
        {
            get { return _metaclass; }
        }

        public MofElement()
        {   
        }

        public MofElement(IElement container, IElement metaClass) : this()
        {
            _container = container;
            _metaclass = metaClass;
        }
         
        public IElement container()
        {
            return _container;
        }

        public IElement getMetaClass()
        {
            return _metaclass;
        }
    }
}
