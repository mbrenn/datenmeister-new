#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Uml.Helper;

namespace DatenMeister.Forms.FormCreator
{
    public partial class FormCreator
    {
        /// <summary>
        ///     Creates the list form out of the elements in the reflective collection.
        ///     Supports the creation by the metaclass and by the object's properties
        /// </summary>
        /// <param name="elements">Elements to be queried</param>
        /// <param name="creationMode">The used creation mode</param>
        /// <returns>The created list form </returns>
        public IElement CreateListFormForCollection(IReflectiveCollection elements,
            FormFactoryConfiguration creationMode)
        {
            var result = MofFactory.create(_DatenMeister.TheOne.Forms.__ListForm);

            AddToListFormByElements(result, elements, creationMode with { IsForListView = true });

            return result;
        }

        /// <summary>
        ///     Creates a list form for a certain metaclass being used inside an extent form
        /// </summary>
        /// <param name="element">Element being used to determine the propertytype... But this is empty</param>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="propertyType"></param>
        /// <param name="creationMode"></param>
        public IElement CreateListFormForPropertyValues(
            IObject? element,
            string propertyName,
            IElement? propertyType,
            FormFactoryConfiguration creationMode)
        {
            if (!creationMode.CreateByMetaClass)
                throw new InvalidOperationException("The list form will only be created for the metaclass");

            var result = MofFactory.create(_DatenMeister.TheOne.Forms.__ListForm);

            FormMethods.AddToFormCreationProtocol(
                result,
                "[FormCreator.CreateListFormForPropertyValues]");

            AddFieldsToFormByMetaClass(result, propertyType, creationMode);
            result.set(_DatenMeister._Forms._ListForm.property, propertyName);
            result.set(_DatenMeister._Forms._ListForm.metaClass, propertyType);
            result.set(_DatenMeister._Forms._ListForm.title,
                $"{propertyName} - {NamedElementMethods.GetName(propertyType)}");
            result.set(_DatenMeister._Forms._ListForm.defaultTypesForNewElements, new[] { propertyType });

            FormMethods.AddToFormCreationProtocol(
                result,
                "[FormCreator.CreateListFormForPropertyValues] Set default type: " +
                NamedElementMethods.GetName(propertyType));


            return result;
        }

        /// <summary>
        ///     Creates a list form for a certain metaclass
        /// </summary>
        /// <param name="metaClass"></param>
        /// <param name="creationMode"></param>
        public IElement CreateListFormForMetaClass(
            IElement? metaClass,
            FormFactoryConfiguration creationMode)
        {
            return CreateListFormForMetaClass(metaClass, creationMode, null);
        }

        /// <summary>
        ///     Creates a list form for a certain metaclass
        /// </summary>
        /// <param name="metaClass"></param>
        /// <param name="creationMode"></param>
        /// <param name="property">Property being used</param>
        public IElement CreateListFormForMetaClass(
            IObject? metaClass,
            FormFactoryConfiguration creationMode,
            IElement? property = null)
        {
            if (!creationMode.CreateByMetaClass)
                throw new InvalidOperationException("The list form will only be created for the metaclass");

            var result = MofFactory.create(_DatenMeister.TheOne.Forms.__ListForm);
            var realPropertyName = NamedElementMethods.GetName(property);
            var propertyName = property != null ? realPropertyName : "List";

            FormMethods.AddToFormCreationProtocol(
                result,
                "[FormCreator.CreateListFormForMetaClass] Created for: " + NamedElementMethods.GetName(metaClass));

            var title =
                (metaClass != null ? NamedElementMethods.GetName(metaClass) : string.Empty) +
                (metaClass != null && property != null ? " - " : "") +
                propertyName;

            result.set(_DatenMeister._Forms._ListForm.title, "Types: " + title);
            result.set(_DatenMeister._Forms._ListForm.name, title);
            result.set(_DatenMeister._Forms._ListForm.property, realPropertyName);

            if (metaClass != null)
            {
                AddFieldsToFormByMetaClass(result, metaClass, creationMode with { IsForListView = true });

                var defaultType = MofFactory.create(_DatenMeister.TheOne.Forms.__DefaultTypeForNewElement);
                defaultType.set(_DatenMeister._Forms._DefaultTypeForNewElement.metaClass, metaClass);
                defaultType.set(
                    _DatenMeister._Forms._DefaultTypeForNewElement.name,
                    NamedElementMethods.GetName(metaClass));
                result.set(_DatenMeister._Forms._ListForm.defaultTypesForNewElements, new[] { defaultType });

                FormMethods.AddToFormCreationProtocol(
                    result,
                    "[FormCreator.CreateListFormForMetaClass] Added Default Type for: " +
                    NamedElementMethods.GetName(defaultType));
            }
            else
            {
                // Ok, we have no metaclass, but let's add at least the columns for the property 'name'
                AddFieldsToFormByMetaClassProperty(
                    result,
                    _UML.TheOne.CommonStructure.NamedElement._name,
                    new FormFactoryConfiguration
                        { CreateByPropertyValues = false, AutomaticMetaClassField = false, IsForListView = true },
                    FormUmlElementType.Property);
            }

            return result;
        }

