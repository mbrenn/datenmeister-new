using System;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Filler;

namespace DatenMeister.Uml.Helper
{
    public class UmlNameResolution : IUmlNameResolution
    {
        public string GetName(IObject element)
        {
            // If the element is not uml induced or the property is empty, check by
            // the default "name" property
            if ( element.isSet(_UML._CommonStructure._NamedElement.name) )
            {
                return element.get(_UML._CommonStructure._NamedElement.name).ToString();
            }

            var elementAsHasId = element as IHasId;
            if (elementAsHasId != null)
            {
                return elementAsHasId.Id;
            }

            return element.ToString();
        }

        public string GetName(object element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            var asObject = element as IObject;
            return asObject == null ? element.ToString() : GetName(asObject);
        }
    }
}