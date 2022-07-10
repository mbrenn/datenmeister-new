#nullable enable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Extent.Manager.Extents.Configuration;
using _PrimitiveTypes = DatenMeister.Core.Models.EMOF._PrimitiveTypes;

namespace DatenMeister.Forms.FormCreator
{
    /// <summary>
    ///     Creates a view out of the given extent, elements (collection) or element).
    /// </summary>
    public partial class FormCreator : IFormFactory
    {
        /// <summary>
        ///     Defines a small helper enumeration which can be used to define the type of the
        ///     umlElement for the method AddToFormByUmlElement.
        /// </summary>
        public enum FormUmlElementType
        {
            /// <summary>
            ///     Definition is not givne
            /// </summary>
            Unknown,

            /// <summary>
            ///     The element is a property
            /// </summary>
            Property,

            /// <summary>
            ///     The element is a class
            /// </summary>
            Class,

            /// <summary>
            ///     The element is an enumeration
            /// </summary>
            Enumeration
        }

        private readonly ExtentSettings _extentSettings;

        /// <summary>
        ///     Stores the reference to the view logic which is required to get the views
        ///     for the tabs of the extent form
        /// </summary>
        private readonly FormMethods? _formLogic;

        private readonly IScopeStorage _scopeStorage;

        /// <summary>
        ///     Stores the associated workspace logic
        /// </summary>
        private readonly IWorkspaceLogic _workspaceLogic;

        private IElement? _booleanType;
        private IElement? _dateTimeType;

        /// <summary>
        ///     Just intermediate memory
        /// </summary>
        private IFactory? _f;


        /// <summary>
        ///     Defines the parent form factory which is used to create subforms.
        /// </summary>
        private readonly IFormFactory _parentFormFactory;

        /// <summary>
        /// The cached real type
        /// </summary>
        private IElement? _realType;

        /// <summary>
        /// The cached integer type
        /// </summary>
        private IElement? _integerType;
        
        /// <summary>
        /// The cached string type
        /// </summary>
        private IElement? _stringType;
        private Workspace? _uriResolver;

        /// <summary>
        ///     Initializes a new instance of the FormCreator class
        /// </summary>
        /// <param name="workspaceLogic">The workspace logic to be used</param>
        /// <param name="scopeStorage">The scope storage</param>
        public FormCreator(
            IWorkspaceLogic workspaceLogic,
            IScopeStorage scopeStorage)
        {
            _formLogic = new FormMethods(workspaceLogic, scopeStorage);
            _scopeStorage = scopeStorage;
            _extentSettings = scopeStorage.Get<ExtentSettings>();
            _workspaceLogic = workspaceLogic;
            _parentFormFactory = this;
        }

        /// <summary>
        ///     Initializes a new instance of the FormCreator class
        /// </summary>
        /// <param name="workspaceLogic">The workspace logic to be used</param>
        /// <param name="scopeStorage">The scope storage</param>
        /// <param name="formLogic">View logic being used</param>
        /// <param name="parentFormFactory">The parent form factory</param>
        private FormCreator(
            IWorkspaceLogic workspaceLogic,
            FormMethods formLogic,
            IScopeStorage scopeStorage,
            IFormFactory? parentFormFactory)
        {
            _formLogic = formLogic;
            _scopeStorage = scopeStorage;
            _extentSettings = scopeStorage.Get<ExtentSettings>();
            _workspaceLogic = workspaceLogic;
            _parentFormFactory = parentFormFactory ?? this;
        }

        /// <summary>
        ///     Stores the factory to create the fields and forms
        /// </summary>
        private IFactory MofFactory
        {
            get
            {
                if (_f != null) return _f;

                var userExtent = _formLogic?.GetUserFormExtent(true);
                _f = userExtent != null
                    ? new MofFactory(userExtent)
                    : InMemoryObject.TemporaryFactory;

                return _f;
            }
        }

