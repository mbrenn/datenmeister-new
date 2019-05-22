using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Uml.Helper
{
    public static class ClassifierMethods
    {
        /// <summary>
        /// Returns a list of all properties within the classifier. 
        /// Also properties from generalized classes will be returned
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


        /// <summary>
        /// Gets or sets a value whether the given element is a derived type of the fullname.
        /// </summary>
        /// <param name="specializedClassifier">Classifier to be verified</param>
        /// <param name="generalizedFullName">The full name given</param>
        /// <returns>true, if the element is a derived type</returns>
        public static bool IsSpecializedClassifierOf(IElement specializedClassifier, string generalizedFullName)
        {
            if (NamedElementMethods.GetFullName(specializedClassifier) == generalizedFullName)
            {
                return true;
            }

            return GetGeneralizations(specializedClassifier).Any(x => IsSpecializedClassifierOf(x, generalizedFullName));
        }

        /// <summary>
        /// Gets the information whether the specialized classifier can be generalized to the generalizedClassifier
        /// </summary>
        /// <param name="specializedClassifer">Special class which is checked</param>
        /// <param name="generalizedClassifier">The class against the specialized will be checked against. </param>
        /// <returns>true, if</returns>
        public static bool IsSpecializedClassifierOf(IElement specializedClassifer, IElement generalizedClassifier)
        {
            if (specializedClassifer == null || generalizedClassifier == null)
            {
                return false;
            }

            if (specializedClassifer.@equals(generalizedClassifier))
            {
                return true;
            }

            return GetGeneralizations(specializedClassifer)
                .Any(generalization => IsSpecializedClassifierOf(generalization, generalizedClassifier));
        }

        /// <summary>
        /// Adds a new generalization to the specialized classifier mapping to the
        /// generalizedClassifier
        /// </summary>
        /// <param name="specializedClassifier">The classifier which will have a new generalization
        /// and consequently will get the properties of the generalization attached</param>
        /// <param name="generalizedClassifier">Generalized class being used as base for specialized one</param>
        public static IElement AddGeneralization(IElement specializedClassifier, IElement generalizedClassifier)
        {
            if (GetGeneralizations(specializedClassifier).Contains(generalizedClassifier))
            {
                // Nothing to do
                return null;
            }

            var factory = new MofFactory(specializedClassifier);
            var uml = GiveMe.Scope.WorkspaceLogic.GetUmlData();

            var newGeneralization = factory.create(uml.Classification.__Generalization);
            newGeneralization.set(
                _UML._Classification._Generalization.general, 
                generalizedClassifier);
            newGeneralization.set(
                _UML._Classification._Generalization.specific, 
                specializedClassifier);
            specializedClassifier.AddCollectionItem(
                _UML._Classification._Classifier.generalization, 
                newGeneralization);

            return newGeneralization;
        }
    }
}