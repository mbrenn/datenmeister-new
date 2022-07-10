#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Uml.Helper;

namespace DatenMeister.Forms.FormCreator
{
    public partial class FormCreator
    {
        /// <summary>
        ///     Stores the configuration whether we require a tab for each property
        /// </summary>
        private const bool ConfigurationFormCreatorSeparateProperties = true;

        public IElement CreateExtentFormForItem(IObject element, FormFactoryConfiguration configuration)
        {
            var detailForm = CreateDetailFormForItem(element, configuration);

            return CreateCollectionFormFromTabs(new MofFactory(detailForm), detailForm);
        }

        /// <summary>
        ///     Checks whether a detail form is already within the element form.
        ///     If yes, then it is directly returned, otherwise a new detail form is created and added to the form
        /// </summary>
        /// <param name="extentForm">extentForm to be evaluated</param>
        public IElement GetOrCreateDetailFormIntoExtentForm(IElement extentForm)
        {
            var tabs = extentForm.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._CollectionForm.tab);

            foreach (var tab in tabs.OfType<IElement>())
            {
                if (ClassifierMethods.IsSpecializedClassifierOf(
                        tab.getMetaClass(),
                        _DatenMeister.TheOne.Forms.__RowForm))
                {
                    return tab;
                }
            }

            // Create new one
            var newTab = new MofFactory(extentForm).create(_DatenMeister.TheOne.Forms.__RowForm);
            tabs.add(newTab);
            return newTab;
        }

        /// <summary>
        ///     Creates an extent form containing the subforms
        /// </summary>
        /// <param name="tabsAsForms">The Forms which are converted to an extent form</param>
        /// <returns>The created extent</returns>
        public static IElement CreateCollectionFormFromTabs(params IElement[] tabsAsForms)
        {
            var factory = new MofFactory(tabsAsForms.First());
            return CreateCollectionFormFromTabs(factory, tabsAsForms);
        }

        /// <summary>
        ///     Creates an extent form containing the subforms
        /// </summary>
        /// <param name="factory">The factory being used</param>
        /// <param name="tabsAsForms">The Forms which are converted to an extent form</param>
        /// <returns>The created extent</returns>
        public static IElement CreateCollectionFormFromTabs(IFactory? factory, params IElement[] tabsAsForms)
        {
            factory ??= new MofFactory(tabsAsForms.First());
            var result = factory.create(_DatenMeister.TheOne.Forms.__CollectionForm);
            result.set(_DatenMeister._Forms._CollectionForm.tab, tabsAsForms);
            return result;
        }

        /// <summary>
        ///     Creates an extent form for the given extent by parsing through each element
        ///     and creating the form out of the max elements
        /// </summary>
        /// <param name="extent">Extent to be parsed</param>
        /// <param name="creationMode">The creation mode being used</param>
        /// <returns>The created element</returns>
        public IElement CreateExtentFormForExtent(IExtent extent, FormFactoryConfiguration creationMode)
        {
            var extentFormConfiguration = new ExtentFormConfiguration();
            var extentTypes = extent.GetConfiguration().ExtentTypes;
            extentFormConfiguration.ExtentTypes.AddRange(extentTypes);

            return CreateExtentFormForCollection(
                extent.elements().TakeFirst(100),
                creationMode,
                extentFormConfiguration);
        }