        /// <summary>
        ///     Creates the form logic by using the private constructor
        /// </summary>
        /// <param name="workspaceLogic">Workspace Logic to be evaluated</param>
        /// <param name="scopeStorage">Scope storage</param>
        /// <param name="parentFormFactory">
        ///     The parent factory which will be called in case a subform needs to be
        ///     created
        /// </param>
        /// <returns>The form creator</returns>
        public static FormCreator Create(
            IWorkspaceLogic workspaceLogic,
            IScopeStorage scopeStorage,
            IFormFactory? parentFormFactory = null)
        {
            var formLogic = new FormMethods(
                workspaceLogic, scopeStorage);
            return new FormCreator(
                workspaceLogic,
                formLogic,
                scopeStorage,
                parentFormFactory);
        }

        /// <summary>
        ///     Creates the fields of the form by evaluation of the given object.
        ///     Depending on the creation mode, the evaluation will be done by metaclass
        ///     or by evaluation of the properties.
        ///     This method is independent whether it is used in an list or extent form.
        /// </summary>
        /// <param name="form">Form which will be extended by the given object</param>
        /// <param name="item">Item being used</param>
        /// <param name="creationMode">Creation mode for the form. Whether by metaclass or ByProperties</param>
        /// <param name="cache">Cache being used to store intermediate items</param>
        private void AddFieldsToForm(
            IObject form,
            object item,
            FormFactoryConfiguration creationMode,
            FormCreatorCache cache)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            // First phase: Get the properties by using the metaclass
            var asElement = item as IElement;
            var metaClass = asElement?.metaclass;
            var wasInMetaClass = false;

            if (creationMode.CreateByMetaClass && metaClass != null)
            {
                if (!cache.CoveredMetaClasses.Contains(metaClass))
                {
                    cache.CoveredMetaClasses.Add(metaClass);
                    wasInMetaClass = AddFieldsToFormByMetaClass(
                        form,
                        metaClass,
                        creationMode with
                        {
                            AutomaticMetaClassField = false
                        },
                        cache);
                }
                else
                {
                    wasInMetaClass = true;
                }
            }

            // Second phase: Get properties by the object itself
            // This item does not have a metaclass and also no properties, so we try to find them by using the item
            var itemAsAllProperties = item as IObjectAllProperties;

            var isByProperties = creationMode.CreateByPropertyValues;
            var isOnlyPropertiesIfNoMetaClass = creationMode.OnlyCreateByValuesWhenMetaClassIsNotSet;

            if ((isByProperties
                 || isOnlyPropertiesIfNoMetaClass && !wasInMetaClass)
                && itemAsAllProperties != null)
            {
                AddFieldsToFormByPropertyValues(form, item, creationMode, cache);
                form.set(_DatenMeister._Forms._RowForm.allowNewProperties, true);
            }

            // Third phase: Add metaclass element itself
            var isMetaClass = creationMode.AutomaticMetaClassField;
            if (!cache.MetaClassAlreadyAdded
                && isMetaClass
                && !FormMethods.HasMetaClassFieldInForm(form))
            {
                // Add the element itself
                var metaClassField = MofFactory.create(_DatenMeister.TheOne.Forms.__MetaClassElementFieldData);
                metaClassField.set(_DatenMeister._Forms._MetaClassElementFieldData.name, "Metaclass");

                form.get<IReflectiveCollection>(_DatenMeister._Forms._RowForm.field).add(metaClassField);

                // Sets the information in cache, that the element was already added
                cache.MetaClassAlreadyAdded = true;
            }

#if DEBUG
            if (!FormMethods.ValidateForm(form))
                throw new InvalidOperationException("Something went wrong during creation of form");
#endif
        }

