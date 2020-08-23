#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.EMOF;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.DefaultTypes;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.Forms.FormCreator
{
    public partial class FormCreator
    {
        /// <summary>
        /// Creates a list form for a certain metaclass
        /// </summary>
        /// <param name="metaClass"></param>
        /// <param name="creationMode"></param>
        /// <param name="property">Property being used</param>
        public IElement CreateListFormForMetaClass(
            IObject? metaClass,
            CreationMode creationMode,
            IElement? property = null)
        {
            if (!creationMode.HasFlag(CreationMode.ByMetaClass))
            {
                throw new InvalidOperationException("The list form will only be created for the metaclass");
            }

            var result = _factory.create(_formAndFields.__ListForm);
            var realPropertyName = NamedElementMethods.GetName(property);
            var propertyName = property != null ? realPropertyName : "List";
            
            var title = 
                (metaClass != null ? NamedElementMethods.GetName(metaClass) : string.Empty) +
                (metaClass != null && property != null ? " - " : "") +
                (property != null ? NamedElementMethods.GetName(property) : "");

            result.set(_FormAndFields._ListForm.title, title);
            result.set(_FormAndFields._ListForm.name, propertyName);
            result.set(_FormAndFields._ListForm.property, realPropertyName);
            
            if (metaClass != null)
            {
                AddToFormByMetaclass(result, metaClass, creationMode | CreationMode.ForListForms);
                
                var defaultType = _factory.create(_formAndFields.__DefaultTypeForNewElement);
                defaultType.set(_FormAndFields._DefaultTypeForNewElement.metaClass, metaClass);
                defaultType.set(_FormAndFields._DefaultTypeForNewElement.name, NamedElementMethods.GetName(metaClass));
                result.set(_FormAndFields._ListForm.defaultTypesForNewElements, new[] {defaultType});
            }
            else
            {
                // Ok, we have no metaclass, but let's add at least the columns for the property 'name'
                var nameProperty = _uml?.CommonStructure.NamedElement._name;
                if (nameProperty != null)
                {
                    AddToFormByUmlElement(result, nameProperty, CreationMode.ForListForms | CreationMode.ByMetaClass);
                }
            }

            return result;
        }

        /// <summary>
        /// Creates the list form out of the elements in the reflective collection.
        /// Supports the creation by the metaclass and by the object's properties
        /// </summary>
        /// <param name="elements">Elements to be queried</param>
        /// <param name="creationMode">The used creation mode</param>
        /// <returns>The created list form </returns>
        public IElement CreateListFormForElements(IReflectiveCollection elements, CreationMode creationMode)
        {
            var cache = new FormCreatorCache();
            var alreadyVisitedMetaClasses = new HashSet<IElement>();

            IObject? firstElementMetaClass = null;
            var metaClassAdded = false;
            var result = _factory.create(_formAndFields.__ListForm);
            var onlyCommonProperties = creationMode.HasFlagFast(CreationMode.OnlyCommonProperties);
            
            // Figure out only the elements which have common properties
            var propertyNames = onlyCommonProperties ? new HashSet<string>() : null;

            if (propertyNames != null)
            {
                DefinePropertyNames(elements, propertyNames, creationMode);
                
                foreach (var propertyName in propertyNames)
                {
                    cache.FocusOnPropertyNames.Add(propertyName);    
                }
            }

            foreach (var element in elements.OfType<IObject>())
            {
                var metaClass = (element as IElement)?.getMetaClass();
                if (firstElementMetaClass == null || !creationMode.HasFlag(CreationMode.AddMetaClass))
                {
                    // If this is the first element or when the creator does not allow the addition
                    // of a metaclass
                    firstElementMetaClass = metaClass;
                }

                else if (firstElementMetaClass != metaClass
                         && !metaClassAdded
                         && !cache.MetaClassAlreadyAdded
                         && creationMode.HasFlagFast(CreationMode.AddMetaClass)
                         && !FormMethods.HasMetaClassFieldInForm(result))
                {
                    metaClassAdded = true;
                    cache.MetaClassAlreadyAdded = true;

                    // Create the metaclass as a field
                    var metaClassField = _factory.create(_formAndFields.__MetaClassElementFieldData);
                    metaClassField.set(_FormAndFields._MetaClassElementFieldData.name, "Metaclass");
                    metaClassField.set(_FormAndFields._MetaClassElementFieldData.title, "Metaclass");
                    result.get<IReflectiveSequence>(_FormAndFields._ListForm.field).add(0, metaClassField);
                }

                if (creationMode.HasFlag(CreationMode.ByMetaClass) && metaClass != null)
                {
                    if (alreadyVisitedMetaClasses.Contains(metaClass))
                    {
                        continue;
                    }

                    alreadyVisitedMetaClasses.Add(metaClass);
                    AddToFormByMetaclass(result, metaClass, creationMode, cache);
                }
                else if (creationMode.HasFlag(CreationMode.ByPropertyValues))
                {
                    AddToForm(
                        result,
                        element,
                        creationMode & ~CreationMode.ByMetaClass, 
                        cache);
                }
            }

            return result;
        }

        /// <summary>
        /// Defines the propertynames to be used when the creation flag contains 'OnlyCommon Properties
        /// </summary>
        /// <param name="elements">The enumeration of elements which are parsed</param>
        /// <param name="propertyNames">A set of property names which are evaluated </param>
        /// <param name="creationMode">The creation mode to be used</param>
        private void DefinePropertyNames(IReflectiveCollection elements, ISet<string> propertyNames, CreationMode creationMode)
        {
            if (elements == null) throw new ArgumentNullException(nameof(elements));
            if (propertyNames == null) throw new ArgumentNullException(nameof(propertyNames));
            
            var firstRun = true;
            var toBeDeleted = new HashSet<string>();
            
            // Parses the meta class
            if (creationMode.HasFlagFast(CreationMode.ByMetaClass))
            {
                foreach (var element in elements.OfType<IElement>())
                {
                    var metaClass = element.getMetaClass();
                    if (metaClass == null)
                    {
                        continue;
                    }
                    
                    var propertiesOfElement =
                        ClassifierMethods.GetPropertyNamesOfClassifier(metaClass)
                            .ToList();

                    firstRun = EvaluateProperties(propertiesOfElement, element);
                }
            }

            // Parses the property values
            if (creationMode.HasFlagFast(CreationMode.ByPropertyValues))
            {
                foreach (var element in elements.OfType<IElement>())
                {
                    var hasProperties = element as IObjectAllProperties;
                    if (hasProperties == null)
                    {
                        continue;
                    }

                    var propertiesOfElement = hasProperties.getPropertiesBeingSet().ToList();

                    firstRun = EvaluateProperties(propertiesOfElement, element);
                }
            }

            bool EvaluateProperties(ICollection<string> propertiesOfElement, IElement element)
            {
                // Add the properties into the list, if first run or safe property
                foreach (var propertyName in propertiesOfElement)
                {
                    if (firstRun)
                    {
                        propertyNames.Add(propertyName);
                    }
                    else
                    {
                        var isSafeProperty = DefaultClassifierHints.IsGenericProperty(element, propertyName);
                        if (isSafeProperty)
                        {
                            propertyNames.Add(propertyName);
                        }
                    }

                    firstRun = false;
                }

                // Check which properties are in the list but not in the object
                toBeDeleted.Clear();
                foreach (var propertyName in propertyNames)
                {
                    var isSafeProperty = DefaultClassifierHints.IsGenericProperty(element, propertyName);
                    if (!propertiesOfElement.Contains(propertyName) && !isSafeProperty)
                    {
                        toBeDeleted.Add(propertyName);
                    }
                }

                // Now perform the deletion
                foreach (var propertyName in toBeDeleted)
                {
                    propertyNames.Remove(propertyName);
                }

                return firstRun;
            }
        }

        /// <summary>
        /// Creates a list form for a certain metaclass being used inside an extent form
        /// </summary>
        /// <param name="metaClass"></param>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="creationMode"></param>
        public IElement CreateListFormForPropertyInObject(IElement metaClass, string propertyName, CreationMode creationMode)
        {
            if (!creationMode.HasFlag(CreationMode.ByMetaClass))
            {
                throw new InvalidOperationException("The list form will only be created for the metaclass");
            }

            var result = _factory.create(_formAndFields.__ListForm);
            AddToFormByMetaclass(result, metaClass, creationMode);
            result.set(_FormAndFields._ListForm.property, propertyName);
            result.set(_FormAndFields._ListForm.metaClass, metaClass);
            result.set(_FormAndFields._ListForm.title, $"{propertyName} - {NamedElementMethods.GetName(metaClass)}");
            result.set(_FormAndFields._ListForm.defaultTypesForNewElements, new[]{metaClass});

            return result;
        }

        /// <summary>
        /// Creates a list form for a certain metaclass being used inside an extent form
        /// </summary>
        /// <param name="property">Property to be evaluated</param>
        /// <param name="creationMode"></param>
        public IElement CreateListFormForProperty(IElement property, CreationMode creationMode)
        {
            if (!creationMode.HasFlag(CreationMode.ByMetaClass))
            {
                throw new InvalidOperationException("The list form will only be created for the metaclass");
            }

            var propertyName = property.getOrDefault<string>(_UML._CommonStructure._NamedElement.name);
            var propertyType = PropertyMethods.GetPropertyType(property);

            var result = _factory.create(_formAndFields.__ListForm);
            if (propertyType != null)
            {
                AddToFormByMetaclass(result, propertyType, creationMode);
            }

            result.set(_FormAndFields._ListForm.property, propertyName);
            result.set(_FormAndFields._ListForm.title, $"{propertyName}");
            return result;
        }
        
    }
}