#nullable enable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models;
using DatenMeister.Models.EMOF;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Extents.Configuration;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using _PrimitiveTypes = DatenMeister.Models.EMOF._PrimitiveTypes;
using Workspace = DatenMeister.Runtime.Workspaces.Workspace;

namespace DatenMeister.Modules.Forms.FormCreator
{
    /// <summary>
    /// Creates a view out of the given extent, elements (collection) or element).
    /// 
    /// </summary>
    public partial class FormCreator
    {
        /// <summary>
        /// Stores the reference to the view logic which is required to get the views
        /// for the tabs of the extent form
        /// </summary>
        private readonly FormsPlugin? _formLogic;

        /// <summary>
        /// Stores the associated workspace logic
        /// </summary>
        private readonly IWorkspaceLogic? _workspaceLogic;

        /// <summary>
        /// Stores the factory to create the fields and forms
        /// </summary>
        private readonly IFactory _factory;

        private readonly ExtentSettings _extentSettings;

        private IElement? _stringType;
        private IElement? _integerType;
        private IElement? _booleanType;
        private IElement? _realType;
        private IElement? _dateTimeType;
        private Workspace? _uriResolver;

        /// <summary>
        /// Initializes a new instance of the FormCreator class
        /// </summary>
        /// <param name="workspaceLogic">The workspace logic to be used</param>
        /// <param name="formLogic">View logic being used</param>
        /// <param name="scopeStorage">Stores the extent settings</param>
        public FormCreator(
            IWorkspaceLogic workspaceLogic,
            FormsPlugin? formLogic,
            IScopeStorage scopeStorage)
             : this (workspaceLogic, formLogic, scopeStorage.TryGet<ExtentSettings>())
        {

        }
        
        /// <summary>
        /// Initializes a new instance of the FormCreator class
        /// </summary>
        /// <param name="workspaceLogic">The workspace logic to be used</param>
        /// <param name="formLogic">View logic being used</param>
        /// <param name="extentSettings">Stores the extent settings</param>
        private FormCreator(
            IWorkspaceLogic? workspaceLogic,
            FormsPlugin? formLogic, 
            ExtentSettings? extentSettings = null)
        {
            _formLogic = formLogic;
            _extentSettings = extentSettings ?? new ExtentSettings();
            _workspaceLogic = workspaceLogic;

            var userExtent = _formLogic?.GetUserFormExtent();
            _factory = userExtent != null
                ? new MofFactory(userExtent)
                : InMemoryObject.TemporaryFactory;
        }
        
        /// <summary>
        /// Creates the form logic by using the private constructor
        /// </summary>
        /// <param name="workspaceLogic">Workspace Logic to be evaluated</param>
        /// <param name="formLogic">Form Logic to be evaluated</param>
        /// <param name="extentSettings">Settings of the extent</param>
        /// <returns></returns>
        public static FormCreator Create(IWorkspaceLogic? workspaceLogic,
            FormsPlugin? formLogic, 
            ExtentSettings? extentSettings = null)
        {
            return new FormCreator(workspaceLogic, formLogic, extentSettings);
        }