        /// <summary>
        ///     Adds the fields to the properties as given in the object itself.
        ///     The properties are retrieved by reading the available property types
        ///     from the object itself via the interface IObjectAllProperties
        /// </summary>
        /// <param name="form">Form to be extended</param>
        /// <param name="item">Item to be evaluated</param>
        /// <param name="creationMode">The creation mode that is used</param>
        /// <param name="cache">Cache being used to store intermediate items</param>
        private void AddFieldsToFormByPropertyValues(
            IObject form,
            object item,
            FormFactoryConfiguration creationMode,
            FormCreatorCache cache)
        {
            if (item is not IObjectAllProperties itemAsAllProperties)
                // The object does not allow the retrieving of properties
                return;

            if (item is not IObject itemAsObject)
                // The object cannot be converted and FormCreator does not support
                // non MOF Objects
                return;

            var isReadOnly = creationMode.IsReadOnly;

            // Creates the form out of the properties of the item
            var properties = itemAsAllProperties.getPropertiesBeingSet();

            var focusOnPropertyNames = cache.FocusOnPropertyNames.Any();

            foreach (var propertyName in properties
                         .Where(property => !cache.CoveredPropertyNames.Contains(property)))
            {
                cache.CoveredPropertyNames.Add(propertyName);
                if (focusOnPropertyNames && !cache.FocusOnPropertyNames.Contains(propertyName))
                    // Skip the property name, when we would like to have focus on certain property names
                    continue;

                // Checks, whether the field is already existing
                var column = form
                    .get<IReflectiveCollection>(_DatenMeister._Forms._RowForm.field)
                    .OfType<IObject>()
                    .FirstOrDefault(x => x.getOrDefault<string>(_DatenMeister._Forms._FieldData.name) == propertyName);

                var propertyValue = itemAsObject.getOrDefault<object>(propertyName);

                if (column == null)
                {
                    // Guess by content, which type of field shall be created
                    var propertyType = propertyValue?.GetType();
                    if (propertyType == null)
                        // No propertyType ==> propertyValue is null and nothing is generated
                        continue;

                    if (DotNetHelper.IsEnumeration(propertyType))
                    {
                        column = MofFactory.create(_DatenMeister.TheOne.Forms.__SubElementFieldData);
                    }
                    else if (propertyValue is IObject)
                    {
                        column = MofFactory.create(_DatenMeister.TheOne.Forms.__ReferenceFieldData);
                        column.set(_DatenMeister._Forms._ReferenceFieldData.isSelectionInline, false);
                    }
                    else
                    {
                        column = MofFactory.create(_DatenMeister.TheOne.Forms.__TextFieldData);
                    }

                    column.set(_DatenMeister._Forms._FieldData.name, propertyName);
                    column.set(_DatenMeister._Forms._FieldData.title, propertyName);
                    column.set(_DatenMeister._Forms._FieldData.isReadOnly, isReadOnly);

                    form.get<IReflectiveCollection>(_DatenMeister._Forms._RowForm.field).add(column);

                    FormMethods.AddToFormCreationProtocol(
                        form,
                        "[FormCreator.AddFieldsToFormByPropertyValues]: " + NamedElementMethods.GetName(column));
                }

                // Makes the field to an enumeration, if explicitly requested or the type behind is an enumeration
                column.set(
                    _DatenMeister._Forms._FieldData.isEnumeration,
                    column.getOrDefault<bool>(_DatenMeister._Forms._FieldData.isEnumeration) |
                    DotNetHelper.IsEnumeration(propertyValue?.GetType()));
            }

#if DEBUG
            if (!FormMethods.ValidateForm(form))
                throw new InvalidOperationException("Something went wrong during creation of form");
#endif
        }