        /// <summary>
        ///     Creates the extent by parsing through all the elements and creation of fields.
        /// </summary>
        /// <param name="elements">Elements which are parsed to create the form</param>
        /// <param name="configuration">The creation mode defining whether metaclass are used or not</param>
        /// <param name="extentFormConfiguration">Configuration of the extent form</param>
        /// <returns>The created form</returns>
        public IElement CreateExtentFormForCollection(
            IReflectiveCollection elements,
            FormFactoryConfiguration configuration,
            ExtentFormConfiguration? extentFormConfiguration)
        {
            extentFormConfiguration ??= new ExtentFormConfiguration();
            
            var cache = new FormCreatorCache();
            if (elements == null)
                throw new ArgumentNullException(nameof(elements));

            var tabs = new List<IElement>();

            var result = MofFactory.create(_DatenMeister.TheOne.Forms.__CollectionForm);
            result.set(_DatenMeister._Forms._CollectionForm.name, "Items");
            result.set(_DatenMeister._Forms._Form.isAutoGenerated, true);

            FormMethods.AddToFormCreationProtocol(
                result,
                "[FormCreator.CreateExtentFormForCollection] Using Form Creator");

            var elementsAsObjects = elements.OfType<IObject>().ToList();
            var elementsWithoutMetaClass = elementsAsObjects
                .Where(x =>
                {
                    var element = x as IElement;
                    var metaClass = element?.getMetaClass();
                    return metaClass is null or MofObjectShadow;
                })
                .ToList();

            var elementsWithMetaClass = elementsAsObjects
                .OfType<IElement>()
                .GroupBy(x =>
                {
                    var metaClass = x.getMetaClass();
                    return metaClass is MofObjectShadow ? null : metaClass;
                })
                .Where(x => x.Key != null)
                .ToList();

            // Goes through all the extent types and adds the default metaclasses into the list of tables
            var metaClasses = elementsWithMetaClass.Select(x => x.Key).ToList();
            foreach (var extentType in extentFormConfiguration.ExtentTypes)
            {
                var extentTypeSetting = _extentSettings.GetExtentTypeSetting(extentType);
                if (extentTypeSetting == null) continue;

                foreach (var extentMetaClass in
                         extentTypeSetting.rootElementMetaClasses.Select(
                             x => _workspaceLogic.ResolveElement(x, ResolveType.Default)))
                {
                    if (metaClasses.Any(x => x != null && x.equals(extentMetaClass)))
                    {
                        continue;
                    }

                    metaClasses.Add(extentMetaClass);
                    
                    FormMethods.AddToFormCreationProtocol(
                        result,
                        $"[FormCreator.CreateExtentFormForCollection] Adding listform for '{NamedElementMethods.GetName(extentMetaClass)}' for extent type '{extentTypeSetting.name}'");
                }
            }

            // Create the tab for the elements of without any metaclass
            if (elementsWithoutMetaClass.Any() || elementsAsObjects.Count == 0)
            {
                var form = _parentFormFactory.CreateListFormForCollection(
                    new TemporaryReflectiveCollection(elementsWithoutMetaClass),
                    configuration with { IsForListView = true, AllowFormModifications = false });
                if (form == null)
                {
                    throw new InvalidOperationException("The form was not created... When it should have been.");
                }

                FormMethods.AddToFormCreationProtocol(
                    result,
                    "[FormCreator.CreateExtentFormForCollection]: Create ListForm for unclassified elements");

                form.set(_DatenMeister._Forms._TableForm.name, "Unclassified");
                form.set(_DatenMeister._Forms._TableForm.noItemsWithMetaClass, true);
                
                SortFieldsByImportantProperties(form);
                
                // Remove action create property buttons which were created and are covered by the list forms
                // being created below
                
                
                tabs.Add(form);
            }

            // Go through all the meta classes and create a tab for each of them
            foreach (var groupedMetaclass in metaClasses)
            {
                var group =
                    elementsWithMetaClass.FirstOrDefault(y => y.Key == groupedMetaclass)?.ToList() ??
                    new List<IElement>();

                // Now try to figure out the metaclass
                if (groupedMetaclass == null)
                    // Should not happen, but we need to handle this
                    continue;

                IElement form;
                if (_formLogic != null) // View logic is used to ask for a default list view. 
                {
                    var extent = (elements as IHasExtent)?.Extent;
                    if (extent == null) throw new InvalidOperationException("elements does not have an extent");

                    // Asks the view logic whether it has a list form for the specific metaclass
                    // It will ask the form reportCreator, if there is no view association directly referencing
                    // to the element
                    var formCreator = new FormFactory(_workspaceLogic, _scopeStorage);

                    FormMethods.AddToFormCreationProtocol(
                        result,
                        "[FormCreator.CreateExtentFormForCollection]: Create ListForm for metaclass: " +
                        NamedElementMethods.GetName(groupedMetaclass));

                    form = formCreator.CreateListFormForMetaClass(
                               groupedMetaclass,
                               configuration with
                               {
                                   IsReadOnly = true, IsForListView = true, AllowFormModifications = false
                               }) ??
                           throw new InvalidOperationException("No form was found");

                    if (configuration.CreateByMetaClass)
                    {
                        foreach (var element in @group)
                        {
                            AddFieldsToFormByPropertyValues(form, element, configuration, cache);
                        }
                    }

                    FormMethods.AddToFormCreationProtocol(
                        form,
                        "[FormCreator.CreateExtentFormForCollection]: Create Default Type for metaclass: " +
                        NamedElementMethods.GetName(groupedMetaclass));
                    FormMethods.AddDefaultTypeForNewElement(form, groupedMetaclass);
                }
                else
                {
                    // If no view logic is given, then ask directly the form reportCreator.
                    form = CreateListFormForMetaClass(groupedMetaclass,
                        configuration with { AllowFormModifications = false });
                }

                form.set(_DatenMeister._Forms._TableForm.metaClass, groupedMetaclass);
                tabs.Add(form);
            }

            result.set(_DatenMeister._Forms._CollectionForm.tab, tabs);
            CleanupExtentForm(result);
            
            return result;
        }