        /// <summary>
        /// Creates the fields of the form by evaluation of the given object.
        /// Depending on the creation mode, the evaluation will be done by metaclass
        /// or by evaluation of the properties.
        ///
        /// This method is independent whether it is used in an list or extent form. 
        /// </summary>
        /// <param name="form">Form which will be extended by the given object</param>
        /// <param name="item">Item being used</param>
        /// <param name="creationMode">Creation mode for the form. Whether by metaclass or ByProperties</param>
        /// <param name="cache">Cache being used to store intermediate items</param>
        private void AddToForm(IObject form, object item, CreationMode creationMode, FormCreatorCache cache)
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
                    wasInMetaClass = AddToFormByMetaclass(
                        form,
                        metaClass,
                        creationMode & ~(CreationMode.AddMetaClass),
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

            var isByProperties =
                creationMode.HasFlag(CreationMode.ByPropertyValues);
            var isOnlyPropertiesIfNoMetaClass =
                creationMode.HasFlag(CreationMode.OnlyPropertiesIfNoMetaClass);

            if ((isByProperties
                 || (isOnlyPropertiesIfNoMetaClass && !wasInMetaClass))
                && itemAsAllProperties != null)
            {
                AddToFormByPropertyValues(form, item, creationMode, cache);
                form.set(_DatenMeister._Forms._DetailForm.allowNewProperties, true);
            }

            // Third phase: Add metaclass element itself
            var isMetaClass = creationMode.HasFlag(CreationMode.AddMetaClass);
            if (!cache.MetaClassAlreadyAdded
                && isMetaClass
                && !FormMethods.HasMetaClassFieldInForm(form))
            {
                // Add the element itself
                var metaClassField = _factory.create(_DatenMeister.TheOne.Forms.__MetaClassElementFieldData);
                metaClassField.set(_DatenMeister._Forms._MetaClassElementFieldData.name, "Metaclass");

                form.get<IReflectiveCollection>(_DatenMeister._Forms._DetailForm.field).add(metaClassField);

                // Sets the information in cache, that the element was already added
                cache.MetaClassAlreadyAdded = true;
            }

#if DEBUG
            if (!FormMethods.ValidateForm(form))
                throw new InvalidOperationException("Something went wrong during creation of form");
#endif
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
        private void AddToFormByPropertyValues(IObject form, object item, CreationMode creationMode, FormCreatorCache cache)
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
            
            var isReadOnly = creationMode.HasFlagFast(CreationMode.ReadOnly);

            // Creates the form out of the properties of the item
            var properties = itemAsAllProperties.getPropertiesBeingSet();
            
            var focusOnPropertyNames = cache.FocusOnPropertyNames.Any();

            foreach (var propertyName in properties
                .Where(property => !cache.CoveredPropertyNames.Contains(property)))
            {
                cache.CoveredPropertyNames.Add(propertyName);
                if (focusOnPropertyNames && !cache.FocusOnPropertyNames.Contains(propertyName))
                {
                    // Skip the property name, when we would like to have focus on certain property names
                    continue;
                }

                // Checks, whether the field is already existing
                var column = form
                    .get<IReflectiveCollection>(_DatenMeister._Forms._DetailForm.field)
                    .OfType<IObject>()
                    .FirstOrDefault(x => x.getOrDefault<string>(_DatenMeister._Forms._FieldData.name) == propertyName);

                var propertyValue = itemAsObject.getOrDefault<object>(propertyName);

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
                        column = _factory.create(_DatenMeister.TheOne.Forms.__TextFieldData);
                    }
                    else
                    {
                        if (DotNetHelper.IsEnumeration(propertyType))
                        {
                            column = _factory.create(_DatenMeister.TheOne.Forms.__SubElementFieldData);
                        }
                        else
                        {
                            column = _factory.create(_DatenMeister.TheOne.Forms.__ReferenceFieldData);
                            column.set(_DatenMeister._Forms._ReferenceFieldData.isSelectionInline, false);
                        }
                    }

                    column.set(_DatenMeister._Forms._FieldData.name, propertyName);
                    column.set(_DatenMeister._Forms._FieldData.title, propertyName);
                    column.set(_DatenMeister._Forms._FieldData.isReadOnly, isReadOnly);

                    form.get<IReflectiveCollection>(_DatenMeister._Forms._DetailForm.field).add(column);
                }

