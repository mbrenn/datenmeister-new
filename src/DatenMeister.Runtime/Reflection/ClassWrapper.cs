using System.Collections.Generic;
using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Reflection
{
    public class ClassWrapper
    {
        private readonly IElement _element;
        private readonly DmML _dmml;
        
        public ClassWrapper(DmML dmml)
        {
            _dmml = dmml;
            _element = new MofElement(_dmml.__Class, null);
        }

        public ClassWrapper(IElement element, DmML dmml)
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
            get { return _element.isSet(_dmml.NamedElement) ? _element.get(_dmml.NamedElement).ToString() : string.Empty; }
            set { _element.set(_dmml.NamedElement, value); }
        }

        public IEnumerable<AttributeWrapper> Attributes
        {
            get
            {
                if (!_element.isSet(_dmml.Class.Attribute))
                {
                    var value = new List<AttributeWrapper>();
                    _element.set(value, _dmml.Class.Attribute);
                }

                return (_element.get(_dmml.Class.Attribute) as IReflectiveCollection).ToList<AttributeWrapper>(
                    x=> new AttributeWrapper(x as IElement, _dmml), 
                    x => x.Unwrap());
            }
        }
    }
}