        /// <summary>
        ///     Gets the extent form for the given metaclass.
        ///     This method is used when the user selects a metaclass to which a form shall be created
        /// </summary>
        /// <param name="metaClass">The meta class for which the extent form shall be created</param>
        /// <param name="creationMode">Defines the creation mode</param>
        /// <returns>The created extent form</returns>
        public IElement CreateExtentFormForItemsMetaClass(IElement metaClass,
            FormFactoryConfiguration? creationMode = null)
        {
            creationMode ??= new FormFactoryConfiguration();

            var extentForm = MofFactory.create(_DatenMeister.TheOne.Forms.__ObjectForm);
            extentForm.set(_DatenMeister._Forms._ObjectForm.name, NamedElementMethods.GetName(metaClass) + " - List");
            extentForm.set(_DatenMeister._Forms._ObjectForm.isAutoGenerated, true);
            
            FormMethods.AddToFormCreationProtocol(
                extentForm,
                "[FormCreator.CreateExtentFormForItemsMetaClass]: Add Extent by Metaclass: " +
                NamedElementMethods.GetName(metaClass));

            var tabs = new List<IElement>();

            // Get all properties of the elements
            var properties = ClassifierMethods.GetPropertiesOfClassifier(metaClass).ToList();
            if (properties == null)
                throw new InvalidOperationException(
                    "ExtentForm cannot be created because given element does not have properties");

            var propertiesWithCollection =
                (from p in properties
                    where PropertyMethods.IsCollection(p)
                    select new { propertyName = NamedElementMethods.GetName(p), property = p }).ToList();

            var propertiesWithoutCollection =
                (from p in properties
                    where !PropertyMethods.IsCollection(p)
                    select new { propertyName = NamedElementMethods.GetName(p), property = p }).ToList();

            if (propertiesWithoutCollection.Any() || creationMode.AutomaticMetaClassField)
            {
                var detailForm = MofFactory.create(_DatenMeister.TheOne.Forms.__RowForm);
                detailForm.set(_DatenMeister._Forms._RowForm.name, "Detail");
                detailForm.set(_DatenMeister._Forms._RowForm.isAutoGenerated, true);

                FormMethods.AddToFormCreationProtocol(
                    extentForm,
                    "[FormCreator.CreateExtentFormForItemsMetaClass]: Add DetailForm");

                var fields = new List<IElement>();
                foreach (var property in propertiesWithoutCollection)
                {
                    var field = CreateFieldForProperty(
                        metaClass,
                        property.property,
                        null,
                        new FormFactoryConfiguration { IsReadOnly = true });
                    fields.Add(field);

                    FormMethods.AddToFormCreationProtocol(
                        extentForm,
                        "[FormCreator.CreateExtentFormForItemsMetaClass]: Add Field to Detailform: " +
                        NamedElementMethods.GetName(field));
                }

                if (creationMode.AutomaticMetaClassField
                    || !FormMethods.HasMetaClassFieldInForm(detailForm))
                {
                    // Add the element itself
                    var metaClassField = MofFactory.create(_DatenMeister.TheOne.Forms.__MetaClassElementFieldData);
                    metaClassField.set(_DatenMeister._Forms._MetaClassElementFieldData.name, "Metaclass");
                    fields.Add(metaClassField);

                    FormMethods.AddToFormCreationProtocol(
                        extentForm,
                        "[FormCreator.CreateExtentFormForItemsMetaClass]: Add MetaClass Field to Detailform");
                }

                detailForm.set(_DatenMeister._Forms._RowForm.field, fields);
                
                CleanupDetailForm(detailForm);
                
                tabs.Add(detailForm);
            }

            foreach (var pair in propertiesWithCollection)
            {
                FormMethods.AddToFormCreationProtocol(
                    extentForm,
                    "[FormCreator.CreateExtentFormForItemsMetaClass]: Add ListForm: " +
                    NamedElementMethods.GetName(pair.property));

                var propertyType = PropertyMethods.GetPropertyType(pair.property);
                // Now try to figure out the metaclass
                var form = CreateListFormForMetaClass(
                    propertyType,
                    new FormFactoryConfiguration
                    {
                        CreateByPropertyValues = false, AutomaticMetaClassField = false, AllowFormModifications = false
                    },
                    pair.property);

                tabs.Add(form);
            }

            extentForm.set(_DatenMeister._Forms._CollectionForm.tab, tabs);

            CleanupExtentForm(extentForm);
            
            return extentForm;
        }