        /// <summary>
        ///     Adds the fields for the form by going through the properties of the metaclass.
        ///     It only adds fields, when they are not already added to the given list or detail form
        /// </summary>
        /// <param name="form">Form that will be extended. Must be list or detail form.</param>
        /// <param name="metaClass">Metaclass to be used</param>
        /// <param name="configuration">Creation Mode to be used</param>
        /// <param name="cache">Cache of reportCreator cache</param>
        /// <returns>true, if the metaclass is not null and if the metaclass contains at least on</returns>
        private bool AddFieldsToFormByMetaClass(
            IObject form,
            IObject? metaClass,
            FormFactoryConfiguration configuration,
            FormCreatorCache? cache = null)
        {
            cache ??= new FormCreatorCache();

            var wasInMetaClass = false;
            if (metaClass == null) return false;

            var classifierMethods =
                ClassifierMethods.GetPropertiesOfClassifier(metaClass)
                    .Where(x => x.isSet("name")).ToList();
            var focusOnPropertyNames = cache.FocusOnPropertyNames.Any();

            foreach (var property in classifierMethods)
            {
                wasInMetaClass = true;
                var propertyName = property.get<string?>("name");

                if (propertyName == null ||
                    focusOnPropertyNames && !cache.FocusOnPropertyNames.Contains(propertyName))
                    // Skip the property name, when we would like to have focus on certain property names
                    continue;

                var isAlreadyIn = form
                    .get<IReflectiveCollection>(_DatenMeister._Forms._RowForm.field)
                    .OfType<IObject>()
                    .Any(x => x.getOrDefault<string>(_DatenMeister._Forms._FieldData.name) == propertyName);

                if (isAlreadyIn) continue;

                var column = CreateFieldForProperty(metaClass, property, propertyName, configuration);
                form.get<IReflectiveCollection>(_DatenMeister._Forms._RowForm.field).add(column);

                FormMethods.AddToFormCreationProtocol(form,
                    "[FormCreator.AddFieldsToFormByMetaclass]: Added field by Metaclass: " +
                    NamedElementMethods.GetName(column));
            }

            // After having created all the properties, add the meta class information at the end
            if (!cache.MetaClassAlreadyAdded
                && configuration.AutomaticMetaClassField
                && !FormMethods.HasMetaClassFieldInForm(form))
            {
                var metaClassField = MofFactory.create(_DatenMeister.TheOne.Forms.__MetaClassElementFieldData);
                metaClassField.set(_DatenMeister._Forms._MetaClassElementFieldData.name, "Metaclass");
                metaClassField.set(_DatenMeister._Forms._MetaClassElementFieldData.title, "Metaclass");
                form.get<IReflectiveSequence>(_DatenMeister._Forms._TableForm.field).add(metaClassField);

                cache.MetaClassAlreadyAdded = true;


                FormMethods.AddToFormCreationProtocol(form,
                    "[FormCreator.AddFieldsToFormByMetaclass]: Added metaclass information");
            }

            // Sorts the field by important properties
            SortFieldsByImportantProperties(form);

#if DEBUG
            if (!FormMethods.ValidateForm(form))
                throw new InvalidOperationException("Something went wrong during creation of form");
#endif

            return wasInMetaClass;
        }

