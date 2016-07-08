using System.Collections.Generic;
using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Reflection
{
    public class ClassWrapper
    {
        private readonly IElement _element;

        private DmML _dmml;
        
        public ClassWrapper(DmML dmml)
        {
            _dmml = dmml;
            _element = new MofElement(_dmml?.__Class, null);
        }

        public ClassWrapper(IElement element, DmML dmml)
        {
            _element = element ?? new MofElement(_dmml?.__Class, null);
            _dmml = dmml;
        }

        public IElement Unwrap()
        {
            return _element;
        }

        public string Name
        {
            get { return _element.isSet("name") ? _element.get("name").ToString() : string.Empty; }
            set { _element.set("name", value); }
        }

        public IEnumerable<AttributeWrapper> Attributes
        {
            get
            {
                if (!_element.isSet("attribute"))
                {
                    var value = new List<AttributeWrapper>();
                    _element.set("attribute", value);
                }

                return (_element.get("attribute") as IReflectiveCollection).ToList<AttributeWrapper>(
                    x=> new AttributeWrapper(x as IElement, _dmml), 
                    x => x.Unwrap());
            }
        }
    }
}