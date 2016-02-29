using System;
using System.Collections.Generic;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Uml.Logic
{
    public class UmlLogic : IUmlLogic
    {
        public string GetProperty(IElement element, Func<_UML, object> mapping)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IElement> GetAllClassesThatCanBeCreated(IExtent extent)
        {
            throw new NotImplementedException();
        }
    }
}