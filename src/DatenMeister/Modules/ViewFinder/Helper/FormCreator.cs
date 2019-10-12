using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.ViewFinder.Helper
{
    /// <summary>
    /// Creates a view out of the given extent, elements (collection) or element).
    /// 
    /// </summary>
    public class FormCreator
    {
        /// <summary>
        /// Stores the reference to the view logic which is required to get the views
        /// for the tabs of the extent form
        /// </summary>
        private readonly ViewLogic _viewLogic;

        /// <summary>
        /// Stores the associated workspace logic
        /// </summary>
        private readonly IWorkspaceLogic _workspaceLogic;

        /// <summary>
        /// Stores the factory to create the fields and forms
        /// </summary>
        private readonly IFactory _factory;

        /// <summary>
        /// Stores the information for form and fields including all metaclasses
        /// </summary>
        private readonly _FormAndFields _formAndFields;

        private IElement _stringType;
        private IElement _integerType;
        private IElement _booleanType;
        private IElement _realType;
        private IElement _dateTimeType;

        /// <summary>
        /// Stores the creation mode
        /// </summary>
        [Flags]
        public enum CreationMode
        {
            /// <summary>
            /// Allows the creation of forms by the metaclass of the given extent
            /// </summary>
            ByMetaClass = 0x01,

            /// <summary>
            /// Allows the creation of forms by going through the properties
            /// </summary>
            ByProperties = 0x02,

            /// <summary>
            /// Allowes the creation of forms by going through the propeerties only if
            /// the element does not have a metaclass
            /// </summary>
            OnlyPropertiesIfNoMetaClass = 0x04,

            /// <summary>
            /// Adds the metaclass itself to the form
            /// </summary>
            AddMetaClass = 0x08,

            /// <summary>
            /// Creates only fields that are usable in a list form.
            /// So most of the time only 'TextFields'.
            /// </summary>
            ForListForms = 0x10,

            /// <summary>
            /// Creates all properties that are possible
            /// </summary>
            All = ByMetaClass | ByProperties | AddMetaClass
        }

        public FormCreator(ViewLogic viewLogic)
        {
            _viewLogic = viewLogic;

            _workspaceLogic = _viewLogic?.WorkspaceLogic;
            var userExtent = _viewLogic?.GetUserViewExtent();
            _factory = userExtent != null
                ? new MofFactory(userExtent)
                : InMemoryObject.TemporaryFactory;
            _formAndFields = userExtent?.GetWorkspace().GetFromMetaWorkspace<_FormAndFields>()
                             ?? _FormAndFields.TheOne;
        }


        /// <summary>
        /// Creates a detail form by considering the the information that is stored within
        /// the given element. Dependent upon the creation mode, the form will be created
        /// by using the metaclass or the set properties
        /// </summary>
        /// <param name="element"></param>
        /// <param name="creationMode"></param>
        /// <returns></returns>
        public IElement CreateDetailForm(IObject element, CreationMode creationMode = CreationMode.All)
        {
            var cache = new FormCreatorCache();
            var createdForm = _factory.create(_formAndFields.__DetailForm);
            createdForm.set(_FormAndFields._DetailForm.name, "Item");

            if (creationMode.HasFlag(CreationMode.AddMetaClass))
            {
                createdForm.set(_FormAndFields._DetailForm.hideMetaInformation, true);
            }

            AddToForm(createdForm, element, creationMode, cache);

            return createdForm;
        }

        /// <summary>
        /// Creates a detail form by using the metaclass of the given element
        /// </summary>
        /// <param name="metaClass">Metaclass to which the form will be created</param>
        /// <param name="creationMode">The creation mode being used</param>
        /// <returns>The created form for the metaclass</returns>
        public IElement CreateDetailFormByMetaClass(IElement metaClass, CreationMode creationMode = CreationMode.All)
        {
            var createdForm = _factory.create(_formAndFields.__DetailForm);
            createdForm.set(_FormAndFields._DetailForm.name, "Item");

            if (creationMode.HasFlag(CreationMode.AddMetaClass))
            {
                createdForm.set(_FormAndFields._DetailForm.hideMetaInformation, true);
            }

            AddToFormByMetaclass(createdForm, metaClass, creationMode);

            return createdForm;
        }

        /// <summary>
        /// Creates an extent form containing the subforms
        /// </summary>
        /// <returns>The created extent</returns>
        public IElement CreateExtentForm(params IElement[] subForms)
        {
            var result = _factory.create(_formAndFields.__ExtentForm);
            result.set(_FormAndFields._ExtentForm.tab, subForms);
            return result;
        }

        /// <summary>
        /// Creates an extent form for the given extent by parsing through each element
        /// and creating the form out of the max elements
        /// </summary>
        /// <param name="extent">Extent to be parsed</param>
        /// <param name="creationMode">The creation mode being used</param>
        /// <returns>The created element</returns>
        public IElement CreateExtentForm(IUriExtent extent, CreationMode creationMode)
            => CreateExtentForm(extent.elements(), creationMode);

        private class FormCreatorCache
        {
            public bool MetaClassAlreadyAdded { get; set; }

            public HashSet<IElement> CoveredMetaClasses { get; } = new HashSet<IElement>();

            public HashSet<string> CoveredPropertyNames { get; } = new HashSet<string>();
        }

        /// <summary>
        /// Creates the extent by parsing through all the elements and creation of fields
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
                    return element != null && element.getMetaClass() == null
                           || x is MofObjectShadow objectShadow && objectShadow.getMetaClass() == null
                           || element == null;
                })
                .ToList();

            var elementsWithMetaClass = elementsAsObjects
                .OfType<IElement>()
                .GroupBy(x =>
                {
                    var metaClass = x.getMetaClass();
                    return metaClass is MofObjectShadow ? null : metaClass;
                })
                .Where(x => x.Key != null);

            if (elementsWithoutMetaClass.Any() || elementsAsObjects.Count == 0)
            {
                // If there are elements without a metaclass or if there are no elements in the extent
                // then provide an empty list form
                var form = _factory.create(_formAndFields.__ListForm);
                form.set(_FormAndFields._ListForm.name, "Unclassified");
                form.set(_FormAndFields._ListForm.noItemsWithMetaClass, true);

                foreach (var item in elementsWithoutMetaClass)
                    AddToForm(form, item, creationMode, cache);

                SetDefaultTypes(form);

                tabs.Add(form);
            }

            foreach (var group in elementsWithMetaClass)
            {
                // Now try to figure out the metaclass
                var groupedMetaclass = group.Key;
                IElement form;
                if (_viewLogic != null)
                {
                    // Asks the view logic whether it has a list form for the specific metaclass
                    // It will ask the form creator, if there is no view association directly referencing
                    // to the element
                    form = _viewLogic.GetListFormForExtent(
                        (elements as IHasExtent)?.Extent,
                        groupedMetaclass,
                        ViewDefinitionMode.Default);
                }
                else
                {
                    // If no view logic is given, then ask directly the form creator.
                    form = CreateListForm(groupedMetaclass, creationMode);
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
        /// Creates the list form out of the elements in the reflective collection.
        /// Supports the creation by the metaclass and by the object's properties
        /// </summary>
        /// <param name="elements">Elements to be queried</param>
        /// <param name="creationMode">The used creation mode</param>
        /// <returns>The created list form </returns>
        public IElement CreateListForm(IReflectiveCollection elements, CreationMode creationMode)
        {
            var cache = new FormCreatorCache();

            var alreadyVisitedMetaClasses = new HashSet<IElement>();

            var result = _factory.create(_formAndFields.__ListForm);

            foreach (var element in elements.OfType<IObject>())
            {
                var metaClass = (element as IElement)?.getMetaClass();

                if (creationMode.HasFlag(CreationMode.ByMetaClass) && metaClass != null)
                {
                    if (alreadyVisitedMetaClasses.Contains(metaClass))
                    {
                        continue;
                    }

                    alreadyVisitedMetaClasses.Add(metaClass);
                    AddToFormByMetaclass(result, metaClass, creationMode);
                }
                else if (creationMode.HasFlag(CreationMode.ByProperties))
                {
                    AddToForm(result, element, CreationMode.ByProperties, cache);
                }
            }

            return result;
        }

        /// <summary>
        /// Creates a list form for a certain metaclass
        /// </summary>
        /// <param name="metaClass"></param>
        /// <param name="creationMode"></param>
        public IElement CreateListForm(IElement metaClass, CreationMode creationMode)
        {
            if (!creationMode.HasFlag(CreationMode.ByMetaClass))
            {
                throw new InvalidOperationException("The list form will only be created for the metaclass");
            }

            var result = _factory.create(_formAndFields.__ListForm);

            result.set(_FormAndFields._ListForm.title, $"{NamedElementMethods.GetName(metaClass)}");
            AddToFormByMetaclass(result, metaClass, creationMode);

            var defaultType = _factory.create(_formAndFields.__DefaultTypeForNewElement);
            defaultType.set(_FormAndFields._DefaultTypeForNewElement.metaClass, metaClass);
            result.set(_FormAndFields._ListForm.defaultTypesForNewElements, new[] {defaultType});

            return result;
        }

        /// <summary>
        /// Creates the form out of the given element.
        /// </summary>
        /// <param name="form">Form which will be extended by the given object</param>
        /// <param name="item">Item being used</param>
        /// <param name="creationMode">Creation mode for the form. Whether by metaclass or ByProperties</param>
        /// <param name="cache">Cache being used to store intermediate items</param>
        private void AddToForm(IElement form, object item, CreationMode creationMode, FormCreatorCache cache)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            // First phase: Get the properties by using the metaclass
            var asElement = item as IElement;
            var metaClass = asElement?.metaclass;
            var wasInMetaClass = false;

            if (creationMode.HasFlag(CreationMode.ByMetaClass)
                && metaClass != null)
            {
                if (!cache.CoveredMetaClasses.Contains(metaClass))
                {
                    cache.CoveredMetaClasses.Add(metaClass);
                    wasInMetaClass = AddToFormByMetaclass(form, metaClass, creationMode);
                }
                else
                {
                    wasInMetaClass = true;
                }
            }

            // Second phase: Get properties by the object itself
            // This item does not have a metaclass and also no properties, so we try to find them by using the item
            var itemAsAllProperties = item as IObjectAllProperties;

            var isByProperties =
                creationMode.HasFlag(CreationMode.ByProperties);
            var isOnlyPropertiesIfNoMetaClass =
                creationMode.HasFlag(CreationMode.OnlyPropertiesIfNoMetaClass);

            if ((isByProperties
                 || (isOnlyPropertiesIfNoMetaClass && !wasInMetaClass))
                && itemAsAllProperties != null)
            {
                AddToFormByProperties(form, item, creationMode, cache);
            }

            // Third phase: Add metaclass element itself
            var isMetaClass = creationMode.HasFlag(CreationMode.AddMetaClass);
            if (!cache.MetaClassAlreadyAdded &&
                isMetaClass &&
                !form
                    .get<IReflectiveCollection>(_FormAndFields._Form.field)
                    .OfType<IElement>()
                    .Any(x => x.getMetaClass()?.@equals(_formAndFields.__MetaClassElementFieldData) ?? false))
            {
                // Sets the information in cache, that the element was already added
                cache.MetaClassAlreadyAdded = true;

                // Add the element itself
                var metaClassField = _factory.create(_formAndFields.__MetaClassElementFieldData);
                form.get<IReflectiveCollection>(_FormAndFields._Form.field).add(metaClassField);
            }
        }

        /// <summary>
        /// Adds the fields to the properties as given in the object itself.
        /// The properties are retrieved by reading the available property types
        /// from the object itself via the interface IObjectAllProperties
        /// </summary>
        /// <param name="form">Form to be extended</param>
        /// <param name="item">Item to be evaluated</param>
        /// <param name="creationMode">The creation mode that is used</param>
        /// <param name="cache">Cache being used to store intermediate items</param>
        private void AddToFormByProperties(IElement form, object item, CreationMode creationMode, FormCreatorCache cache)
        {
            if (!(item is IObjectAllProperties itemAsAllProperties))
            {
                // The object does not allow the retrieving of properties
                return;
            }

            if (!(item is IObject itemAsObject))
            {
                // The object cannot be converted and FormCreator does not support
                // non MOF Objects
                return;
            }

            // Creates the form out of the properties of the item
            var properties = itemAsAllProperties.getPropertiesBeingSet();

            foreach (var property in properties
                .Where(property => !cache.CoveredPropertyNames.Contains(property)))
            {
                cache.CoveredPropertyNames.Add(property);

                // Checks, whether a form is already existing
                var column = form
                    .get<IReflectiveCollection>(_FormAndFields._Form.field)
                    .OfType<IObject>()
                    .FirstOrDefault(x => x.getOrDefault<string>(_FormAndFields._FieldData.name) == property);

                var propertyValue = itemAsObject.GetOrDefault(property);

                if (column == null)
                {
                    // Guess by content, which type of field shall be created
                    var propertyType = propertyValue?.GetType();
                    if (propertyType == null)
                    {
                        // No propertyType ==> propertyValue is null and nothing is generated
                        continue;
                    }

                    if (DotNetHelper.IsPrimitiveType(propertyType) || creationMode.HasFlag(CreationMode.ForListForms))
                    {
                        column = _factory.create(_formAndFields.__TextFieldData);
                    }
                    else
                    {
                        if (DotNetHelper.IsEnumeration(propertyType))
                        {
                            column = _factory.create(_formAndFields.__SubElementFieldData);
                        }
                        else
                        {
                            column = _factory.create(_formAndFields.__ReferenceFieldData);
                            column.set(_FormAndFields._ReferenceFieldData.isSelectionInline, false);
                        }
                    }

                    column.set(_FormAndFields._TextFieldData.name, property);
                    column.set(_FormAndFields._TextFieldData.title, property);

                    form.get<IReflectiveCollection>(_FormAndFields._Form.field).add(column);
                }

                // Makes the field to an enumeration, if explicitly requested or the type behind is an enumeration
                column.set(
                    _FormAndFields._FieldData.isEnumeration,
                    column.getOrDefault<bool>(_FormAndFields._FieldData.isEnumeration) | DotNetHelper.IsEnumeration(propertyValue?.GetType()));
            }
        }

        /// <summary>
        /// Adds the fields for the form by going through the properties of the metaclass
        /// </summary>
        /// <param name="form">Form that will be extended</param>
        /// <param name="metaClass">Metaclass to be used</param>
        /// <param name="creationMode">Creation Mode to be used</param>
        /// <returns>true, if the metaclass is not null and if the metaclass contains at least on</returns>
        private bool AddToFormByMetaclass(IElement form, IElement metaClass, CreationMode creationMode)
        {
            var wasInMetaClass = false;
            if (metaClass == null)
            {
                return false;
            }

            var classifierMethods = ClassifierMethods.GetPropertiesOfClassifier(metaClass).Where(x => x.isSet("name")).ToList();
            foreach (var property in classifierMethods)
            {
                wasInMetaClass = true;
                var propertyName = property.get("name").ToString();

                var isAlreadyIn = form
                    .get<IReflectiveCollection>(_FormAndFields._Form.field)
                    .OfType<IObject>()
                    .Any(x => x.getOrDefault<string>(_FormAndFields._FieldData.name) == propertyName);

                if (isAlreadyIn)
                {
                    continue;
                }

                var column = GetFieldForProperty(property, creationMode);
                form.get<IReflectiveCollection>(_FormAndFields._Form.field).add(column);
            }

            return wasInMetaClass;
        }

        /// <summary>
        /// Gets the field data, depending upon the given property
        /// </summary>
        /// <param name="property">Property which is requesting a field</param>
        /// <param name="creationMode">Defines the mode how to create the fields</param>
        /// <returns>The field data</returns>
        public IElement GetFieldForProperty(IElement property, CreationMode creationMode)
        {
            var propertyType = PropertyMethods.GetPropertyType(property);
            var isForListForm = creationMode.HasFlag(CreationMode.ForListForms);
            var propertyName = property.get<string>("name");
            var propertyIsEnumeration = PropertyMethods.IsCollection(property);

            // Check, if field property is an enumeration
            var uml = _workspaceLogic.GetUmlData();
            var primitiveTypes = _workspaceLogic.GetPrimitiveData();
            var uriResolver = _workspaceLogic.GetTypesWorkspace();

            //var uriResolver = propertyType.GetUriResolver();
            _stringType = _stringType ?? primitiveTypes.__String;
            _integerType = _integerType ?? primitiveTypes.__Integer;
            _booleanType = _booleanType ?? primitiveTypes.__Boolean;
            _realType = _realType ?? primitiveTypes.__Real;
            _dateTimeType = _dateTimeType ?? uriResolver.Resolve(CoreTypeNames.DateTimeType, ResolveType.Default, false);

            // Checks, if the property is an enumeration.
            if (propertyType?.metaclass != null)
            {
                if (propertyType.metaclass.@equals(uml.SimpleClassifiers.__Enumeration) && !isForListForm)
                {
                    // If we have an enumeration (C#: Enum) and the field is not for a list form
                    var comboBox = _factory.create(_formAndFields.__DropDownFieldData);
                    comboBox.set(_FormAndFields._DropDownFieldData.name, propertyName);
                    comboBox.set(_FormAndFields._DropDownFieldData.title, propertyName);

                    var values = EnumerationMethods.GetEnumValues(propertyType);
                    comboBox.set(
                        _FormAndFields._DropDownFieldData.values,
                        values.Select(x =>
                        {
                            var data = _factory.create(_formAndFields.__ValuePair);
                            data.set(_FormAndFields._ValuePair.name, x);
                            data.set(_FormAndFields._ValuePair.value, x);
                            return data;
                        }).ToList());
                    return comboBox;
                }

                if (propertyType.@equals(_booleanType) && !isForListForm)
                {
                    // If we have a boolean and the field is not for a list form
                    var checkbox = _factory.create(_formAndFields.__CheckboxFieldData);
                    checkbox.set(_FormAndFields._CheckboxFieldData.name, propertyName);
                    checkbox.set(_FormAndFields._CheckboxFieldData.title, propertyName);
                    return checkbox;
                }

                if (
                    !propertyType.@equals(_stringType) &&
                    !propertyType.@equals(_integerType) &&
                    !propertyType.@equals(_realType) &&
                    !propertyType.@equals(_dateTimeType) && // TODO: Needs to be changed to DateTime Type
                    !isForListForm)
                {
                    // If we have something else than a primitive type and it is not for a list form
                    if (propertyIsEnumeration)
                    {
                        // It can contain multiple elements
                        var elements = _factory.create(_formAndFields.__SubElementFieldData);
                        elements.set(_FormAndFields._SubElementFieldData.name, propertyName);
                        elements.set(_FormAndFields._SubElementFieldData.title, propertyName);
                        elements.set(_FormAndFields._SubElementFieldData.defaultTypesForNewElements,
                            ClassifierMethods.GetSpecializations(propertyType).ToList());

                        return elements;
                    }

                    // It can just contain one element
                    var reference = _factory.create(_formAndFields.__ReferenceFieldData);
                    reference.set(_FormAndFields._ReferenceFieldData.name, propertyName);
                    reference.set(_FormAndFields._ReferenceFieldData.title, propertyName);

                    return reference;
                }
            }

            if (propertyType == null)
            {
                // If we have something else than a primitive type and it is not for a list form
                var element = _factory.create(propertyIsEnumeration
                    ? _formAndFields.__SubElementFieldData
                    : _formAndFields.__ReferenceFieldData);

                // It can just contain one element
                element.set(_FormAndFields._SubElementFieldData.name, propertyName);
                element.set(_FormAndFields._SubElementFieldData.title, propertyName);
                return element;

                /*// Unknown property type, so just create something
                var columnNoPropertyType = _factory.create(_formAndFields.__TextFieldData);
                columnNoPropertyType.set(_FormAndFields._TextFieldData.name, propertyName);
                columnNoPropertyType.set(_FormAndFields._TextFieldData.title, propertyName);

                return columnNoPropertyType;*/
            }

            // Per default, assume some kind of text
            var column = _factory.create(_formAndFields.__TextFieldData);
            column.set(_FormAndFields._TextFieldData.name, propertyName);
            column.set(_FormAndFields._TextFieldData.title, propertyName);

            // If propertyType is an integer, the field can be smaller
            if (propertyType.@equals(_integerType))
            {
                column.set(_FormAndFields._TextFieldData.width, 10);
            }

            return column;
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
            var properties = (element as IObjectAllProperties)?.getPropertiesBeingSet().ToList();
            if (properties == null)
            {
                throw new InvalidOperationException("ExtentForm cannot be created because given element is not of Type IObjectAllProperties");
            }

            var propertiesWithCollection =
                from p in properties
                where element.IsPropertyOfType<IReflectiveCollection>(p)
                let propertyContent = element.get<IReflectiveCollection>(p)
                where propertyContent != null
                select new {propertyName = p, propertyContent};

            var propertiesWithoutCollection =
                (from p in properties
                    where !element.IsPropertyOfType<IReflectiveCollection>(p)
                    let propertyContent = element.get(p)
                    where propertyContent != null
                    select new {propertyName = p, propertyContent}).ToList();

            if (propertiesWithoutCollection.Any())
            {
                var detailForm = _factory.create(_formAndFields.__DetailForm);
                detailForm.set(_FormAndFields._DetailForm.name, "Detail");

                var fields = new List<IElement>();

                foreach (var pair in propertiesWithoutCollection)
                {
                    if (objectMetaClass != null)
                    {
                        var property = ClassifierMethods.GetPropertyOfClassifier(objectMetaClass, pair.propertyName);
                        var field = GetFieldForProperty(property, CreationMode.All);
                        fields.Add(field);
                    }
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

                if (elementsWithoutMetaClass.Any())
                {
                    var form = _factory.create(_formAndFields.__ListForm);
                    form.set(_FormAndFields._ListForm.name, "Unclassified");
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
                    var form = _viewLogic.GetListFormForExtentForPropertyInObject(
                        element,
                        extent,
                        pair.propertyName,
                        groupedMetaclass,
                        ViewDefinitionMode.Default);

                    tabs.Add(form);
                }
            }

            extentForm.set(_FormAndFields._ExtentForm.tab, tabs);

            return extentForm;
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
            result.set(_FormAndFields._ListForm.title, $"{propertyName} - {NamedElementMethods.GetName(metaClass)}");
            return result;
        }
    }
}