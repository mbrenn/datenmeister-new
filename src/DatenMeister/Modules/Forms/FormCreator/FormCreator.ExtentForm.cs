#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.Forms.FormCreator
{
    public partial class FormCreator
    {
        /// <summary>
        /// Checks whether a detail form is already within the element form.
        /// If yes, then it is directly returned, otherwise a new detail form is created and added to the form
        /// </summary>
        /// <param name="extentForm">extentForm to be evaluated</param>
        public IElement GetOrCreateDetailFormIntoExtentForm(IElement extentForm)
        {
            var tabs = extentForm.getOrDefault<IReflectiveCollection>(_FormAndFields._ExtentForm.tab);

            foreach (var tab in tabs.OfType<IElement>())
            {
                if (ClassifierMethods.IsSpecializedClassifierOf(
                    tab.getMetaClass(),
                    _formAndFields.__DetailForm))
                {
                    return tab;
                }
            }
            
            // Create new one
            var newTab = new MofFactory(extentForm).create(_formAndFields.__DetailForm);
            tabs.add(newTab);
            return newTab;
        }
        
        /// <summary>
        /// Creates the extent by parsing through all the elements and creation of fields.
        /// </summary>
        /// <param name="elements">Elements which are parsed to create the form</param>
        /// <param name="creationMode">The creation mode defining whether metaclass are used or not</param>
        /// <returns></returns>
        public IElement CreateExtentForm(IReflectiveCollection elements, CreationMode creationMode)
        {
            var cache = new FormCreatorCache();

            if (elements == null)
                throw new ArgumentNullException(nameof(elements));

            var tabs = new List<IElement>();

            var result = _factory.create(_formAndFields.__ExtentForm);
            result.set(_FormAndFields._ExtentForm.name, "Items");

            var elementsAsObjects = elements.OfType<IObject>().ToList();
            var elementsWithoutMetaClass = elementsAsObjects
                .Where(x =>
                {
                    var element = x as IElement;
                    var metaClass = element?.getMetaClass();
                    return metaClass == null;
                })
                .ToList();

            var elementsWithMetaClass = elementsAsObjects
                .OfType<IElement>()
                .GroupBy(x =>
                {
                    var metaClass = x.getMetaClass();
                    return metaClass;
                })
                .Where(x => x.Key != null)
                .ToList();

            if (elementsWithoutMetaClass.Any() || elementsAsObjects.Count == 0)
            {
                // If there are elements without a metaclass or if there are no elements at all within the extent
                // then provide an empty list form
                var form = _factory.create(_formAndFields.__ListForm);
                form.set(_FormAndFields._ListForm.name, "Unclassified");
                form.set(_FormAndFields._ListForm.noItemsWithMetaClass, true);

                foreach (var item in elementsWithoutMetaClass)
                    AddToForm(form, item, creationMode, cache);

                SetDefaultTypes(form);

                tabs.Add(form);
            }

            // Go through all the meta classes. 
            foreach (var group in elementsWithMetaClass)
            {
                // Now try to figure out the metaclass
                var groupedMetaclass = group.Key;
                IElement form;
                if (_formLogic != null) // View logic is used to ask for a default list view. 
                {
                    var extent = (elements as IHasExtent)?.Extent;
                    if (extent == null)
                    {
                        throw new InvalidOperationException("elements does not have an extent");
                    }
                    
                    // Asks the view logic whether it has a list form for the specific metaclass
                    // It will ask the form creator, if there is no view association directly referencing
                    // to the element
                    form = _formLogic.GetListFormForExtent(
                        extent,
                        groupedMetaclass,
                        FormDefinitionMode.Default) ?? throw new InvalidOperationException("No form was found");

                    if (creationMode.HasFlag(CreationMode.ByPropertyValues))
                    {
                        foreach (var element in group)
                        {
                            AddToFormByPropertyValues(form, element, creationMode, cache);
                        }
                    }
                }
                else
                {
                    // If no view logic is given, then ask directly the form creator.
                    form = CreateListFormForMetaClass(groupedMetaclass, creationMode);
                }

                form.set(_FormAndFields._ListForm.metaClass, group.Key);

                SetDefaultTypes(form);

                tabs.Add(form);
            }

            result.set(_FormAndFields._ExtentForm.tab, tabs);
            return result;

            void SetDefaultTypes(IElement form)
            {
                var extent = elements.GetAssociatedExtent();
                var defaultTypePackages = extent?.GetDefaultTypePackages();
                if (defaultTypePackages != null)
                    form.set(_FormAndFields._ListForm.defaultTypesForNewElements, defaultTypePackages);
            }
        }

        /// <summary>
        /// Gets the extent form for the given metaclass.
        /// This method is used when the user selects a metaclass to which a form shall be created
        /// </summary>
        /// <param name="metaClass">The meta class for which the extent form shall be created</param>
        /// <param name="creationMode">Defines the creation mode</param>
        /// <returns>The created extent form</returns>
        public IElement CreateExtentFormByMetaClass(IElement metaClass, CreationMode creationMode = CreationMode.All)
        {
            var extentForm = _factory.create(_formAndFields.__ExtentForm);
            extentForm.set(_FormAndFields._ExtentForm.name, NamedElementMethods.GetName(metaClass));

            var tabs = new List<IElement>();

            // Get all properties of the elements
            var properties = ClassifierMethods.GetPropertiesOfClassifier(metaClass).ToList();
            if (properties == null)
            {
                throw new InvalidOperationException("ExtentForm cannot be created because given element does not have properties");
            }

            var propertiesWithCollection =
                (from p in properties
                    where PropertyMethods.IsCollection(p)
                    select new {propertyName = NamedElementMethods.GetName(p), property = p}).ToList();

            var propertiesWithoutCollection =
                (from p in properties
                    where !PropertyMethods.IsCollection(p)
                    select new {propertyName = NamedElementMethods.GetName(p), property = p}).ToList();

            if (propertiesWithoutCollection.Any() || creationMode.HasFlag(CreationMode.AddMetaClass))
            {
                var detailForm = _factory.create(_formAndFields.__DetailForm);
                detailForm.set(_FormAndFields._DetailForm.name, "Detail");

                var fields = new List<IElement>();
                foreach (var property in propertiesWithoutCollection)
                {
                    var field = GetFieldForProperty(
                        property.property, 
                        CreationMode.All | CreationMode.ReadOnly);
                    fields.Add(field);
                }

                if (creationMode.HasFlag(CreationMode.AddMetaClass))
                {
                    // Add the element itself
                    fields.Add(_factory.create(_formAndFields.__MetaClassElementFieldData));
                }

                detailForm.set(_FormAndFields._DetailForm.field, fields);
                tabs.Add(detailForm);
            }

            foreach (var pair in propertiesWithCollection)
            {
                // Now try to figure out the metaclass
                var form = CreateListFormForMetaClass(
                    pair.property,
                    CreationMode.ByMetaClass);

                tabs.Add(form);
            }

            extentForm.set(_FormAndFields._ExtentForm.tab, tabs);

            return extentForm;
        }

        /// <summary>
        /// Creates the extent form for a specific object which is selected in the item explorer view.
        /// </summary>
        /// <param name="element">Element which shall be shown</param>
        /// <param name="extent">Extent containing the element</param>
        /// <param name="creationMode">The creation mode for autogeneration of the fields</param>
        /// <returns>Created Extent form as MofObject</returns>
        public IElement CreateExtentFormForObject(IObject element, IExtent extent, CreationMode creationMode)
        {
            var cache = new FormCreatorCache();

            var extentForm = _factory.create(_formAndFields.__ExtentForm);
            extentForm.set(_FormAndFields._ExtentForm.name, NamedElementMethods.GetName(element));
            var objectMetaClass = (element as IElement)?.getMetaClass();

            var tabs = new List<IElement>();

            // Get all properties of the elements
            var flagAddByMetaClass = creationMode.HasFlag(CreationMode.ByMetaClass) ||
                                     creationMode.HasFlag(CreationMode.AddMetaClass);
            var propertyNamesWithCollection = new List<string>();
            var propertyNamesWithoutCollection = new List<string>();
            
            // Adds the properties by the stored properties of the element
            if (creationMode.HasFlag(CreationMode.ByPropertyValues))
            {
                var properties = (element as IObjectAllProperties)?.getPropertiesBeingSet().ToList();
                properties ??= new List<string>();

                propertyNamesWithCollection = (from p in properties
                    where element.IsPropertyOfType<IReflectiveCollection>(p)
                    let propertyContent = element.get<IReflectiveCollection>(p)
                    where propertyContent != null
                    select p).ToList();

                propertyNamesWithoutCollection = (from p in properties
                    where !element.IsPropertyOfType<IReflectiveCollection>(p)
                    let propertyContent = element.get(p)
                    where propertyContent != null
                    select p).ToList();
            }

            // Adds the properties by the metaclasses
            if (flagAddByMetaClass && objectMetaClass != null)
            {
                var metaClassProperties = ClassifierMethods.GetPropertiesOfClassifier(objectMetaClass);
                foreach (var property in metaClassProperties)
                {
                    if (PropertyMethods.IsCollection(property))
                    {
                        propertyNamesWithCollection.Add(NamedElementMethods.GetName(property));
                    }
                    else
                    {
                        propertyNamesWithoutCollection.Add(NamedElementMethods.GetName(property));
                    }
                }
            }

            // Now collect the property Values
            var propertiesWithCollection =
                from p in propertyNamesWithCollection.Distinct()
                let propertyContent = element.get<IReflectiveCollection>(p)
                select new {propertyName = p, propertyContent};

            var propertiesWithoutCollection =
                (from p in propertyNamesWithoutCollection.Distinct()
                    let propertyContent = element.getOrDefault<object>(p)
                    select new {propertyName = p, propertyContent}).ToList();

            if (propertiesWithoutCollection.Any() || creationMode.HasFlag(CreationMode.AddMetaClass))
            {
                var detailForm = _factory.create(_formAndFields.__DetailForm);
                detailForm.set(_FormAndFields._DetailForm.name, "Detail");

                var fields = new List<IElement>();
                foreach (var pair in propertiesWithoutCollection)
                {
                    if (objectMetaClass != null)
                    {
                        var property = ClassifierMethods.GetPropertyOfClassifier(objectMetaClass, pair.propertyName);
                        if (property != null)
                        {
                            var field = GetFieldForProperty(
                                property, 
                                CreationMode.All | CreationMode.ReadOnly);
                            fields.Add(field);
                        }
                    }
                }

                if (!cache.MetaClassAlreadyAdded && creationMode.HasFlag(CreationMode.AddMetaClass))
                {
                    // Add the element itself
                    fields.Add(_factory.create(_formAndFields.__MetaClassElementFieldData));
                    cache.MetaClassAlreadyAdded = true;
                }

                detailForm.set(_FormAndFields._DetailForm.field, fields);
                tabs.Add(detailForm);
            }

            foreach (var pair in propertiesWithCollection)
            {
                var elementsAsObjects = pair.propertyContent.OfType<IObject>().ToList();
                var elementsWithoutMetaClass = elementsAsObjects.Where(x =>
                {
                    if (x is IElement innerElement)
                    {
                        return innerElement.getMetaClass() == null;
                    }

                    return true;
                }).ToList();

                var elementsWithMetaClass = elementsAsObjects
                    .OfType<IElement>()
                    .GroupBy(x => x.getMetaClass());

                if (elementsWithoutMetaClass.Any() || !elementsAsObjects.Any())
                {
                    // If there are elements included and they are filled
                    // OR, if there is no element included at all, create the corresponding list form
                    var form = _factory.create(_formAndFields.__ListForm);
                    form.set(_FormAndFields._ListForm.name, pair.propertyName);
                    form.set(_FormAndFields._ListForm.property, pair.propertyName);
                    form.set(_FormAndFields._ListForm.noItemsWithMetaClass, true);

                    foreach (var item in elementsWithoutMetaClass)
                    {
                        AddToForm(form, item, creationMode, cache);
                    }

                    tabs.Add(form);
                }

                foreach (var group in elementsWithMetaClass)
                {
                    // Now try to figure out the metaclass
                    var groupedMetaclass = group.Key;
                    var form = _formLogic.GetListFormForExtentForPropertyInObject(
                        element,
                        extent,
                        pair.propertyName,
                        groupedMetaclass,
                        FormDefinitionMode.Default);

                    tabs.Add(form);
                }
            }

            extentForm.set(_FormAndFields._ExtentForm.tab, tabs);

            return extentForm;
        }
        
    }
}