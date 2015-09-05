using DatenMeister.MOF.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.MOF.InMemory
{
    /// <summary>
    /// Implements the IElement according to the Mof specification
    /// </summary>
    public class MofElement : MofObject, IElement
    {
        IElement _container;

        IElement _metaclass;

        public MofElement()
        {
        }

        public MofElement(IElement container, IElement metaClass)
        {
            _container = container;
            _metaclass = metaClass;
        }
         
        public IElement container()
        {
            return _container;
        }

        public IObject getMetaClass()
        {
            return _metaclass;
        }
    }
}
