#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
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
        /// Stores the configuration whether we require a tab for each property
        /// </summary>
        private const bool ConfigurationFormCreatorSeparateProperties = true;

        public IElement CreateExtentFormForItem(IObject element, FormFactoryConfiguration configuration)
        {
            var detailForm = CreateDetailFormForItem(element, configuration);

            return CreateExtentFormFromTabs(new MofFactory(detailForm), detailForm);
        }
        
        /// <summary>
        /// Checks whether a detail form is already within the element form.
        /// If yes, then it is directly returned, otherwise a new detail form is created and added to the form
        /// </summary>
        /// <param name="extentForm">extentForm to be evaluated</param>
        public IElement GetOrCreateDetailFormIntoExtentForm(IElement extentForm)
        {
            var tabs = extentForm.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._ExtentForm.tab);

            foreach (var tab in tabs.OfType<IElement>())
            {
                if (ClassifierMethods.IsSpecializedClassifierOf(
                    tab.getMetaClass(),
                    _DatenMeister.TheOne.Forms.__DetailForm))
                {
                    return tab;
                }
            }
            
            // Create new one
            var newTab = new MofFactory(extentForm).create(_DatenMeister.TheOne.Forms.__DetailForm);
            tabs.add(newTab);
            return newTab;
        }

        /// <summary>
        /// Creates an extent form containing the subforms
        /// </summary>
        /// <param name="factory">The factory being used</param>
        /// <param name="tabsAsForms">The Forms which are converted to an extent form</param>
        /// <returns>The created extent</returns>
        public static IElement CreateExtentFormFromTabs(IFactory? factory, params IElement[] tabsAsForms)
        {
            factory ??= new MofFactory(tabsAsForms.First());
            var result = factory.create(_DatenMeister.TheOne.Forms.__ExtentForm);
            result.set(_DatenMeister._Forms._ExtentForm.tab, tabsAsForms);
            return result;
        }

        /// <summary>
        /// Creates an extent form for the given extent by parsing through each element
        /// and creating the form out of the max elements
        /// </summary>
        /// <param name="extent">Extent to be parsed</param>
        /// <param name="creationMode">The creation mode being used</param>
        /// <returns>The created element</returns>
        public IElement CreateExtentFormForExtent(IExtent extent, FormFactoryConfiguration creationMode)
        {
            var extentFormConfiguration = new ExtentFormConfiguration();
            var extentTypes = extent.GetConfiguration().ExtentTypes;
            extentFormConfiguration.ExtentTypes.AddRange(extentTypes);
            
            return CreateExtentFormForCollection(extent.elements(), creationMode, extentFormConfiguration);
        }

        /// <summary>
        /// Creates the extent by parsing through all the elements and creation of fields.
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

            var result = mofFactory.create(_DatenMeister.TheOne.Forms.__ExtentForm);
            result.set(_DatenMeister._Forms._ExtentForm.name, "Items");

            var elementsAsObjects = elements.OfType<IObject>().ToList();
            var elementsWithoutMetaClass = elementsAsObjects
                .Where(x =>
                {
                    var element = x as IElement;
                    var metaClass = element?.getMetaClass();
                    return metaClass == null || metaClass is MofObjectShadow;
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

                metaClasses.AddRange(extentTypeSetting.rootElementMetaClasses);
            }

            // Create the tab for the elements of without any metaclass
            if (elementsWithoutMetaClass.Any() || elementsAsObjects.Count == 0)
            {
                // If there are elements without a metaclass or if there are no elements at all within the extent
                // then provide an empty list form
                var form = mofFactory.create(_DatenMeister.TheOne.Forms.__ListForm);
                form.set(_DatenMeister._Forms._ListForm.name, "Unclassified");
                form.set(_DatenMeister._Forms._ListForm.noItemsWithMetaClass, true);

                foreach (var item in elementsWithoutMetaClass)
                    AddFieldsToForm(form, item, configuration, cache);

                AddTextFieldForNameIfNoFieldAvailable(form);
                SortFieldsByImportantProperties(form);
                
                SetDefaultTypesByPackages(form);

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
                    // It will ask the form reportCreator, if there is no view association directly referencing
                    // to the element
                    var formCreator = new FormFactory(_formLogic, _scopeStorage);
                    form = formCreator.CreateListFormForCollection(
                        extent.elements(),
                        new FormFactoryConfiguration() { AllowFormModifications = false }) ?? throw new InvalidOperationException("No form was found");

                    if (configuration.CreateByMetaClass)
                    {
                        foreach (var element in group)
                        {
                            AddFieldsToFormByPropertyValues(form, element, configuration, cache);
                        }
                    }
                }
                else
                {
                    // If no view logic is given, then ask directly the form reportCreator.
                    form = CreateListFormForMetaClass(groupedMetaclass, configuration);
                }

                form.set(_DatenMeister._Forms._ListForm.metaClass, groupedMetaclass);

                SetDefaultTypesByPackages(form);

                tabs.Add(form);
            }

            result.set(_DatenMeister._Forms._ExtentForm.tab, tabs);
            return result;

            // Some helper method which creates the button to create new elements by the extent being connected
            // to the enumeration of elements
            void SetDefaultTypesByPackages(IObject form)
            {
                var extent = elements.GetAssociatedExtent();
                var defaultTypePackages = extent?.GetConfiguration().GetDefaultTypePackages();
                if (defaultTypePackages != null)
                {
                    var currentDefaultPackages =
                        form.get<IReflectiveCollection>(_DatenMeister._Forms._ListForm.defaultTypesForNewElements);
                    
                    // Now go through the packages and pick the classifier and add them to the list
                    foreach (var package in defaultTypePackages)
                    {
                        var childItems =
                            package.getOrDefault<IReflectiveCollection>(_UML._Packages._Package.packagedElement);
                        if (childItems == null) continue;

                        foreach (var type in childItems.OfType<IElement>())
                        {
                            if (type.equals(_UML.TheOne.StructuredClassifiers.__Class))
                            {
                                var defaultType = mofFactory.create(_DatenMeister.TheOne.Forms.__DefaultTypeForNewElement);
                                defaultType.set(_DatenMeister._Forms._DefaultTypeForNewElement.metaClass, package);
                                defaultType.set(_DatenMeister._Forms._DefaultTypeForNewElement.name, NamedElementMethods.GetName(package));
                                currentDefaultPackages.add(defaultType);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the extent form for the given metaclass.
        /// This method is used when the user selects a metaclass to which a form shall be created
        /// </summary>
        /// <param name="metaClass">The meta class for which the extent form shall be created</param>
        /// <param name="creationMode">Defines the creation mode</param>
        /// <returns>The created extent form</returns>
        public IElement CreateExtentFormForItemsMetaClass(IElement metaClass, FormFactoryConfiguration? creationMode = null)
        {
            creationMode ??= new FormFactoryConfiguration();
            
            var extentForm = mofFactory.create(_DatenMeister.TheOne.Forms.__ExtentForm);
            extentForm.set(_DatenMeister._Forms._ExtentForm.name, NamedElementMethods.GetName(metaClass) + " - List");

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

            if (propertiesWithoutCollection.Any() || creationMode.AutomaticMetaClassField)
            {
                var detailForm = mofFactory.create(_DatenMeister.TheOne.Forms.__DetailForm);
                detailForm.set(_DatenMeister._Forms._DetailForm.name, "Detail");

                var fields = new List<IElement>();
                foreach (var property in propertiesWithoutCollection)
                {
                    var field = CreateFieldForProperty(
                        metaClass,
                        property.property, 
                        null,
                        new FormFactoryConfiguration {IsReadOnly = true});
                    fields.Add(field);
                }

                if (creationMode.AutomaticMetaClassField
                    || !FormMethods.HasMetaClassFieldInForm(detailForm))
                {
                    // Add the element itself
                    var metaClassField = mofFactory.create(_DatenMeister.TheOne.Forms.__MetaClassElementFieldData);
                    metaClassField.set(_DatenMeister._Forms._MetaClassElementFieldData.name, "Metaclass");
                    fields.Add(metaClassField);
                }

                detailForm.set(_DatenMeister._Forms._DetailForm.field, fields);
                tabs.Add(detailForm);
            }

            foreach (var pair in propertiesWithCollection)
            {
                var propertyType = PropertyMethods.GetPropertyType(pair.property);
                // Now try to figure out the metaclass
                var form = CreateListFormForMetaClass(
                    propertyType,
                    new FormFactoryConfiguration { CreateByPropertyValues = false, AutomaticMetaClassField = false },
                    pair.property);

                tabs.Add(form);
            }

            extentForm.set(_DatenMeister._Forms._ExtentForm.tab, tabs);

            return extentForm;
        }

        public class P
        {
            public string PropertyName { get; set; } = string.Empty;
            
            public IElement? PropertyType { get; set; }

            public class PropertyNameEqualityComparer : IEqualityComparer<P>
            {
                public bool Equals(P? x, P? y)
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
        /// This is the typical method that is used to create the form via the FormFinder
        /// </summary>
        /// <param name="element">Element which shall be shown</param>
        /// <param name="extent">Extent containing the element</param>
        /// <param name="creationMode">The creation mode for auto-generation of the fields</param>
        /// <returns>Created Extent form as MofObject</returns>
        public IElement CreateExtentFormForObject(IObject element, IExtent extent, FormFactoryConfiguration creationMode)
        {
            if (_workspaceLogic == null)
                throw new InvalidOperationException("WorkspaceLogic is null");
            
            var cache = new FormCreatorCache();

            // Creates the empty form
            var extentForm = mofFactory.create(_DatenMeister.TheOne.Forms.__ExtentForm);
            extentForm.set(_DatenMeister._Forms._ExtentForm.name, NamedElementMethods.GetName(element));
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
                select new {propertyName = p.PropertyName, propertyType = p.PropertyType, propertyContent};

            var propertiesWithoutCollection =
                (from p in propertyNamesWithoutCollection.Distinct(new P.PropertyNameEqualityComparer())
                    let propertyContent = element.getOrDefault<object>(p.PropertyName)
                    select new {propertyName = p.PropertyName, propertyType = p.PropertyType, propertyContent})
                .ToList();

            //
            // Now, we got
            // propertyNamesWithCollection containing all properties which have a collection as a subitem ==> List Form
            // propertyNamesWithoutCollection containing all properties which have a collection as a subitem ==> Detail Form
            if (propertiesWithoutCollection.Any() || creationMode.AutomaticMetaClassField)
            {
                var detailForm = mofFactory.create(_DatenMeister.TheOne.Forms.__DetailForm);
                detailForm.set(_DatenMeister._Forms._DetailForm.name, "Detail");

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
                            creationMode with { IsReadOnly = true });
                        fields.Add(field);
                    }
                }

                if (!cache.MetaClassAlreadyAdded
                    && creationMode.AutomaticMetaClassField
                    && extent != null
                    && (_workspaceLogic == null ||
                        !new FormMethods(_workspaceLogic, _scopeStorage).HasMetaClassFieldInForm(extent, fields)))
                {
                    // Add the element itself
                    var metaClassField = mofFactory.create(_DatenMeister.TheOne.Forms.__MetaClassElementFieldData);
                    metaClassField.set(_DatenMeister._Forms._MetaClassElementFieldData.name, "Metaclass");
                    fields.Add(metaClassField);

                    cache.MetaClassAlreadyAdded = true;
                }

                detailForm.set(_DatenMeister._Forms._DetailForm.field, fields);
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
                        if (x is IElement innerElement)
                        {
                            return innerElement.getMetaClass() == null;
                        }

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
                        var form = mofFactory.create(_DatenMeister.TheOne.Forms.__ListForm);
                        form.set(_DatenMeister._Forms._ListForm.name, propertyName);
                        form.set(_DatenMeister._Forms._ListForm.property, propertyName);
                        form.set(_DatenMeister._Forms._ListForm.noItemsWithMetaClass, true);

                        foreach (var item in elementsWithoutMetaClass)
                        {
                            AddFieldsToForm(form, item, creationMode, cache);
                        }

                        tabs.Add(form);
                    }

                    foreach (var group in elementsWithMetaClass)
                    {
                        // Now try to figure out the metaclass
                        var groupedMetaclass = group.Key ?? throw new InvalidOperationException("Key may not be null");
                        if (_formLogic != null && extent != null)
                        {
                            var form = _parentFormFactory.CreateListFormForPropertyValues(
                                element,
                                propertyName,
                                groupedMetaclass,
                                new FormFactoryConfiguration() { AllowFormModifications = false });
                            if (form != null)
                            {
                                tabs.Add(form);
                            }
                        }
                        else
                        {
                            tabs.Add(
                                CreateListFormForPropertyValues(null, pair.propertyName, groupedMetaclass, creationMode with { AllowFormModifications = false }));
                        }
                    }
                }
                else
                {                    
                    // If there are elements included and they are filled
                    // OR, if there is no element included at all, create the corresponding list form
                    var form = _parentFormFactory.CreateListFormForCollection(
                        new TemporaryReflectiveCollection(elementsAsObjects), 
                        creationMode with { AllowFormModifications = false });
                    if (form != null)
                    {
                        form.set(_DatenMeister._Forms._ListForm.name, $"Property: {propertyName}");
                        form.set(_DatenMeister._Forms._ListForm.property, propertyName);

                        // Adds the form to the tabs
                        tabs.Add(form);
                    }
                }
            }

            // ReSharper restore HeuristicUnreachableCode
            extentForm.set(_DatenMeister._Forms._ExtentForm.tab, tabs);

            return extentForm;
        }

        private void SortFieldsByImportantProperties(IObject form)
        {
            var fields = form.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._ListForm.field);
            if (fields == null) return;
            var fieldsAsList = fields.OfType<IElement>().ToList();

            // Check if the name is within the list, if yes, push it to the front
            var fieldName = fieldsAsList.FirstOrDefault(x =>
                x.getOrDefault<string>(_UML._CommonStructure._NamedElement.name) ==
                _UML._CommonStructure._NamedElement.name);

            if (fieldName != null)
            {
                fieldsAsList.Remove(fieldName);
                fieldsAsList.Insert(0, fieldName);
            }
            
            // Sets it
            form.set(_DatenMeister._Forms._ListForm.field, fieldsAsList);
        }

        /// <summary>
        /// Checks whether at least one field is given.
        /// If no field is given, then the one text field for the name will be added
        /// </summary>
        /// <param name="form">Form to be checked</param>
        private void AddTextFieldForNameIfNoFieldAvailable(IObject form)
        {
            // If the field is empty, create an empty textfield with 'name' as a placeholder
            var fieldLength =
                form.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._ListForm.field)?.Count() ?? 0;
            if (fieldLength == 0)
            {
                var factory = new MofFactory(form);
                var textFieldData = factory.create(_DatenMeister.TheOne.Forms.__TextFieldData);
                textFieldData.set(_DatenMeister._Forms._TextFieldData.name, "name");
                textFieldData.set(_DatenMeister._Forms._TextFieldData.title, "name");

                form.AddCollectionItem(_DatenMeister._Forms._ListForm.field, textFieldData);
            }
        }
    }
    
    /// <summary>
    /// A configuration helper class to create the extent form
    /// </summary>
    public class ExtentFormConfiguration
    {
        /// <summary>
        /// Gets or sets the extent type to be used
        /// </summary>
        public List<string> ExtentTypes { get; set; } = new List<string>();
    }
}