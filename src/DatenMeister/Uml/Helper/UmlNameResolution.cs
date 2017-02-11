using System;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Uml.Helper
{
    public class UmlNameResolution : IUmlNameResolution
    {

        /// <summary>
        /// Gets the name of the given object
        /// </summary>
        /// <param name="element">Element whose name is requested</param>
        /// <returns>The found name or null, if not found</returns>
        string IUmlNameResolution.GetName(IObject element)
        {
            return GetName(element);
        }

        /// <summary>
        /// Gets the name of the given object
        /// </summary>
        /// <param name="element">Element whose name is requested</param>
        /// <returns>The found name or null, if not found</returns>
        public static string GetName(IObject element)
        {
            if(element == null)
            {
                return "null";
            }

            // If the element is not uml induced or the property is empty, check by
            // the default "name" property
            if (element.isSet(_UML._CommonStructure._NamedElement.name))
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