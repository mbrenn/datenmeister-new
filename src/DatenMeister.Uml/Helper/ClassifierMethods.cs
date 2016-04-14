using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.DataLayer;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Uml.Helper
{
    public class ClassifierMethods
    {
        private readonly IDataLayerLogic _dataLayerLogic;

        public ClassifierMethods(IDataLayerLogic dataLayerLogic)
        {
            _dataLayerLogic = dataLayerLogic;
        }

        /// <summary>
        /// Returns a list of all properties within the classifier. 
        /// Also properties from generalized classes need to be returned
        /// </summary>
        /// <param name="classifier"></param>
        /// <param name="legacy">true, if legacy access to the attributes shall be used. 
        /// This means that the methods are accessed via string definitions and not via the properties</param>
        /// <returns></returns>
        public IEnumerable<IElement> GetPropertiesOfClassifier(IElement classifier, bool legacy = false)
        {
            var metaLayer = _dataLayerLogic.GetMetaLayerFor(_dataLayerLogic.GetDataLayerOfObject(classifier));
            var uml = legacy ? null : _dataLayerLogic.Get<_UML>(metaLayer);
            var propertyOwnedAttribute = legacy ? "ownedAttribute" : uml.Classification.Classifier.attribute;
            var propertyGeneralization = legacy ? "generalization" : uml.Classification.Classifier.generalization;
            var propertyGeneral = legacy ? "general" : uml.Classification.Generalization.general;

            if (classifier.isSet(propertyOwnedAttribute))
            {
                var result = classifier.get(propertyOwnedAttribute) as IEnumerable;
                foreach (var item in result)
                {
                    yield return item as IElement;
                }
            }

            // Check for generalizations
            if (classifier.isSet(propertyGeneralization))
            {
                var generalizations = classifier.get(propertyGeneralization) as IEnumerable;
                foreach (var generalization in generalizations.Cast<IElement>())
                {
                    var general = generalization.get(propertyGeneral) as IElement;
                    foreach (var found in GetPropertiesOfClassifier(general, legacy))
                    {
                        yield return found;
                    }
                }
            }

        }
    }
}