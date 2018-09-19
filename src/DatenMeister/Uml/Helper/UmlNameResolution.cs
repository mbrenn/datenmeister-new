using System;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Uml.Helper
{
    public static class UmlNameResolution
    {
        /// <summary>
        /// Gets the name of the given object
        /// </summary>
        /// <param name="element">Element whose name is requested</param>
        /// <returns>The found name or null, if not found</returns>
        public static string GetName(IObject element)
        {
            if (element == null)
            {
                return "null";
            }

            // If the element is not uml induced or the property is empty, check by
            // the default "name" property
            if (element.isSet(_UML._CommonStructure._NamedElement.name))
            {
                return element.get(_UML._CommonStructure._NamedElement.name).ToString();
            }

            switch (element)
            {
                case IHasId elementAsHasId:
                    return elementAsHasId.Id;
                case MofObjectShadow shadowedObject:
                    return shadowedObject.Uri;
                default:
                    return element.ToString();
            }
        }

        /// <summary>
        /// Gets the name of the given object
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetName(object element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return 
                !(element is IObject asObject) ?
                    element.ToString() 
                    : GetName(asObject);
        }
    }
}