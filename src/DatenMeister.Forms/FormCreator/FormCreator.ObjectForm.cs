﻿using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Uml.Helper;

namespace DatenMeister.Forms.FormCreator
{
    public partial class FormCreator
    {
        /// <summary>
        ///     Creates the extent form for a specific object which is selected in the item explorer view.
        ///     This is the typical method that is used to create the form via the FormFinder
        /// </summary>
        /// <param name="element">Element which shall be shown</param>
        /// <param name="creationMode">The creation mode for auto-generation of the fields</param>
        /// <returns>Created Extent form as MofObject</returns>
        public IElement CreateObjectFormForItem(
            IObject element,
            FormFactoryConfiguration creationMode)
        {
            if (_workspaceLogic == null)
                throw new InvalidOperationException("WorkspaceLogic is null");

            var factory = GetMofFactory(creationMode);

            var cache = new FormCreatorCache();

            // Creates the empty form
            var objectForm = factory.create(_DatenMeister.TheOne.Forms.__ObjectForm);
            objectForm.set(
                _DatenMeister._Forms._ObjectForm.name, 
                $"Object Form for '{NamedElementMethods.GetName(element)}'");
            objectForm.set(_DatenMeister._Forms._ObjectForm.isAutoGenerated, true);

            FormMethods.AddToFormCreationProtocol(
                objectForm,
                "[FormCreator.CreateObjectFormForItem]: Create ExtentForm");

            var objectMetaClass = (element as IElement)?.getMetaClass();

            var tabs = new List<IElement>();

            // Get all properties of the elements
            var flagAddByMetaClass = creationMode.AutomaticMetaClassField;
            var propertyNamesWithCollection = new List<P>();
            var propertyNamesWithoutCollection = new List<P>();

            // Adds the properties by the stored properties of the element
            if (creationMode.CreateByPropertyValues)
            {
                var properties = (element as IObjectAllProperties)?.getPropertiesBeingSet().ToList();
                properties ??= new List<string>();

                propertyNamesWithCollection = (from p in properties
                    where element.IsPropertyOfType<IReflectiveCollection>(p)
                    let propertyContent = element.get<IReflectiveCollection>(p)
                    where propertyContent != null
                    select new P { PropertyName = p }).ToList();

                propertyNamesWithoutCollection = (from p in properties
                    where !element.IsPropertyOfType<IReflectiveCollection>(p)
                    let propertyContent = element.get(p)
                    where propertyContent != null
                    select new P { PropertyName = p }).ToList();
            }

            // Adds the properties by the metaclasses
            if (flagAddByMetaClass && objectMetaClass != null)
            {
                var metaClassProperties = ClassifierMethods.GetPropertiesOfClassifier(objectMetaClass);
                foreach (var property in metaClassProperties)
                {
                    if (PropertyMethods.IsCollection(property))
                    {
                        propertyNamesWithCollection.Add(
                            new P
                            {
                                PropertyName = NamedElementMethods.GetName(property),
                                PropertyType = PropertyMethods.GetPropertyType(property),
                                Property = property
                            });
                    }
                    
                    // As temporary workaround, we do also add the collections tothe detail view
                    propertyNamesWithoutCollection.Add(
                        new P
                        {
                            PropertyName = NamedElementMethods.GetName(property),
                            PropertyType = property,
                            Property = property
                        });
                }
            }

            // Now collect the property Values
            propertyNamesWithCollection.Reverse();
            propertyNamesWithoutCollection.Reverse();

            var propertiesWithCollection =
                from p in propertyNamesWithCollection.Distinct(new P.PropertyNameEqualityComparer())
                let propertyContent = element.get<IReflectiveCollection>(p.PropertyName)
                select new { propertyName = p.PropertyName, propertyType = p.PropertyType, propertyContent };

            var propertiesWithoutCollection =
                (from p in propertyNamesWithoutCollection.Distinct(new P.PropertyNameEqualityComparer())
                    let propertyContent = element.getOrDefault<object>(p.PropertyName)
                    select new { propertyName = p.PropertyName, propertyType = p.PropertyType, propertyContent })
                .ToList();

            //
            // Now, we got
            // propertyNamesWithCollection containing all properties which have a collection as a subitem ==> List Form
            // propertyNamesWithoutCollection containing all properties which have a collection as a subitem ==> Detail Form
            if (propertiesWithoutCollection.Any() || creationMode.AutomaticMetaClassField)
            {
                var detailForm = factory.create(_DatenMeister.TheOne.Forms.__RowForm);
                detailForm.set(_DatenMeister._Forms._RowForm.name, "Detail");
                detailForm.set(_DatenMeister._Forms._RowForm.isAutoGenerated, true);

                FormMethods.AddToFormCreationProtocol(
                    objectForm,
                    "[FormCreator.CreateObjectFormForItem]: Create DetailForm into Extent");

                var fields = new List<IElement>();
                foreach (var pair in propertiesWithoutCollection)
                {
                    if (pair.propertyName != null)
                    {
                        var property =
                            objectMetaClass == null
                                ? null
                                : ClassifierMethods.GetPropertyOfClassifier(objectMetaClass, pair.propertyName);

                        var field = CreateFieldForProperty(
                            objectMetaClass,
                            property,
                            pair.propertyName,
                            creationMode);
                        fields.Add(field);

                        FormMethods.AddToFormCreationProtocol(
                            objectForm,
                            "[FormCreator.CreateObjectFormForItem]: Added field to DetailForm: " +
                            NamedElementMethods.GetName(field));
                    }
                }

                if (!cache.MetaClassAlreadyAdded
                    && creationMode.AutomaticMetaClassField
                    && (_workspaceLogic == null ||
                        !new FormMethods(_workspaceLogic, _scopeStorage).HasMetaClassFieldInForm(fields)))
                {
                    // Add the element itself
                    var metaClassField = factory.create(_DatenMeister.TheOne.Forms.__MetaClassElementFieldData);
                    metaClassField.set(_DatenMeister._Forms._MetaClassElementFieldData.name, "Metaclass");
                    fields.Add(metaClassField);

                    FormMethods.AddToFormCreationProtocol(
                        objectForm,
                        "[FormCreator.CreateObjectFormForItem]: Added metaclass to DetailForm");

                    cache.MetaClassAlreadyAdded = true;
                }

                detailForm.set(_DatenMeister._Forms._RowForm.field, fields);
                tabs.Add(detailForm);
            }

            foreach (var pair in propertiesWithCollection)
            {
                var propertyName = pair.propertyName;
                var elementsAsObjects = pair.propertyContent.OfType<IObject>().ToList();

                if (propertyName == _UML._Packages._Package.packagedElement)
                {
                    // If the property for the list is 'packagedElement', then create multiple subforms...
                    var elementsWithoutMetaClass = elementsAsObjects.Where(x =>
                    {
                        if (x is IElement innerElement) return innerElement.getMetaClass() == null;

                        return true;
                    }).ToList();

                    var elementsWithMetaClass = elementsAsObjects
                        .OfType<IElement>()
                        .Where(x => x.getMetaClass() != null)
                        .GroupBy(x => x.getMetaClass()!);

                    if (elementsWithoutMetaClass.Any() || !elementsAsObjects.Any())
                    {
                        // If there are elements included and they are filled
                        // OR, if there is no element included at all, create the corresponding list form
                        var form = factory.create(_DatenMeister.TheOne.Forms.__TableForm);
                        form.set(_DatenMeister._Forms._TableForm.name, propertyName);
                        form.set(_DatenMeister._Forms._TableForm.property, propertyName);
                        form.set(_DatenMeister._Forms._TableForm.noItemsWithMetaClass, true);
                        form.set(_DatenMeister._Forms._TableForm.isAutoGenerated, true);

                        foreach (var item in elementsWithoutMetaClass)
                        {
                            AddFieldsToForm(form, item, creationMode, cache);
                        }

                        FormMethods.AddToFormCreationProtocol(
                            objectForm,
                            "[FormCreator.CreateObjectFormForItem]: Added Listform: " +
                            NamedElementMethods.GetName(pair.propertyType));
                        
                        FormMethods.RemoveDuplicatingDefaultNewTypes(form);

                        tabs.Add(form);
                    }

                    foreach (var group in elementsWithMetaClass)
                    {
                        // Now try to figure out the metaclass
                        var groupedMetaclass = group.Key ?? throw new InvalidOperationException("Key may not be null");
                        if (_formLogic != null)
                        {
                            FormMethods.AddToFormCreationProtocol(
                                objectForm,
                                "[FormCreator.CreateObjectFormForItem]: Add Listform for metaclass:" +
                                NamedElementMethods.GetName(groupedMetaclass));

                            var form = _parentFormFactory.CreateTableFormForProperty(
                                element,
                                propertyName,
                                groupedMetaclass,
                                new FormFactoryConfiguration { AllowFormModifications = false });
                            if (form != null) tabs.Add(form);
                        }
                        else
                        {
                            FormMethods.AddToFormCreationProtocol(
                                objectForm,
                                "[FormCreator.CreateObjectFormForItem]: Add Listform for metaclass:" +
                                NamedElementMethods.GetName(groupedMetaclass));

                            tabs.Add(
                                CreateTableFormForProperty(null, pair.propertyName, groupedMetaclass,
                                    creationMode with { AllowFormModifications = false }));
                        }
                    }
                }
                else
                {
                    FormMethods.AddToFormCreationProtocol(
                        objectForm,
                        "[FormCreator.CreateExtentFormForObject]: Add Listform for by reflective collection: " +
                        propertyName);

                    // If there are elements included and they are filled
                    // OR, if there is no element included at all, create the corresponding list form
                    var form = _parentFormFactory.CreateTableFormForCollection(
                        new TemporaryReflectiveCollection(elementsAsObjects),
                        creationMode with { AllowFormModifications = false });
                    if (form != null)
                    {
                        form.set(_DatenMeister._Forms._TableForm.name, $"Property: {propertyName}");
                        form.set(_DatenMeister._Forms._TableForm.property, propertyName);

                        // Adds the form to the tabs
                        tabs.Add(form);

                        var propertyType = pair.propertyType;
                        if (propertyType != null)
                        {
                            AddFieldsToRowOrTableFormByMetaClass(form, propertyType, creationMode, cache);
                            FormMethods.AddDefaultTypeForNewElement(form, propertyType);

                            FormMethods.AddToFormCreationProtocol(
                                form,
                                "[FormCreator.CreateObjectFormForItem]: Add DefaultTypeForNewElement driven by ExtentType: " +
                                NamedElementMethods.GetName(propertyType));
                        }
                    }
                }
            }

            // ReSharper restore HeuristicUnreachableCode
            objectForm.set(_DatenMeister._Forms._ObjectForm.tab, tabs);

            CleanupObjectForm(objectForm);
            return objectForm;
        }
        