        /// <summary>
        ///     Takes the given uml item and includes it into the form.
        ///     The element can be of type enumeration, class or property.
        ///     For the creation rules, see chapter "FormManager" in the Documentation
        /// </summary>
        /// <param name="form">Form that will be enriched</param>
        /// <param name="umlClassOrProperty">The uml element, property, class or type that will be added</param>
        /// <param name="creationMode">The creation mode</param>
        /// <param name="umlElementType"></param>
        /// <returns>true, if an element was created</returns>
        public bool AddFieldsToFormByMetaClassProperty(
            IElement form,
            IElement umlClassOrProperty,
            FormFactoryConfiguration creationMode,
            FormUmlElementType umlElementType = FormUmlElementType.Unknown)
        {
            if (form == null) throw new ArgumentNullException(nameof(form));
            if (umlClassOrProperty == null) throw new ArgumentNullException(nameof(umlClassOrProperty));

            var noDuplicate = true;

            // First, select the type of the form
            var isDetailForm =
                ClassifierMethods.IsSpecializedClassifierOf(form.getMetaClass(),
                    _DatenMeister.TheOne.Forms.__RowForm);
            var isListForm =
                ClassifierMethods.IsSpecializedClassifierOf(form.getMetaClass(),
                    _DatenMeister.TheOne.Forms.__TableForm);
            var isCollectionForm =
                ClassifierMethods.IsSpecializedClassifierOf(form.getMetaClass(),
                    _DatenMeister.TheOne.Forms.__CollectionForm);
            var isTableForm =
                ClassifierMethods.IsSpecializedClassifierOf(form.getMetaClass(),
                    _DatenMeister.TheOne.Forms.__TableForm);
            var isNoneOfTheForms = !(isDetailForm || isListForm || isCollectionForm || isTableForm);
            if (isNoneOfTheForms)
                throw new InvalidOperationException("Given element is not a detail, a list, a collection or table form");

            // Second, select the type of the umlElement
            var isPropertyUml =
                umlElementType == FormUmlElementType.Unknown &&
                ClassifierMethods.IsSpecializedClassifierOf(umlClassOrProperty.getMetaClass(),
                    _UML.TheOne.Classification.__Property)
                || umlElementType == FormUmlElementType.Property;
            var isClassUml =
                umlElementType == FormUmlElementType.Unknown &&
                ClassifierMethods.IsSpecializedClassifierOf(umlClassOrProperty.getMetaClass(),
                    _UML.TheOne.StructuredClassifiers.__Class)
                || umlElementType == FormUmlElementType.Class;
            var isEnumerationUml =
                umlElementType == FormUmlElementType.Unknown &&
                ClassifierMethods.IsSpecializedClassifierOf(umlClassOrProperty.getMetaClass(),
                    _UML.TheOne.SimpleClassifiers.__Enumeration)
                || umlElementType == FormUmlElementType.Enumeration;
            var isNoneOfTheUml = !(isPropertyUml || isClassUml || isEnumerationUml);
            if (isNoneOfTheUml)
                throw new InvalidOperationException(
                    "Given element is not a property, not a class, not an enumeration.");

            // First, let's parse the properties
            if (isDetailForm && isPropertyUml || isListForm && isPropertyUml)
            {
                if (noDuplicate && FormMethods.GetField(form, NamedElementMethods.GetName(umlClassOrProperty)) != null)
                    // Field is already existing
                    return false;

                var column = CreateFieldForProperty(
                    umlClassOrProperty.container(),
                    umlClassOrProperty,
                    null,
                    creationMode);
                form.get<IReflectiveCollection>(_DatenMeister._Forms._RowForm.field).add(column);

                FormMethods.AddToFormCreationProtocol(form,
                    "[FormCreator.AddFieldsToFormByMetaClassProperty]: Added Property: " +
                    NamedElementMethods.GetName(column));
                return true;
            }

            if (isCollectionForm && isPropertyUml)
            {
                var isPropertyACollection = PropertyMethods.IsCollection(umlClassOrProperty);

                if (!isPropertyACollection)
                {
                    // Property is a single element, so a field is added to the detail form, if not already
                    // existing
                    var detailForm = GetOrCreateDetailFormIntoExtentForm(form);
                    var result = AddFieldsToFormByMetaClassProperty(detailForm, umlClassOrProperty, creationMode);

                    FormMethods.AddToFormCreationProtocol(form,
                        "[FormCreator.AddFieldsToFormByMetaClassProperty]: Added Property to Detail: " +
                        NamedElementMethods.GetName(umlClassOrProperty));

                    return result;
                }

                var propertyName = umlClassOrProperty.getOrDefault<string>(_UML._CommonStructure._NamedElement.name);
                if (noDuplicate && FormMethods.GetTableFormForPropertyName(form, propertyName) != null)
                    // List form is already existing
                    return false;

                // Property is a collection, so a list form is created for the property
                var tabs = form.get<IReflectiveCollection>(_DatenMeister._Forms._CollectionForm.tab);

                // Now try to figure out the metaclass
                var listForm = CreateListFormForProperty(
                    umlClassOrProperty,
                    FormFactoryConfiguration.CreateByMetaClassOnly);

                FormMethods.AddToFormCreationProtocol(form,
                    "[FormCreator.AddFieldsToFormByMetaClassProperty]: Added Property to List: " +
                    NamedElementMethods.GetName(umlClassOrProperty));

                tabs.add(listForm);
                return true;
            }

            // Now, let's parse the enumerations
            if (isDetailForm && isEnumerationUml || isListForm && isEnumerationUml)
            {
                var propertyName = NamedElementMethods.GetName(umlClassOrProperty)
                    .ToLower(CultureInfo.InvariantCulture);
                var column = CreateFieldForEnumeration(propertyName, umlClassOrProperty, creationMode);
                form.get<IReflectiveCollection>(_DatenMeister._Forms._RowForm.field).add(column);

                FormMethods.AddToFormCreationProtocol(form,
                    "[FormCreator.AddFieldsToFormByMetaClassProperty]: Added Enumeration: " +
                    NamedElementMethods.GetName(column));

                return true;
            }

            if (isCollectionForm && isEnumerationUml)
            {
                var detailForm = GetOrCreateDetailFormIntoExtentForm(form);
                var result = AddFieldsToFormByMetaClassProperty(detailForm, umlClassOrProperty, creationMode);

                FormMethods.AddToFormCreationProtocol(form,
                    "[FormCreator.AddFieldsToFormByMetaClassProperty]: Added Enumeration to Detail: " +
                    NamedElementMethods.GetName(umlClassOrProperty));

                return result;
            }

            // Now the classes... All properties are created into this. 
            if (isClassUml)
            {
                var properties = ClassifierMethods.GetPropertiesOfClassifier(umlClassOrProperty);
                var added = true;
                foreach (var property in properties)
                {
                    added &= AddFieldsToFormByMetaClassProperty(form, property, creationMode);

                    FormMethods.AddToFormCreationProtocol(form,
                        "[FormCreator.AddFieldsToFormByMetaClassProperty]: Added Enumeration to Detail: " +
                        NamedElementMethods.GetName(property));
                }

                return added;
            }

            return false;
        }

