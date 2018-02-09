using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Uml.Helper
{
    public static class ClassifierMethods
    {
        /// <summary>
        /// Returns a list of all properties within the classifier. 
        /// Also properties from generalized classes need to be returned
        /// </summary>
        /// <param name="classifier">Gets the properties and all properties from base classes</param>
        public static IEnumerable<IElement> GetPropertiesOfClassifier(IElement classifier)
        {
            if (classifier == null) throw new ArgumentNullException(nameof(classifier));

            var propertyOwnedAttribute = _UML._StructuredClassifiers._StructuredClassifier.ownedAttribute;

            if (classifier.isSet(propertyOwnedAttribute))
            {
                var result = (IEnumerable) classifier.get(propertyOwnedAttribute);
                foreach (var item in result)
                {
                    yield return item as IElement;
                }
            }

            // Check for generalizations
            foreach (var general in GetGeneralizations(classifier))
            {
                foreach (var found in GetPropertiesOfClassifier(general))
                {
                    yield return found;
                }
            }
        }

        /// <summary>
        /// Gets all generalizations of the given elements
        /// </summary>
        /// <param name="classifier">Classifier, whose generalizations are requested</param>
        /// <returns>Enumeration of elements</returns>
        public static IEnumerable<IElement> GetGeneralizations(IElement classifier)
        {
            var propertyGeneralization = _UML._Classification._Classifier.generalization;
            var propertyGeneral = _UML._Classification._Generalization.general;

            if (classifier.isSet(propertyGeneralization))
            {
                // Check for generalizations
                if (classifier.isSet(propertyGeneralization))
                {
                    var generalizations = classifier.get(propertyGeneralization) as IEnumerable;
                    foreach (var generalization in generalizations.Cast<IElement>())
                    {
                        if (!(generalization.get(propertyGeneral) is IElement general))
                        {
                            throw new InvalidOperationException("Somehow I got a null.... Generalizations needs to be verified");
                        }
                        yield return general;
                    }
                }
            }
        }

        /// <summary>
        /// Gets all specializations of the given element and the element itself
        /// </summary>
        /// <param name="extent">Extent being used to find all elements</param>
        /// <param name="element">Element to be </param>
        /// <returns></returns>
        public static IEnumerable<IElement> GetSpecializations(IUriExtent extent, IElement element)
        {
            yield return element;

            extent.
        }

        /// <summary>
        /// Returns an enumeration of property names which are returned
        /// by the method GetPropertiesOfClassifier
        /// </summary>
        /// <param name="classifier">Classifier which is queried</param>
        /// <returns>Enumeration of properties</returns>
        public static IEnumerable<string> GetPropertyNamesOfClassifier(IElement classifier)
        {
            return GetPropertiesOfClassifier(classifier).Select(x => x.get("name").ToString());
        }
    }
}