        public class P
        {
            public string PropertyName { get; set; } = string.Empty;

            public IElement? PropertyType { get; set; }

            public IElement? Property { get; set; }

            public class PropertyNameEqualityComparer : IEqualityComparer<P>
            {
                public bool Equals(P? x, P? y)
                {
                    if (x == null || y == null) return false;

                    return x.PropertyName?.Equals(y.PropertyName) == true;
                }

                public int GetHashCode(P obj)
                {
                    return obj.PropertyName?.GetHashCode() ?? 0;
                }
            }
        }

        /// <summary>
        ///     Creates the extent form for a specific object which is selected in the item explorer view.
        ///     This is the typical method that is used to create the form via the FormFinder
        /// </summary>
        /// <param name="element">Element which shall be shown</param>
        /// <param name="extent">Extent containing the element</param>
        /// <param name="creationMode">The creation mode for auto-generation of the fields</param>
        /// <returns>Created Extent form as MofObject</returns>
        public IElement CreateExtentFormForObject(IObject element, IExtent extent,
            FormFactoryConfiguration creationMode)
        {
            if (_workspaceLogic == null)
                throw new InvalidOperationException("WorkspaceLogic is null");

            var cache = new FormCreatorCache();

            // Creates the empty form
            var extentForm = MofFactory.create(_DatenMeister.TheOne.Forms.__ObjectForm);
            extentForm.set(_DatenMeister._Forms._ObjectForm.name, NamedElementMethods.GetName(element));
            extentForm.set(_DatenMeister._Forms._ObjectForm.isAutoGenerated, true);

            FormMethods.AddToFormCreationProtocol(
                extentForm,
                "[FormCreator.CreateExtentFormForObject]: Create ExtentForm");

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
                    if (PropertyMethods.IsCollection(property))
                        propertyNamesWithCollection.Add(
                            new P
                            {
                                PropertyName = NamedElementMethods.GetName(property),
                                PropertyType = PropertyMethods.GetPropertyType(property),
                                Property = property
                            });
                    else
                        propertyNamesWithoutCollection.Add(
                            new P
                            {
                                PropertyName = NamedElementMethods.GetName(property),
                                PropertyType = property,
                                Property = property
                            });
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
                var detailForm = MofFactory.create(_DatenMeister.TheOne.Forms.__RowForm);
                detailForm.set(_DatenMeister._Forms._RowForm.name, "Detail");
                detailForm.set(_DatenMeister._Forms._RowForm.isAutoGenerated, true);

                FormMethods.AddToFormCreationProtocol(
                    extentForm,
                    "[FormCreator.CreateExtentFormForObject]: Create DetailForm into Extent");

                var fields = new List<IElement>();
                foreach (var pair in propertiesWithoutCollection)
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
                            creationMode with { IsReadOnly = true });
                        fields.Add(field);