        /// <summary>
        ///     Creates an extent form containing the subforms
        /// </summary>
        /// <param name="factory">The factory being used</param>
        /// <param name="tabsAsForms">The Forms which are converted to an extent form</param>
        /// <returns>The created extent</returns>
        public static IElement CreateObjectFormFromTabs(IFactory? factory, params IElement[] tabsAsForms)
        {
            factory ??= new MofFactory(tabsAsForms.First());
            var result = factory.create(_DatenMeister.TheOne.Forms.__ObjectForm);
            result.set(_DatenMeister._Forms._RowForm.isAutoGenerated, true);
            result.set(_DatenMeister._Forms._CollectionForm.tab, tabsAsForms);
            return result;
        }

        /// <summary>
        /// Creates an object form by the definition of a metaclass
        /// </summary>
        /// <param name="metaClass">MetaClass to be handled</param>
        /// <param name="configuration">Configuration of the Form Factory</param>
        /// <returns>The returned Object Form</returns>
        public IElement CreateObjectFormForMetaClass(IElement metaClass, FormFactoryConfiguration configuration)
        {
            var factory = GetMofFactory(configuration);
            var result = factory.create(_DatenMeister.TheOne.Forms.__ObjectForm);
            result.set(_DatenMeister._Forms._RowForm.isAutoGenerated, true);
            result.set(
                _DatenMeister._Forms._RowForm.name,
                $"Object Form for '{NamedElementMethods.GetFullName(metaClass)}'");
            
            var rowForm = _parentFormFactory.CreateRowFormByMetaClass(metaClass, configuration);
            result.set(_DatenMeister._Forms._CollectionForm.tab, new []{rowForm});
            return result;
        }
    }
}