        /// <summary>
        ///     Adds the property to the list elements by parsing through the
        /// </summary>
        /// <param name="form">Contains the form which is parsed</param>
        /// <param name="elements">Goes through the elements</param>
        /// <param name="creationMode">The creation mode to be used</param>
        public void AddToListFormByElements(
            IElement form,
            IReflectiveCollection elements,
            FormFactoryConfiguration creationMode)
        {
            var metaClassAdded = false;
            var onlyCommonProperties = creationMode.IncludeOnlyCommonProperties;

            var cache = new FormCreatorCache();
            var alreadyVisitedMetaClasses = new HashSet<IElement>();

            IObject? firstElementMetaClass = null;

            // Figure out only the elements which have common properties
            var propertyNames = onlyCommonProperties ? new HashSet<string>() : null;

            if (propertyNames != null)
            {
                DefinePropertyNames(elements, propertyNames, creationMode);

                foreach (var propertyName in propertyNames) cache.FocusOnPropertyNames.Add(propertyName);
            }

            foreach (var element in elements.OfType<IObject>())
            {
                var metaClass = (element as IElement)?.getMetaClass();
                if (firstElementMetaClass == null || !creationMode.AutomaticMetaClassField)
                {
                    // If this is the first element or when the reportCreator does not allow the addition
                    // of a metaclass
                    firstElementMetaClass = metaClass;
                }
                else if (!Equals(firstElementMetaClass, metaClass)
                         && !metaClassAdded
                         && !cache.MetaClassAlreadyAdded
                         && creationMode.AutomaticMetaClassField
                         && !FormMethods.HasMetaClassFieldInForm(form))
                {
                    metaClassAdded = true;
                    cache.MetaClassAlreadyAdded = true;

                    // Create the metaclass as a field
                    var metaClassField = MofFactory.create(_DatenMeister.TheOne.Forms.__MetaClassElementFieldData);
                    metaClassField.set(_DatenMeister._Forms._MetaClassElementFieldData.name, "Metaclass");
                    metaClassField.set(_DatenMeister._Forms._MetaClassElementFieldData.title, "Metaclass");
                    form.get<IReflectiveSequence>(_DatenMeister._Forms._ListForm.field).add(0, metaClassField);

                    FormMethods.AddToFormCreationProtocol(
                        form,
                        "[FormCreator.AddToListFormByElements] Added metaclass");
                }

                if (creationMode.CreateByMetaClass && metaClass != null)
                {
                    if (alreadyVisitedMetaClasses.Contains(metaClass)) continue;

                    alreadyVisitedMetaClasses.Add(metaClass);
                    AddFieldsToFormByMetaClass(form, metaClass, creationMode with { IsForListView = true }, cache);
                }
                else if (creationMode.CreateByPropertyValues)
                {
                    AddFieldsToForm(
                        form,
                        element,
                        creationMode with { CreateByMetaClass = false, IsForListView = true },
                        cache);
                }

                AddTextFieldForNameIfNoFieldAvailable(form);
                SortFieldsByImportantProperties(form);
            }
        }

        /// <summary>
        ///     Defines the propertynames to be used when the creation flag contains 'OnlyCommon Properties
        /// </summary>
        /// <param name="elements">The enumeration of elements which are parsed</param>
        /// <param name="propertyNames">A set of property names which are evaluated </param>
        /// <param name="creationMode">The creation mode to be used</param>
        private void DefinePropertyNames(IReflectiveCollection elements, ISet<string> propertyNames,
            FormFactoryConfiguration creationMode)
        {
            if (elements == null) throw new ArgumentNullException(nameof(elements));
            if (propertyNames == null) throw new ArgumentNullException(nameof(propertyNames));

            var firstRun = true;
            var toBeDeleted = new HashSet<string>();

            // Parses the meta class
            if (creationMode.CreateByMetaClass)
                foreach (var element in elements.OfType<IElement>())
                {
                    var metaClass = element.getMetaClass();
                    if (metaClass == null) continue;

                    var propertiesOfElement =
                        ClassifierMethods.GetPropertyNamesOfClassifier(metaClass)
                            .ToList();

                    firstRun = EvaluateProperties(propertiesOfElement, element);
                }

            // Parses the property values
            if (creationMode.CreateByPropertyValues)
                foreach (var element in elements.OfType<IElement>())
                {
                    var hasProperties = element as IObjectAllProperties;
                    if (hasProperties == null) continue;

                    var propertiesOfElement = hasProperties.getPropertiesBeingSet().ToList();

                    firstRun = EvaluateProperties(propertiesOfElement, element);
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
                        if (isSafeProperty) propertyNames.Add(propertyName);
                    }

                    firstRun = false;
                }

                // Check which properties are in the list but not in the object
                toBeDeleted.Clear();
                foreach (var propertyName in propertyNames)
                {
                    var isSafeProperty = DefaultClassifierHints.IsGenericProperty(element, propertyName);
                    if (!propertiesOfElement.Contains(propertyName) && !isSafeProperty) toBeDeleted.Add(propertyName);
                }

                // Now perform the deletion
                foreach (var propertyName in toBeDeleted) propertyNames.Remove(propertyName);

                return firstRun;
            }
        }

        /// <summary>
        ///     Creates a list form for a certain metaclass being used inside an extent form
        /// </summary>
        /// <param name="property">Property to be evaluated</param>
        /// <param name="creationMode"></param>
        public IElement CreateListFormForProperty(IElement property, FormFactoryConfiguration creationMode)
        {
            if (!creationMode.CreateByMetaClass)
                throw new InvalidOperationException("The list form will only be created for the metaclass");

            var propertyName = property.getOrDefault<string>(_UML._CommonStructure._NamedElement.name);
            var propertyType = PropertyMethods.GetPropertyType(property);

            var result = MofFactory.create(_DatenMeister.TheOne.Forms.__ListForm);

            FormMethods.AddToFormCreationProtocol(
                result,
                "[FormCreator.CreateListFormForProperty] Created for Property: " + propertyName);

            if (propertyType != null) AddFieldsToFormByMetaClass(result, propertyType, creationMode);

            result.set(_DatenMeister._Forms._ListForm.property, propertyName);
            result.set(_DatenMeister._Forms._ListForm.title, $"{propertyName}");
            return result;
        }
    }
}