        /// <summary>
        ///     Gets the field data, depending upon the given property
        /// </summary>
        /// <param name="parentMetaClass">Meta class of the parent item</param>
        /// <param name="property">Uml-Property which is requesting a field</param>
        /// <param name="propertyName">Name of the property wo whose values the list form shall be created.</param>
        /// <param name="configuration">Defines the mode how to create the fields</param>
        /// <returns>The field data</returns>
        private IElement CreateFieldForProperty(
            IObject? parentMetaClass,
            IObject? property,
            string? propertyName,
            FormFactoryConfiguration configuration)
        {
            if (property == null && propertyName == null)
                throw new InvalidOperationException("property == null && propertyName == null");

            var propertyType = property == null ? null : PropertyMethods.GetPropertyType(property);

            propertyName ??= property.get<string>("name");
            var propertyIsEnumeration = property != null && PropertyMethods.IsCollection(property);
            var isReadOnly = configuration.IsReadOnly;

            // Check, if field property is an enumeration
            _uriResolver ??= _workspaceLogic?.GetTypesWorkspace();

            _stringType ??= _PrimitiveTypes.TheOne.__String;
            _integerType ??= _PrimitiveTypes.TheOne.__Integer;
            _booleanType ??= _PrimitiveTypes.TheOne.__Boolean;
            _realType ??= _PrimitiveTypes.TheOne.__Real;
            _dateTimeType ??= _uriResolver?.ResolveElement(CoreTypeNames.DateTimeType, ResolveType.Default, false);

            // Checks, if the property is an enumeration.
            var propertyTypeMetaClass = propertyType?.metaclass; // The type of the type (enum, class, struct, etc)
            if (propertyTypeMetaClass != null && propertyType != null)
            {
                if (propertyTypeMetaClass.equals(_UML.TheOne.SimpleClassifiers.__Enumeration))
                    return CreateFieldForEnumeration(propertyName, propertyType, configuration);

                if (propertyType.equals(_booleanType))
                {
                    // If we have a boolean and the field is not for a list form
                    var checkbox = MofFactory.create(_DatenMeister.TheOne.Forms.__CheckboxFieldData);
                    checkbox.set(_DatenMeister._Forms._CheckboxFieldData.name, propertyName);
                    checkbox.set(_DatenMeister._Forms._CheckboxFieldData.title, propertyName);
                    checkbox.set(_DatenMeister._Forms._CheckboxFieldData.isReadOnly, isReadOnly);
                    return checkbox;
                }

                if (propertyType.equals(_dateTimeType))
                {
                    var dateTimeField = MofFactory.create(_DatenMeister.TheOne.Forms.__DateTimeFieldData);
                    dateTimeField.set(_DatenMeister._Forms._CheckboxFieldData.name, propertyName);
                    dateTimeField.set(_DatenMeister._Forms._CheckboxFieldData.title, propertyName);
                    dateTimeField.set(_DatenMeister._Forms._CheckboxFieldData.isReadOnly, isReadOnly);
                    return dateTimeField;
                }

                if (
                    !propertyType.equals(_stringType) &&
                    !propertyType.equals(_integerType) &&
                    !propertyType.equals(_realType) &&
                    !propertyType.equals(_dateTimeType))
                {
                    // If we have something else than a primitive type and it is not for a list form
                    if (propertyIsEnumeration)
                    {
                        // It can contain multiple elements
                        var elementsField = MofFactory.create(_DatenMeister.TheOne.Forms.__SubElementFieldData);
                        elementsField.set(_DatenMeister._Forms._SubElementFieldData.name, propertyName);
                        elementsField.set(_DatenMeister._Forms._SubElementFieldData.title, propertyName);
                        elementsField.set(_DatenMeister._Forms._SubElementFieldData.isReadOnly,
                            configuration.IsForListView || configuration.IsReadOnly);

                        if (!configuration.IsForListView)
                        {
                            FormMethods.AddDefaultTypeForNewElement(elementsField, propertyType);
                        }

                        elementsField.set(
                            _DatenMeister._Forms._SubElementFieldData.includeSpecializationsForDefaultTypes, true);
                        elementsField.set(_DatenMeister._Forms._SubElementFieldData.isReadOnly, isReadOnly);

                        IElement? enumerationListForm = null;
                        if (!configuration.IsForListView)
                        {
                            if (_formLogic != null)
                                enumerationListForm =
                                    new FormFactory(_workspaceLogic!, _scopeStorage)
                                        .CreateListFormForMetaClass(propertyType, configuration);

                            // Create the internal form out of the metaclass
                            if (enumerationListForm == null
                                && configuration.CreateByMetaClass)
                                enumerationListForm =
                                    CreateListFormForMetaClass(
                                        propertyType,
                                        configuration with { IsForListView = true },
                                        property as IElement);

                            if (enumerationListForm != null)
                                elementsField.set(_DatenMeister._Forms._SubElementFieldData.form, enumerationListForm);
                        }

                        return elementsField;
                    }

                    // It can just contain one element (or is in list view)
                    var reference = MofFactory.create(_DatenMeister.TheOne.Forms.__ReferenceFieldData);
                    reference.set(_DatenMeister._Forms._ReferenceFieldData.name, propertyName);
                    reference.set(_DatenMeister._Forms._ReferenceFieldData.title, propertyName);
                    reference.set(_DatenMeister._Forms._ReferenceFieldData.isReadOnly, isReadOnly);
                    reference.set(_DatenMeister._Forms._ReferenceFieldData.isEnumeration, propertyIsEnumeration);
                    reference.set(
                        _DatenMeister._Forms._ReferenceFieldData.metaClassFilter,
                        new[] { propertyType });

                    return reference;
                }
            }

            if (propertyType == null)
            {
                // If we have something else than a primitive type and it is not for a list form
                var element = MofFactory.create(propertyIsEnumeration
                    ? _DatenMeister.TheOne.Forms.__SubElementFieldData
                    : _DatenMeister.TheOne.Forms.__AnyDataFieldData);

                // It can just contain one element
                element.set(_DatenMeister._Forms._SubElementFieldData.name, propertyName);
                element.set(_DatenMeister._Forms._SubElementFieldData.title, propertyName);
                element.set(_DatenMeister._Forms._SubElementFieldData.isReadOnly, isReadOnly);
                element.set(_DatenMeister._Forms._SubElementFieldData.isEnumeration, propertyIsEnumeration);

                if (propertyType != null)
                {
                    FormMethods.AddDefaultTypeForNewElement(element, propertyType);
                }

                return element;
            }

            // Per default, assume some kind of text
            var column = MofFactory.create(_DatenMeister.TheOne.Forms.__TextFieldData);
            column.set(_DatenMeister._Forms._TextFieldData.name, propertyName);
            column.set(_DatenMeister._Forms._TextFieldData.title, propertyName);
            column.set(_DatenMeister._Forms._TextFieldData.isReadOnly, isReadOnly);

            // If propertyType is an integer, the field can be smaller
            if (propertyType.equals(_integerType)) column.set(_DatenMeister._Forms._TextFieldData.width, 10);

            return column;
        }

