﻿#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Runtime.Workspaces;
using static DatenMeister.Core.Models.EMOF._UML._Classification;

namespace DatenMeister.Core.Uml.Helper
{
    public static class ClassifierMethods
    {
        /// <summary>
        /// Logger for the classifier methods
        /// </summary>
        public static ILogger ClassLogger = new ClassLogger(typeof(ClassifierMethods));

        /// <summary>
        /// Returns a list of all properties within the classifier.
        /// Also properties from generalized classes will be returned
        /// </summary>
        /// <param name="classifier">Gets the properties and all properties from base classes</param>
        /// <param name="alreadyIn">Returns the properties that are already in. </param>
        /// <param name="followGeneralizations">true, if the generalizations shall also be followed</param>
        public static IEnumerable<IElement> GetPropertiesOfClassifier(IObject classifier, HashSet<string>? alreadyIn = null, bool followGeneralizations = true)
        {
            if (classifier == null) throw new ArgumentNullException(nameof(classifier));
            alreadyIn ??= new HashSet<string>();

            var propertyOwnedAttribute = _UML._StructuredClassifiers._StructuredClassifier.ownedAttribute;

            if (classifier.isSet(propertyOwnedAttribute))
            {
                var result = (IEnumerable) (classifier.get(propertyOwnedAttribute) ??
                                            throw new InvalidOperationException(
                                                "classifier.get did not include 'ownedAttribute'"));
                
                foreach (var item in result.OfType<IElement>())
                {
                    // Checks, if a property with the same name was already selected
                    var name = item.getOrDefault<string>(_UML._CommonStructure._NamedElement.name);
                    if (alreadyIn.Contains(name))
                    {
                        continue;
                    }

                    alreadyIn.Add(name);

                    yield return item;
                }
            }

            if (followGeneralizations)
            {
                // Check for generalizations
                foreach (var general in GetGeneralizations(classifier))
                {
                    foreach (var found in GetPropertiesOfClassifier(general, alreadyIn, false))
                    {
                        yield return found;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the property of a classifier by name
        /// </summary>
        /// <param name="classifier">Classifier being queried</param>
        /// <param name="propertyName">Name of the properties</param>
        /// <returns>The found property</returns>
        public static IElement? GetPropertyOfClassifier(IObject classifier, string propertyName)
        {
            if (classifier == null) throw new ArgumentNullException(nameof(classifier));

            var properties = GetPropertiesOfClassifier(classifier);
            return properties.FirstOrDefault(x => x.getOrDefault<string>(_UML._CommonStructure._NamedElement.name) == propertyName);
        }

        /// <summary>
        /// Gets an enumeration of all properties having a composite
        /// </summary>
        /// <param name="classifier">Classifier to be evaluated</param>
        /// <returns>Enumeration of properties</returns>
        public static IEnumerable<IElement> GetCompositingProperties(IElement classifier)
        {
            var properties = GetPropertiesOfClassifier(classifier);
            foreach (var property in properties)
            {
                if (property.getOrDefault<bool>(_UML._Classification._Property.isComposite))
                {
                    yield return property;
                }
                else if (property.getOrDefault<string>(_UML._Classification._Property.aggregation) == "composite")
                {
                    yield return property;
                }
            }
        }

        /// <summary>
        /// Gets all generalizations of the given elements
        /// </summary>
        /// <param name="classifier">Classifier, whose generalizations are requested</param>
        /// <param name="alreadyVisited">Contains the elements that have been visited already
        /// The already visited elements will not be returned again</param>
        /// <returns>Enumeration of elements</returns>
        public static IEnumerable<IElement> GetGeneralizations(IObject classifier, HashSet<IElement>? alreadyVisited = null)
        {
            alreadyVisited ??= new HashSet<IElement>();
            var propertyGeneralization = _UML._Classification._Classifier.generalization;
            var propertyGeneral = _UML._Classification._Generalization.general;

            if (classifier.isSet(propertyGeneralization))
            {
                var generalizations = classifier.getOrDefault<IReflectiveCollection>(propertyGeneralization);
                foreach (var generalization in generalizations.Cast<IElement>())
                {
                    var general = generalization.getOrDefault<IElement>(propertyGeneral);
                    if (general == null)
                    {
                        var generalAsString = generalization.getOrDefault<string>(propertyGeneral);
                        if (!string.IsNullOrEmpty(generalAsString))
                        {
                            general = classifier.GetUriExtentOf()?.element("#" + generalAsString);
                        }
                    }

                    if (general == null)
                    {
                        var innerText = $"Somehow I got a null for the general of the generalizations of " +
                            $"{classifier}.... Generalizations needs to be verified";
                        ClassLogger.Error(innerText);
                        throw new InvalidOperationException(innerText);
                    }

                    if (alreadyVisited.Contains(general))
                    {
                        continue;
                    }

                    alreadyVisited.Add(general);

                    yield return general;

                    // Checks if the general also has generalization
                    foreach (var childGeneral in GetGeneralizations(general, alreadyVisited))
                    {
                        yield return childGeneral;
                    }
                }
            }
        }

        /// <summary>
        /// Gets all specializations of the given element and the element itself
        /// </summary>
        /// <param name="element">Element to be </param>
        /// <param name="visitedElements">Contains the elements that already have been visited.
        /// If the element has been visited, then no recursion is done</param>
        /// <param name="withoutItself">Flag indicating whether the value itself shall also be returned</param>
        /// <returns></returns>
        public static IEnumerable<IElement> GetSpecializations(
            IElement element,
            HashSet<IElement>? visitedElements = null,
            bool withoutItself = false)
        {
            visitedElements ??= new HashSet<IElement>();
            var extent = (element as IHasExtent)?.Extent;
            var workspace = extent?.GetWorkspace();
            if (extent == null)
            {
                // The element is not connected to an extent, so metaclasses cannot be found
                yield break;
            }

            var classInstance = _UML.TheOne.StructuredClassifiers.__Class;
            if (classInstance == null)
            {
                throw new InvalidOperationException("Classifier is not known in metaextent");
            }

            if (!withoutItself)
            {
                if (!visitedElements.Contains(element))
                {
                    yield return element;
                }
            }

            if (workspace != null)
            {
                // Go through each element within the found scope
                foreach (var elementInExtent in
                    AllDescendentsQuery.GetDescendents(workspace.GetAllElements())
                        .OfType<IElement>()
                        .Where(elementInExtent => classInstance.equals(elementInExtent.getMetaClass()))
                        .Where(elementInExtent => !visitedElements.Contains(elementInExtent)))
                {
                    var found = false;
                    try
                    {
                        var generalizations = GetGeneralizations(elementInExtent).ToList();
                        if (!visitedElements.Contains(elementInExtent) && generalizations.Contains(element))
                        {
                            found = true;

                            visitedElements.Add(elementInExtent);
                        }
                    }
                    catch(InvalidOperationException exc)
                    {
                        var innerText = $"Exception during determination of generalizations: " +
                            $"{exc} - Element {elementInExtent.ToString()} is disregarded";
                        ClassLogger.Warn(innerText);
                    }

                    // We have to move the yielding outside the try catch block
                    if (found)
                    {
                        yield return elementInExtent;
                    }
                }
            }
        }

        /// <summary>
        /// Returns an enumeration of property names which are returned
        /// by the method GetPropertiesOfClassifier
        /// </summary>
        /// <param name="classifier">Classifier which is queried</param>
        /// <returns>Enumeration of properties</returns>
        public static IEnumerable<string> GetPropertyNamesOfClassifier(IElement classifier)
        {
            return GetPropertiesOfClassifier(classifier)
                .Select(x => x.getOrDefault<string>(_UML._CommonStructure._NamedElement.name))
                .Where(x => x != null)
                .Select(x => x?.ToString() ?? string.Empty);
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
        /// Gets the information whether the specialized classifier can be generalized to the generalizedClassifier.
        /// The classifier itself is also considered as valid
        /// </summary>
        /// <param name="specializedClassifier">Special class which is checked</param>
        /// <param name="generalizedClassifier">The class against the specialized will be checked against. </param>
        /// <returns>true, if</returns>
        public static bool IsSpecializedClassifierOf(IElement? specializedClassifier, IElement? generalizedClassifier)
        {
            if (specializedClassifier == null || generalizedClassifier == null)
            {
                return false;
            }

            if (specializedClassifier.equals(generalizedClassifier))
            {
                return true;
            }

            return GetGeneralizations(specializedClassifier)
                .Any(generalization => IsSpecializedClassifierOf(generalization, generalizedClassifier));
        }

        /// <summary>
        /// Adds a new generalization to the specialized classifier mapping to the
        /// generalizedClassifier
        /// </summary>
        /// <param name="specializedClassifier">The classifier which will have a new generalization
        /// and consequently will get the properties of the generalization attached</param>
        /// <param name="generalizedClassifier">Generalized class being used as base for specialized one</param>
        public static IElement? AddGeneralization(IElement specializedClassifier, IElement generalizedClassifier)
        {
            if (specializedClassifier == null) throw new ArgumentNullException(nameof(specializedClassifier));
            if (generalizedClassifier == null) throw new ArgumentNullException(nameof(generalizedClassifier));

            if (GetGeneralizations(specializedClassifier).Contains(generalizedClassifier))
            {
                // Nothing to do
                return null;
            }

            var factory = new MofFactory(specializedClassifier);

            var newGeneralization = factory.create(_UML.TheOne.Classification.__Generalization);
            specializedClassifier.AddCollectionItem(
                _UML._Classification._Classifier.generalization,
                newGeneralization);
            newGeneralization.set(
                _UML._Classification._Generalization.general,
                generalizedClassifier);
            /*newGeneralization.set(
                _UML._Classification._Generalization.specific, 
                specializedClassifier);*/

            return newGeneralization;
        }

        /// <summary>
        /// Checks whether the element is of a primitive type
        /// </summary>
        /// <param name="element">Element to be checked</param>
        /// <returns>true, if primitive type, otherwise false, if probably of class or something else</returns>
        public static bool IsOfPrimitiveType(IElement element)
        {
            return (element.getMetaClass() as IHasId)?.Id?.EndsWith("PrimitiveType") == true;
        }

        /// <summary>
        /// Gets the property type of a certain property within the element.
        /// It can be null, if the property is not associated to the metaclass or
        /// if the propertytype is not connected to a metaclass
        /// </summary>
        /// <param name="element"></param>
        /// <param name="propertyName"></param>
        /// <returns>The propertytype of the property or nul, if not found. </returns>
        public static IElement? GetPropertyTypeOfValuesProperty(IElement? element, string propertyName)
        {
            var metaClass = element?.getMetaClass();
            if (element == null || metaClass == null)
            {
                return null;
            }

            var property = GetPropertyOfClassifier(metaClass, propertyName);
            if (property == null)
            {
                return null;
            }

            var propertyType = PropertyMethods.GetPropertyType(property);
            return propertyType;
        }

        /// <summary>
        /// Gets the property type of the metaclass' property by referencing the property name
        /// </summary>
        /// <param name="metaClass">Metaclass to be queried</param>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>The found property type or null, if not found</returns>
        public static IElement? GetPropertyType(IElement? metaClass, string propertyName)
        {
            if (metaClass == null)
            {
                return null;
            }

            var property = GetPropertyOfClassifier(metaClass, propertyName);
            if (property == null)
            {
                return null;
            }
            
            var propertyType = PropertyMethods.GetPropertyType(property);
            return propertyType;
        }
    }
}