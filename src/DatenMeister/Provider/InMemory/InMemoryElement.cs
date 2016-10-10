using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Provider.InMemory
{
    /// <summary>
    ///     Implements the IElement according to the Mof specification
    /// </summary>
    public class InMemoryElement : InMemoryObject, IElement, IElementSetMetaClass, IElementSetContainer
    {
        private IElement _container;

        public InMemoryElement()
        {
        }

        public InMemoryElement(IElement container, IElement metaClass) : this()
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

        public void setContainer(IElement container)
        {
            _container = container;
        }

        public override string ToString()
        {
            return metaclass == null || !metaclass.isSet("name")? 
                base.ToString() :
                $"[{metaclass.get("name")}] {base.ToString()}";
        }
    }
}