        /// <summary>
        ///     Creates a field for the given enumeration
        /// </summary>
        /// <param name="propertyName">Name of the property being used to add title and name</param>
        /// <param name="propertyType">Type of the enumeration</param>
        /// <param name="creationMode">The used creation mode</param>
        /// <returns>The created element of the enumeration</returns>
        private IElement CreateFieldForEnumeration(string propertyName, IElement propertyType,
            FormFactoryConfiguration creationMode)
        {
            var isReadOnly = creationMode.IsReadOnly;
            // If we have an enumeration (C#: Enum) and the field is not for a list form
            var comboBox = MofFactory.create(_DatenMeister.TheOne.Forms.__DropDownFieldData);
            comboBox.set(_DatenMeister._Forms._DropDownFieldData.name, propertyName);
            comboBox.set(_DatenMeister._Forms._DropDownFieldData.title, propertyName);
            comboBox.set(_DatenMeister._Forms._DropDownFieldData.isReadOnly, isReadOnly);

            var values = EnumerationMethods.GetEnumValues(propertyType);
            comboBox.set(
                _DatenMeister._Forms._DropDownFieldData.values,
                values.Select(x =>
                {
                    var data = MofFactory.create(_DatenMeister.TheOne.Forms.__ValuePair);
                    data.set(_DatenMeister._Forms._ValuePair.name, x);
                    data.set(_DatenMeister._Forms._ValuePair.value, x);
                    return data;
                }).ToList());
            return comboBox;
        }
        