                        FormMethods.AddToFormCreationProtocol(
                            extentForm,
                            "[FormCreator.CreateExtentFormForObject]: Added field to DetailForm: " +
                            NamedElementMethods.GetName(field));
                    }

                if (!cache.MetaClassAlreadyAdded
                    && creationMode.AutomaticMetaClassField
                    && extent != null
                    && (_workspaceLogic == null ||
                        !new FormMethods(_workspaceLogic, _scopeStorage).HasMetaClassFieldInForm(extent, fields)))
                {
                    // Add the element itself
                    var metaClassField = MofFactory.create(_DatenMeister.TheOne.Forms.__MetaClassElementFieldData);
                    metaClassField.set(_DatenMeister._Forms._MetaClassElementFieldData.name, "Metaclass");
                    fields.Add(metaClassField);

                    FormMethods.AddToFormCreationProtocol(
                        extentForm,
                        "[FormCreator.CreateExtentFormForObject]: Added metaclass to DetailForm");

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
                        var form = MofFactory.create(_DatenMeister.TheOne.Forms.__TableForm);
                        form.set(_DatenMeister._Forms._TableForm.name, propertyName);
                        form.set(_DatenMeister._Forms._TableForm.property, propertyName);
                        form.set(_DatenMeister._Forms._TableForm.noItemsWithMetaClass, true);
                        form.set(_DatenMeister._Forms._TableForm.isAutoGenerated, true);

                        foreach (var item in elementsWithoutMetaClass)
                        {
                            AddFieldsToForm(form, item, creationMode, cache);
                        }

                        FormMethods.AddToFormCreationProtocol(
                            extentForm,
                            "[FormCreator.CreateExtentFormForObject]: Added Listform: " +
                            NamedElementMethods.GetName(pair.propertyType));
                        
                        FormMethods.RemoveDuplicatingDefaultNewTypes(form);

                        tabs.Add(form);
                    }

                    foreach (var group in elementsWithMetaClass)
                    {
                        // Now try to figure out the metaclass
                        var groupedMetaclass = group.Key ?? throw new InvalidOperationException("Key may not be null");
                        if (_formLogic != null && extent != null)
                        {
                            FormMethods.AddToFormCreationProtocol(
                                extentForm,
                                "[FormCreator.CreateExtentFormForObject]: Add Listform for metaclass:" +
                                NamedElementMethods.GetName(groupedMetaclass));

                            var form = _parentFormFactory.CreateListFormForPropertyValues(
                                element,
                                propertyName,
                                groupedMetaclass,
                                new FormFactoryConfiguration { AllowFormModifications = false });
                            if (form != null) tabs.Add(form);
                        }
                        else
                        {
                            FormMethods.AddToFormCreationProtocol(
                                extentForm,
                                "[FormCreator.CreateExtentFormForObject]: Add Listform for metaclass:" +
                                NamedElementMethods.GetName(groupedMetaclass));

                            tabs.Add(
                                CreateListFormForPropertyValues(null, pair.propertyName, groupedMetaclass,
                                    creationMode with { AllowFormModifications = false }));
                        }
                    }
                }
                else
                {
                    FormMethods.AddToFormCreationProtocol(
                        extentForm,
                        "[FormCreator.CreateExtentFormForObject]: Add Listform for by reflective collection: " +
                        propertyName);

                    // If there are elements included and they are filled
                    // OR, if there is no element included at all, create the corresponding list form
                    var form = _parentFormFactory.CreateListFormForCollection(
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
                            AddFieldsToFormByMetaClass(form, propertyType, creationMode, cache);
                            FormMethods.AddDefaultTypeForNewElement(form, propertyType);

                            FormMethods.AddToFormCreationProtocol(
                                form,
                                "[FormCreator.CreateExtentFormForObject]: Add DefaultTypeForNewElement driven by ExtentType: " +
                                NamedElementMethods.GetName(propertyType));
                        }
                    }
                }
            }

            // ReSharper restore HeuristicUnreachableCode
            extentForm.set(_DatenMeister._Forms._ObjectForm.tab, tabs);

            CleanupExtentForm(extentForm);
            return extentForm;
        }

        public void CleanupExtentForm(IElement extentForm)
        {
            
        }

    }
}