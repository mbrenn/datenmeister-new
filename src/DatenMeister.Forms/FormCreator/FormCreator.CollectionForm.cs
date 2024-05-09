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
using DatenMeister.Core.Uml.Helper;

namespace DatenMeister.Forms.FormCreator
{
    public partial class FormCreator
    {
        /// <summary>
        ///     Stores the configuration whether we require a tab for each property
        /// </summary>
        private const bool ConfigurationFormCreatorSeparateProperties = true;

        /// <summary>
        ///     Checks whether a detail form is already within the element form.
        ///     If yes, then it is directly returned, otherwise a new detail form is created and added to the form
        /// </summary>
        /// <param name="collectionOrObjectForm">extentForm to be evaluated</param>
        public IElement GetOrCreateRowFormIntoForm(IElement collectionOrObjectForm)
        {
            var tabs = collectionOrObjectForm.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._CollectionForm.tab);

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
            var newTab = new MofFactory(collectionOrObjectForm).create(_DatenMeister.TheOne.Forms.__RowForm);
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
        /// <param name="collection">Collection to be evaluated</param>
        /// <param name="creationMode">The creation mode being used</param>
        /// <returns>The created element</returns>
        public IElement CreateCollectionFormForExtent(
            IExtent extent,
            IReflectiveCollection collection,
            FormFactoryConfiguration creationMode)
        {
            var extentFormConfiguration = new CollectionFormConfiguration();
            var extentTypes = extent.GetConfiguration().ExtentTypes;
            extentFormConfiguration.ExtentTypes.AddRange(extentTypes);
           
            var form = CreateCollectionFormForCollection(
                collection.TakeFirst(100),
                creationMode,
                extentFormConfiguration);
            form.set(_DatenMeister._Forms._CollectionForm.name, $"Collection Form by '{extent}'");
            return form;
        }

        /// <summary>
        ///     Creates the extent by parsing through all the elements and creation of fields.
        /// </summary>
        /// <param name="elements">Elements which are parsed to create the form</param>
        /// <param name="configuration">The creation mode defining whether metaclass are used or not</param>
        /// <param name="extentFormConfiguration">Configuration of the extent form</param>
        /// <returns>The created form</returns>
        public IElement CreateCollectionFormForCollection(
            IReflectiveCollection elements,
            FormFactoryConfiguration configuration,
            CollectionFormConfiguration? extentFormConfiguration)
        {
            extentFormConfiguration ??= new CollectionFormConfiguration();
            
            var cache = new FormCreatorCache();
            if (elements == null)
                throw new ArgumentNullException(nameof(elements));

            var tabs = new List<IElement>();

            var result = GetMofFactory(configuration).create(_DatenMeister.TheOne.Forms.__CollectionForm);
            result.set(_DatenMeister._Forms._CollectionForm.name, "Items");
            result.set(_DatenMeister._Forms._Form.isAutoGenerated, true);

            FormMethods.AddToFormCreationProtocol(
                result,
                "[FormCreator.CreateCollectionFormForCollection] Using Form Creator");

            var elementsAsObjects = elements.OfType<IObject>().ToList();
            var elementsWithoutMetaClass = elementsAsObjects
                .Where(x =>
                {
                    var element = x as IElement;
                    var metaClass = element?.getMetaClass();
                    return metaClass is null; // or MofObjectShadow;
                })
                .ToList();

            var elementsWithMetaClass = elementsAsObjects
                .OfType<IElement>()
                .GroupBy(x =>
                {
                    var metaClass = x.getMetaClass();
                    return metaClass;// is MofObjectShadow ? null : metaClass;
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
                        $"[FormCreator.CreateCollectionFormForCollection] Adding listform for '{NamedElementMethods.GetName(extentMetaClass)}' for extent type '{extentTypeSetting.name}'");
                }
            }

            // Create the tab for the elements of without any metaclass
            if (elementsWithoutMetaClass.Any() || elementsAsObjects.Count == 0)
            {
                var form = _parentFormFactory.CreateTableFormForCollection(
                    new TemporaryReflectiveCollection(elementsWithoutMetaClass),
                    configuration with { IsForTableForm = true, AllowFormModifications = false });
                if (form == null)
                {
                    throw new InvalidOperationException("The form was not created... When it should have been.");
                }

                FormMethods.AddToFormCreationProtocol(
                    result,
                    "[FormCreator.CreateCollectionFormForCollection]: Create ListForm for unclassified elements");

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
                        "[FormCreator.CreateCollectionFormForCollection]: Create ListForm for metaclass: " +
                        NamedElementMethods.GetName(groupedMetaclass));