        private static void SortFieldsByImportantProperties(IObject form)
        {
            var fields = form.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._TableForm.field);
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
            form.set(_DatenMeister._Forms._TableForm.field, fieldsAsList);

            FormMethods.AddToFormCreationProtocol(
                form,
                "[FormCreator.SortFieldsByImportantProperties]: Fields are sorted");
        }

        /// <summary>
        ///     Checks whether at least one field is given.
        ///     If no field is given, then the one text field for the name will be added
        /// </summary>
        /// <param name="form">Form to be checked</param>
        private static void AddTextFieldForNameIfNoFieldAvailable(IObject form)
        {
            // If the field is empty, create an empty textfield with 'name' as a placeholder
            var fieldLength =
                form.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._TableForm.field)?.Count() ?? 0;
            if (fieldLength == 0)
            {
                var factory = new MofFactory(form);
                var textFieldData = factory.create(_DatenMeister.TheOne.Forms.__TextFieldData);
                textFieldData.set(_DatenMeister._Forms._TextFieldData.name, "name");
                textFieldData.set(_DatenMeister._Forms._TextFieldData.title, "name");

                form.AddCollectionItem(_DatenMeister._Forms._TableForm.field, textFieldData);

                FormMethods.AddToFormCreationProtocol(
                    form,
                    "[FormCreator.AddTextFieldForNameIfNoFieldAvailable]: Added default 'name' because it is empty");
            }
        }

        /// <summary>
        ///     Defines a class being used as an internal cache for the form creation.
        ///     This improves the speed of form creation since some state are explicitly stored in the
        ///     cache instead of being required to be evaluated out of the field structure.
        /// </summary>
        private class FormCreatorCache
        {
            /// <summary>
            ///     True, if the metaclass has been already covered
            /// </summary>
            public bool MetaClassAlreadyAdded { get; set; }

            /// <summary>
            ///     The meta classes that have been covered
            /// </summary>
            public HashSet<IElement> CoveredMetaClasses { get; } = new();

            /// <summary>
            ///     The property names that already have been covered and are within the
            /// </summary>
            public HashSet<string> CoveredPropertyNames { get; } = new();

            /// <summary>
            ///     When at least one value is set, only those properties will be added which
            ///     are added to this hashset.
            /// </summary>
            public HashSet<string> FocusOnPropertyNames { get; } = new();
        }
    }
}