                // Makes the field to an enumeration, if explicitly requested or the type behind is an enumeration
                column.set(
                    _DatenMeister._Forms._FieldData.isEnumeration,
                    column.getOrDefault<bool>(_DatenMeister._Forms._FieldData.isEnumeration) | DotNetHelper.IsEnumeration(propertyValue?.GetType()));
            }
            
#if DEBUG
            if (!FormMethods.ValidateForm(form))
                throw new InvalidOperationException("Something went wrong during creation of form");
#endif
        }

        /// <summary>
        /// Adds the fields for the form by going through the properties of the metaclass.
        /// It only adds fields, when they are not already added to the given list or detail form
        /// </summary>
        /// <param name="form">Form that will be extended. Must be list or detail form.</param>
        /// <param name="metaClass">Metaclass to be used</param>
        /// <param name="creationMode">Creation Mode to be used</param>
        /// <param name="cache">Cache of reportCreator cache</param>
        /// <returns>true, if the metaclass is not null and if the metaclass contains at least on</returns>
        private bool AddToFormByMetaclass(IObject form, IObject metaClass, CreationMode creationMode, FormCreatorCache? cache = null)
        {
            cache ??= new FormCreatorCache();
            
            var wasInMetaClass = false;
            if (metaClass == null)
            {
                return false;
            }

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
                {
                    // Skip the property name, when we would like to have focus on certain property names
                    continue;
                }

                var isAlreadyIn = form
                    .get<IReflectiveCollection>(_DatenMeister._Forms._DetailForm.field)
                    .OfType<IObject>()
                    .Any(x => x.getOrDefault<string>(_DatenMeister._Forms._FieldData.name) == propertyName);

                if (isAlreadyIn)
                {
                    continue;
                }

                var column = GetFieldForProperty(metaClass, property, creationMode);
                form.get<IReflectiveCollection>(_DatenMeister._Forms._DetailForm.field).add(column);
            }

            // After having created all the properties, add the meta class information at the end
            if (!cache.MetaClassAlreadyAdded
                && creationMode.HasFlagFast(CreationMode.AddMetaClass)
                && !FormMethods.HasMetaClassFieldInForm(form))
            {
                var metaClassField = _factory.create(_DatenMeister.TheOne.Forms.__MetaClassElementFieldData);
                metaClassField.set(_DatenMeister._Forms._MetaClassElementFieldData.name, "Metaclass");
                metaClassField.set(_DatenMeister._Forms._MetaClassElementFieldData.title, "Metaclass");
                form.get<IReflectiveSequence>(_DatenMeister._Forms._ListForm.field).add(metaClassField);

                cache.MetaClassAlreadyAdded = true;
            }

#if DEBUG
            if (!FormMethods.ValidateForm(form))
                throw new InvalidOperationException("Something went wrong during creation of form");
