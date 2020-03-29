#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.Forms.FormCreator
{
    public partial class FormCreator
    {
        /// <summary>
        /// Stores the configuration whether we require a tab for each property
        /// </summary>
        private const bool ConfigurationFormCreatorSeparateProperties = false;

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
                if (groupedMetaclass == null)
                {
                    // Should not happen, but we need to handle this
                    continue;
                }

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
            extentForm.set(_FormAndFields._ExtentForm.name, NamedElementMethods.GetName(metaClass) + " - List");

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

                if (creationMode.HasFlag(CreationMode.AddMetaClass)
                    || !FormMethods.HasMetaClassFieldInForm(detailForm))
                {
                    // Add the element itself
                    var metaClassField = _factory.create(_formAndFields.__MetaClassElementFieldData);
                    metaClassField.set(_FormAndFields._MetaClassElementFieldData.name, "Metaclass");
                    fields.Add(metaClassField);
                }

                detailForm.set(_FormAndFields._DetailForm.field, fields);
                tabs.Add(detailForm);
            }

            foreach (var pair in propertiesWithCollection)
            {
                var propertyType = PropertyMethods.GetPropertyType(pair.property);
                // Now try to figure out the metaclass
                var form = CreateListFormForMetaClass(
                    propertyType,
                    CreationMode.ByMetaClass,
                    pair.property);

                tabs.Add(form);
            }

            extentForm.set(_FormAndFields._ExtentForm.tab, tabs);

            return extentForm;
        }

        public class P
        {
            public string? PropertyName { get; set; }
            
            public IElement? PropertyType { get; set; }

            public class PropertyNameEqualityComparer : IEqualityComparer<P>
            {
                public bool Equals(P x, P y)
                {
                    if (x == null || y == null)
                    {
                        return false;
                    }

                    return x.PropertyName?.Equals(y.PropertyName) == true;
                }

                public int GetHashCode(P obj)
                {
                    return obj.PropertyName?.GetHashCode() ?? 0;
                }
            }
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
            if (_workspaceLogic == null)
                throw new InvalidOperationException("WorkspaceLogic is null");
            
            var cache = new FormCreatorCache();

            var extentForm = _factory.create(_formAndFields.__ExtentForm);
            extentForm.set(_FormAndFields._ExtentForm.name, NamedElementMethods.GetName(element));
            var objectMetaClass = (element as IElement)?.getMetaClass();

            var tabs = new List<IElement>();

            // Get all properties of the elements
            var flagAddByMetaClass = creationMode.HasFlag(CreationMode.ByMetaClass) ||
                                     creationMode.HasFlag(CreationMode.AddMetaClass);
            var propertyNamesWithCollection = new List<P>();
            var propertyNamesWithoutCollection = new List<P>();
            
            // Adds the properties by the stored properties of the element
            if (creationMode.HasFlag(CreationMode.ByPropertyValues))
            {
                var properties = (element as IObjectAllProperties)?.getPropertiesBeingSet().ToList();
                properties ??= new List<string>();

                propertyNamesWithCollection = (from p in properties
                    where element.IsPropertyOfType<IReflectiveCollection>(p)
                    let propertyContent = element.get<IReflectiveCollection>(p)
                    where propertyContent != null
                    select new P {PropertyName = p}).ToList();

                propertyNamesWithoutCollection = (from p in properties
                    where !element.IsPropertyOfType<IReflectiveCollection>(p)
                    let propertyContent = element.get(p)
                    where propertyContent != null
                    select new P {PropertyName = p}).ToList();
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
                                PropertyType = property
                            });
                    }
                    else
                    {
                        propertyNamesWithoutCollection.Add(
                            new P
                            {
                                PropertyName = NamedElementMethods.GetName(property),
                                PropertyType = property
                            });
                    }
                }
            }

            // Now collect the property Values
            propertyNamesWithCollection.Reverse();
            propertyNamesWithoutCollection.Reverse();
            
            var propertiesWithCollection =
                from p in propertyNamesWithCollection.Distinct(new P.PropertyNameEqualityComparer())
                let propertyContent = element.get<IReflectiveCollection>(p.PropertyName)
                select new {propertyName = p.PropertyName, propertyType =p.PropertyType, propertyContent};

            var propertiesWithoutCollection =
                (from p in propertyNamesWithoutCollection.Distinct(new P.PropertyNameEqualityComparer())
                    let propertyContent = element.getOrDefault<object>(p.PropertyName)
                    select new {propertyName = p.PropertyName, propertyType = p.PropertyType, propertyContent})
                .ToList();

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

                if (!cache.MetaClassAlreadyAdded
                    && creationMode.HasFlag(CreationMode.AddMetaClass)
                    && extent != null
                    && !new FormMethods(_workspaceLogic).HasMetaClassFieldInForm(extent, fields))
                {
                    // Add the element itself
                    var metaClassField = _factory.create(_formAndFields.__MetaClassElementFieldData);
                    metaClassField.set(_FormAndFields._MetaClassElementFieldData.name, "Metaclass");
                    fields.Add(metaClassField);

                    cache.MetaClassAlreadyAdded = true;
                }

                detailForm.set(_FormAndFields._DetailForm.field, fields);
                tabs.Add(detailForm);
            }

            foreach (var pair in propertiesWithCollection)
            {
                var elementsAsObjects = pair.propertyContent.OfType<IObject>().ToList();

                if (ConfigurationFormCreatorSeparateProperties)
#pragma warning disable 162
                {
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
                        if (_formLogic != null)
                        {
                            var form = _formLogic.GetListFormForExtentForPropertyInObject(
                                element,
                                extent,
                                pair.propertyName,
                                groupedMetaclass,
                                FormDefinitionMode.Default);
                            tabs.Add(form);
                        }
                        else
                        {
                            tabs.Add(
                                CreateListFormForPropertyInObject(groupedMetaclass, pair.propertyName, creationMode));
                        }
                    }
                }
                else
