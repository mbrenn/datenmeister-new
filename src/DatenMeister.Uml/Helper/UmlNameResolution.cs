using System;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Uml.Helper
{
    public class UmlNameResolution : IUmlNameResolution
    {
        public string GetName(IObject element)
        {
            // If the element is not uml induced or the property is empty, check by
            // the default "name" property
            return element.isSet(_UML._CommonStructure._NamedElement.name)
                ? element.get(_UML._CommonStructure._NamedElement.name).ToString()
                : element.ToString();
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