#endif
            
            return wasInMetaClass;
        }

        /// <summary>
        /// Takes the given uml item and includes it into the form.
        /// The element can be of type enumeration, class or property.
        ///
        /// For the creation rules, see chapter "FormManager" in the Documentation
        /// </summary>
        /// <param name="form">Form that will be enriched</param>
        /// <param name="umlElement">The uml element, property, class or type that will be added</param>
        /// <param name="creationMode">The creation mode</param>
        /// <returns>true, if an element was created</returns>
        public bool AddToFormByUmlElement(IElement form, IElement umlElement, CreationMode creationMode)
        {
            if (form == null) throw new ArgumentNullException(nameof(form));
            if (umlElement == null) throw new ArgumentNullException(nameof(umlElement));

            var noDuplicate = creationMode.HasFlagFast(CreationMode.NoDuplicate);
            
            // First, select the type of the form
            var isDetailForm = 
                ClassifierMethods.IsSpecializedClassifierOf(form.getMetaClass(), _DatenMeister.TheOne.Forms.__DetailForm);
            var isListForm = 
                ClassifierMethods.IsSpecializedClassifierOf(form.getMetaClass(), _DatenMeister.TheOne.Forms.__ListForm);
            var isExtentForm = 
                ClassifierMethods.IsSpecializedClassifierOf(form.getMetaClass(), _DatenMeister.TheOne.Forms.__ExtentForm);
            var isNoneOfTheForms = !(isDetailForm || isListForm || isExtentForm);
            if (isNoneOfTheForms)
            {
                throw new InvalidOperationException("Given element is not a detail, a list or an extent form");
            }

            // Second, select the type of the umlElement
            var isPropertyUml =
                ClassifierMethods.IsSpecializedClassifierOf(umlElement.getMetaClass(), _UML.TheOne.Classification.__Property);
            var isClassUml =
                ClassifierMethods.IsSpecializedClassifierOf(umlElement.getMetaClass(), _UML.TheOne.StructuredClassifiers.__Class);
            var isEnumerationUml =
                ClassifierMethods.IsSpecializedClassifierOf(umlElement.getMetaClass(), _UML.TheOne.SimpleClassifiers.__Enumeration);
            var isNoneOfTheUml = !(isPropertyUml || isClassUml || isEnumerationUml);
            if (isNoneOfTheUml)
            {
                throw new InvalidOperationException(
                    "Given element is not a property, not a class, not an enumeration.");
            }

            // Now create the element depending on the form type
            if (isListForm)
            {
                // Adds the flag for list forms
                creationMode |= CreationMode.ForListForms;
            }
            
            // First, let's parse the properties
            if (isDetailForm && isPropertyUml || isListForm && isPropertyUml)
            {
                if (noDuplicate && FormMethods.GetField(form, NamedElementMethods.GetName(umlElement)) != null)
                {
                    // Field is already existing
                    return false;
                }

                var column = GetFieldForProperty(umlElement.container(), umlElement, creationMode);
                form.get<IReflectiveCollection>(_DatenMeister._Forms._DetailForm.field).add(column);
                return true;
            }

            if (isExtentForm && isPropertyUml)
            {
                var isPropertyACollection = PropertyMethods.IsCollection(umlElement);

                if (!isPropertyACollection)
                {
                    // Property is a single element, so a field is added to the detail form, if not already
                    // existing
                    var detailForm = GetOrCreateDetailFormIntoExtentForm(form);
                    return AddToFormByUmlElement(detailForm, umlElement, creationMode);
                }
                else
                {
                    var propertyName = umlElement.getOrDefault<string>(_UML._CommonStructure._NamedElement.name);
                    if (noDuplicate && FormMethods.GetListTabForPropertyName(form, propertyName) != null )
                    {
                        // List form is already existing
                        return false;
                    }
                    
                    // Property is a collection, so a list form is created for the property
                    var tabs = form.get<IReflectiveCollection>(_DatenMeister._Forms._ExtentForm.tab);
                    
                    // Now try to figure out the metaclass
                    var listForm = CreateListFormForProperty(umlElement, CreationMode.ByMetaClass);
                    tabs.add(listForm);
                    return true;
                }
            }
            
            // Now, let's parse the enumerations
            if (isDetailForm && isEnumerationUml || isListForm && isEnumerationUml)
            {
                var propertyName = NamedElementMethods.GetName(umlElement).ToLower(CultureInfo.InvariantCulture);
                var column = CreateFieldForEnumeration(propertyName, umlElement, creationMode);
                form.get<IReflectiveCollection>(_DatenMeister._Forms._DetailForm.field).add(column);
                return true;
            }

            if (isExtentForm && isEnumerationUml)
            {
                var detailForm = GetOrCreateDetailFormIntoExtentForm(form);
                return AddToFormByUmlElement(detailForm, umlElement, creationMode);
            }
            
            // Now the classes... All properties are created into this. 
            if (isClassUml)
            {
                var properties = ClassifierMethods.GetPropertiesOfClassifier(umlElement);
                var added = true;
                foreach (var property in properties)
                {
                    added &= AddToFormByUmlElement(form, property, creationMode);
                }

                return added;
            }

            return false;
        }

        /// <summary>
        /// Gets the field data, depending upon the given property
        /// </summary>
        /// <param name="property">Uml-Property which is requesting a field</param>
        /// <param name="creationMode">Defines the mode how to create the fields</param>
        /// <returns>The field data</returns>
        private IElement GetFieldForProperty(IObject? parentMetaClass, IObject property, CreationMode creationMode)
        {
            var propertyType = PropertyMethods.GetPropertyType(property);
            var isForListForm = creationMode.HasFlagFast(CreationMode.ForListForms);
            var propertyName = property.get<string>("name");
            var propertyIsEnumeration = PropertyMethods.IsCollection(property);
            var isReadOnly = creationMode.HasFlagFast(CreationMode.ReadOnly);

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
                if (propertyTypeMetaClass.@equals(_UML.TheOne.SimpleClassifiers.__Enumeration) && !isForListForm)
                {
                    return CreateFieldForEnumeration(propertyName, propertyType, creationMode);
                }

                if (propertyType.@equals(_booleanType) && !isForListForm)
                {
                    // If we have a boolean and the field is not for a list form
                    var checkbox = _factory.create(_DatenMeister.TheOne.Forms.__CheckboxFieldData);
                    checkbox.set(_DatenMeister._Forms._CheckboxFieldData.name, propertyName);
                    checkbox.set(_DatenMeister._Forms._CheckboxFieldData.title, propertyName);
                    checkbox.set(_DatenMeister._Forms._CheckboxFieldData.isReadOnly, isReadOnly);
                    return checkbox;
                }

                if (propertyType.@equals(_dateTimeType) && !isForListForm)
                {
                    var dateTimeField= _factory.create(_DatenMeister.TheOne.Forms.__DateTimeFieldData);
                    dateTimeField.set(_DatenMeister._Forms._CheckboxFieldData.name, propertyName);
                    dateTimeField.set(_DatenMeister._Forms._CheckboxFieldData.title, propertyName);
                    dateTimeField.set(_DatenMeister._Forms._CheckboxFieldData.isReadOnly, isReadOnly);
                    return dateTimeField;
                }

                if (
                    !propertyType.@equals(_stringType) &&
                    !propertyType.@equals(_integerType) &&
                    !propertyType.@equals(_realType) &&
                    !propertyType.@equals(_dateTimeType) &&
                    !isForListForm)
                {
                    // If we have something else than a primitive type and it is not for a list form
                    if (propertyIsEnumeration)
                    {
                        // It can contain multiple elements
                        var elements = _factory.create(_DatenMeister.TheOne.Forms.__SubElementFieldData);
                        elements.set(_DatenMeister._Forms._SubElementFieldData.name, propertyName);
                        elements.set(_DatenMeister._Forms._SubElementFieldData.title, propertyName);

                        var defaultTypeForNewElement =
                            _factory.create(_DatenMeister.TheOne.Forms.__DefaultTypeForNewElement);
                        defaultTypeForNewElement.set(_DatenMeister._Forms._DefaultTypeForNewElement.name,
                            NamedElementMethods.GetName(propertyType));
                        defaultTypeForNewElement.set(_DatenMeister._Forms._DefaultTypeForNewElement.metaClass,
                            propertyType);
                        elements.set(_DatenMeister._Forms._SubElementFieldData.defaultTypesForNewElements,
                            new[] {defaultTypeForNewElement});
                        
                        elements.set(_DatenMeister._Forms._SubElementFieldData.includeSpecializationsForDefaultTypes, true);
                        elements.set(_DatenMeister._Forms._SubElementFieldData.isReadOnly, isReadOnly);

                        IElement? enumerationListForm = null;
                        if (_formLogic != null)
                        {
                            var formFinder = new FormFinder.FormFinder(_formLogic);
                            enumerationListForm = formFinder.FindFormsFor(
                                new FindFormQuery
                                {
                                    parentMetaClass = parentMetaClass,
                                    parentProperty = propertyName,
                                    metaClass = propertyType,
                                    FormType = _DatenMeister._Forms.___FormType.ObjectList
                                }).FirstOrDefault();
                        }
                        // Create the internal form out of the metaclass
                        if (enumerationListForm == null 
                            && creationMode.HasFlag(CreationMode.ByMetaClass))
                        {
                            enumerationListForm =
                                CreateListFormForMetaClass(
                                    propertyType,
                                    CreationMode.All,
                                    property as IElement);
                        }

                        if (enumerationListForm != null)
                        {
                            elements.set(_DatenMeister._Forms._SubElementFieldData.form, enumerationListForm);
                        }

                        return elements;
                    }

                    // It can just contain one element
                    var reference = _factory.create(_DatenMeister.TheOne.Forms.__ReferenceFieldData);
                    reference.set(_DatenMeister._Forms._ReferenceFieldData.name, propertyName);
                    reference.set(_DatenMeister._Forms._ReferenceFieldData.title, propertyName);
                    reference.set(_DatenMeister._Forms._ReferenceFieldData.isReadOnly, isReadOnly);
                    if (propertyType != null)
                    {
                        reference.set(
                            _DatenMeister._Forms._ReferenceFieldData.metaClassFilter,
                            new[] {propertyType});
                    }

                    return reference;
                }
            }

            if (propertyType == null)
            {
                // If we have something else than a primitive type and it is not for a list form
                var element = _factory.create(propertyIsEnumeration
                    ? _DatenMeister.TheOne.Forms.__SubElementFieldData
                    : _DatenMeister.TheOne.Forms.__AnyDataFieldData);

                // It can just contain one element
                element.set(_DatenMeister._Forms._SubElementFieldData.name, propertyName);
                element.set(_DatenMeister._Forms._SubElementFieldData.title, propertyName);
                element.set(_DatenMeister._Forms._SubElementFieldData.isReadOnly, isReadOnly);
                element.set(_DatenMeister._Forms._SubElementFieldData.isEnumeration, propertyIsEnumeration);
                
                if (propertyType != null)
                {
                    var defaultTypeForNewElement =
                        _factory.create(_DatenMeister.TheOne.Forms.__DefaultTypeForNewElement);
                    defaultTypeForNewElement.set(_DatenMeister._Forms._DefaultTypeForNewElement.name,
                        NamedElementMethods.GetName(propertyType));
                    defaultTypeForNewElement.set(_DatenMeister._Forms._DefaultTypeForNewElement.metaClass,
                        propertyType);
                    element.set(_DatenMeister._Forms._SubElementFieldData.defaultTypesForNewElements,
                        new[] {defaultTypeForNewElement});
                }
                
                return element;
            }

            // Per default, assume some kind of text
            var column = _factory.create(_DatenMeister.TheOne.Forms.__TextFieldData);
            column.set(_DatenMeister._Forms._TextFieldData.name, propertyName);
            column.set(_DatenMeister._Forms._TextFieldData.title, propertyName);
            column.set(_DatenMeister._Forms._TextFieldData.isReadOnly, isReadOnly);

            // If propertyType is an integer, the field can be smaller
            if (propertyType.@equals(_integerType))
            {
                column.set(_DatenMeister._Forms._TextFieldData.width, 10);
            }

            return column;
        }

        /// <summary>
        /// Creates a field for the given enumeration
        /// </summary>
        /// <param name="propertyName">Name of the property being used to add title and name</param>
        /// <param name="propertyType">Type of the enumeration</param>
        /// <param name="creationMode">The used creation mode</param>
        /// <returns>The created element of the enumeration</returns>
        private IElement CreateFieldForEnumeration(string propertyName, IElement propertyType, CreationMode creationMode)
        {
            var isReadOnly = creationMode.HasFlagFast(CreationMode.ReadOnly);
            // If we have an enumeration (C#: Enum) and the field is not for a list form
            var comboBox = _factory.create(_DatenMeister.TheOne.Forms.__DropDownFieldData);
            comboBox.set(_DatenMeister._Forms._DropDownFieldData.name, propertyName);
            comboBox.set(_DatenMeister._Forms._DropDownFieldData.title, propertyName);
            comboBox.set(_DatenMeister._Forms._DropDownFieldData.isReadOnly, isReadOnly);

            var values = EnumerationMethods.GetEnumValues(propertyType);
            comboBox.set(
                _DatenMeister._Forms._DropDownFieldData.values,
                values.Select(x =>
                {
                    var data = _factory.create(_DatenMeister.TheOne.Forms.__ValuePair);
                    data.set(_DatenMeister._Forms._ValuePair.name, x);
                    data.set(_DatenMeister._Forms._ValuePair.value, x);
                    return data;
                }).ToList());
            return comboBox;
        }

        /// <summary>
        /// Defines a class being used as an internal cache for the form creation.
        /// This improves the speed of form creation since some state are explicitly stored in the
        /// cache instead of being required to be evaluated out of the field structure. 
        /// </summary>
        private class FormCreatorCache
        {
            /// <summary>
            /// True, if the metaclass has been already covered
            /// </summary>
            public bool MetaClassAlreadyAdded { get; set; }

            /// <summary>
            /// The meta classes that have been covered
            /// </summary>
            public HashSet<IElement> CoveredMetaClasses { get; } = new HashSet<IElement>();

            /// <summary>
            /// The property names that already have been covered and are within the 
            /// </summary>
            public HashSet<string> CoveredPropertyNames { get; } = new HashSet<string>();
            
            /// <summary>
            /// Skips the given property names
            /// </summary>
            public HashSet<string> FocusOnPropertyNames { get; } = new HashSet<string>();
        }
    }
}