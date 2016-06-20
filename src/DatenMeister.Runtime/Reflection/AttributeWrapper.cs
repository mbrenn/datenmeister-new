using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Reflection
{
    public class AttributeWrapper
    {
        private readonly IElement _element;
        private readonly DmML _dmml;

        public AttributeWrapper(DmML dmml)
        {
            _dmml = dmml;
            _element = new MofElement(_dmml.__Property, null);
        }

        public AttributeWrapper(IElement element, DmML dmml)
        {
            _element = element;
            _dmml = dmml;
        }

        public IElement Unwrap()
        {
            return _element;
        }

        public string Name
        {
            get { return _element.isSet(_dmml.NamedElement.Name) ? _element.get(_dmml.NamedElement.Name).ToString() : string.Empty; }
            set { _element.set(_dmml.NamedElement.Name, value); }
        }
    }
}