                    form = formCreator.CreateTableFormForMetaClass(
                               groupedMetaclass,
                               configuration with
                               {
                                   IsReadOnly = true, IsForTableForm = true, AllowFormModifications = false
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
                        "[FormCreator.CreateCollectionFormForCollection]: Create Default Type for metaclass: " +
                        NamedElementMethods.GetName(groupedMetaclass));
                    FormMethods.AddDefaultTypeForNewElement(form, groupedMetaclass);
                }
                else
                {
                    // If no view logic is given, then ask directly the form reportCreator.
                    form = CreateTableFormForMetaClass(groupedMetaclass,
                        configuration with { AllowFormModifications = false });
                }

                form.set(_DatenMeister._Forms._TableForm.metaClass, groupedMetaclass);
                tabs.Add(form);
            }

            result.set(_DatenMeister._Forms._CollectionForm.tab, tabs);
            CleanupCollectionForm(result);
            
            return result;
        }

        /// <summary>
        ///     Gets the extent form for the given metaclass.
        ///     This method is used when the user selects a metaclass to which a form shall be created
        /// </summary>
        /// <param name="metaClass">The meta class for which the extent form shall be created</param>
        /// <param name="creationMode">Defines the creation mode</param>
        /// <returns>The created extent form</returns>
        public IElement CreateCollectionFormForMetaClass(IElement metaClass,
            FormFactoryConfiguration? creationMode = null)
        {
            creationMode ??= new FormFactoryConfiguration();
            var factory = GetMofFactory(creationMode);

            var collectionForm = factory.create(_DatenMeister.TheOne.Forms.__CollectionForm);
            collectionForm.set(_DatenMeister._Forms._ObjectForm.name, $"ListForm for '{NamedElementMethods.GetName(metaClass)}'");
            collectionForm.set(_DatenMeister._Forms._ObjectForm.isAutoGenerated, true);
            
            FormMethods.AddToFormCreationProtocol(
                collectionForm,
                "[FormCreator.CreateCollectionFormForItemsMetaClass]: Add Extent by Metaclass: " +
                NamedElementMethods.GetName(metaClass));

            var tabs = new List<IElement>();

            // Get all properties of the elements
            var properties = ClassifierMethods.GetPropertiesOfClassifier(metaClass).ToList();
            if (properties == null)
                throw new InvalidOperationException(
                    "CollectionForm cannot be created because given element does not have properties");

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
                var rowForm = factory.create(_DatenMeister.TheOne.Forms.__RowForm);
                rowForm.set(_DatenMeister._Forms._RowForm.name, "Detail");
                rowForm.set(_DatenMeister._Forms._RowForm.isAutoGenerated, true);

                FormMethods.AddToFormCreationProtocol(
                    collectionForm,
                    "[FormCreator.CreateCollectionFormForItemsMetaClass]: Add DetailForm");

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
                        collectionForm,
                        "[FormCreator.CreateCollectionFormForItemsMetaClass]: Add Field to RowForm: " +
                        NamedElementMethods.GetName(field));
                }

                if (creationMode.AutomaticMetaClassField
                    || !FormMethods.HasMetaClassFieldInForm(rowForm))
                {
                    // Add the element itself
                    var metaClassField = factory.create(_DatenMeister.TheOne.Forms.__MetaClassElementFieldData);
                    metaClassField.set(_DatenMeister._Forms._MetaClassElementFieldData.name, "Metaclass");
                    fields.Add(metaClassField);

                    FormMethods.AddToFormCreationProtocol(
                        collectionForm,
                        "[FormCreator.CreateCollectionFormForItemsMetaClass]: Add MetaClass Field to RowForm");
                }

                rowForm.set(_DatenMeister._Forms._RowForm.field, fields);
                
                CleanupRowForm(rowForm);
                
                tabs.Add(rowForm);
            }

            foreach (var pair in propertiesWithCollection)
            {
                FormMethods.AddToFormCreationProtocol(
                    collectionForm,
                    "[FormCreator.CreateCollectionFormForItemsMetaClass]: Add ListForm: " +
                    NamedElementMethods.GetName(pair.property));

                var propertyType = PropertyMethods.GetPropertyType(pair.property);
                // Now try to figure out the metaclass
                var tableForm = CreateTableFormForMetaClass(
                    propertyType,
                    new FormFactoryConfiguration
                    {
                        CreateByPropertyValues = false, AutomaticMetaClassField = false, AllowFormModifications = false
                    },
                    pair.property);

                tabs.Add(tableForm);
            }

            collectionForm.set(_DatenMeister._Forms._CollectionForm.tab, tabs);

            CleanupCollectionForm(collectionForm);
            
            return collectionForm;
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
        public void CleanupCollectionForm(IElement extentForm)
        {
            
        }

        public void CleanupObjectForm(IElement extentForm)
        {
            
        }

    }
}