#pragma warning restore 162
                {
                    // If there are elements included and they are filled
                    // OR, if there is no element included at all, create the corresponding list form
                    var form = _factory.create(_formAndFields.__ListForm);
                    form.set(_FormAndFields._ListForm.name, pair.propertyName);
                    form.set(_FormAndFields._ListForm.property, pair.propertyName);

                    if (creationMode.HasFlagFast(CreationMode.ByPropertyValues))
                    {
                        foreach (var item in elementsAsObjects)
                        {
                            AddToForm(form, item, creationMode, cache);
                        }
                    }

                    if (creationMode.HasFlagFast(CreationMode.ByMetaClass))
                    {
                        var property = pair.propertyType;
                        var propertyType = property != null ? PropertyMethods.GetPropertyType(property) : null;
                        if (propertyType != null)
                        {
                            AddToFormByMetaclass(
                                form,
                                PropertyMethods.GetPropertyType(propertyType),
                                creationMode);
                        }
                    }
                    
                    // If the field is empty, create an empty textfield with 'name' as a placeholder
                    var fieldLength =
                        form.getOrDefault<IReflectiveCollection>(_FormAndFields._ListForm.field)?.Count() ?? 0;
                    if (fieldLength == 0)
                    {
                        var factory = new MofFactory(form);
                        var textFieldData = factory.create(_formAndFields.__TextFieldData);
                        textFieldData.set(_FormAndFields._TextFieldData.name, "name");
                        textFieldData.set(_FormAndFields._TextFieldData.title, "name");
                        
                        form.AddCollectionItem(_FormAndFields._ListForm.field, textFieldData);
                    }

                    // Adds the form to the tabs
                    tabs.Add(form);
                }
            }

            extentForm.set(_FormAndFields._ExtentForm.tab, tabs);

            return extentForm;
        }
        
    }
}