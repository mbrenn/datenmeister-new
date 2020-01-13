#nullable enable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.DefaultTypes;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;

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
        private readonly FormLogic _formLogic;

        private readonly DefaultClassifierHints _defaultClassifierHints;

        /// <summary>
        /// Stores the associated workspace logic
        /// </summary>
        private readonly IWorkspaceLogic? _workspaceLogic;

        /// <summary>
        /// Stores the factory to create the fields and forms
        /// </summary>
        private readonly IFactory _factory;

        /// <summary>
        /// Stores the information for form and fields including all metaclasses
        /// </summary>
        private readonly _FormAndFields _formAndFields;

        private IElement? _stringType;
        private IElement? _integerType;
        private IElement? _booleanType;
        private IElement? _realType;
        private IElement? _dateTimeType;
        private _UML? _uml;
        private _PrimitiveTypes? _primitiveTypes;
        private Workspace? _uriResolver;

        /// <summary>
        /// Initializes a new instance of the FormCreator class
        /// </summary>
        /// <param name="formLogic">View logic being used</param>
        /// <param name="defaultClassifierHints">The classifier hints</param>
        public FormCreator(FormLogic formLogic, DefaultClassifierHints defaultClassifierHints)
        {
            _formLogic = formLogic;
            _defaultClassifierHints = defaultClassifierHints;

            _workspaceLogic = _formLogic?.WorkspaceLogic;
            var userExtent = _formLogic?.GetUserFormExtent();
            _factory = userExtent != null
                ? new MofFactory(userExtent)
                : InMemoryObject.TemporaryFactory;
            _formAndFields = userExtent?.GetWorkspace()?.GetFromMetaWorkspace<_FormAndFields>()
                             ?? _FormAndFields.TheOne;
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
                creationMode.HasFlag(CreationMode.ByPropertyValues);
            var isOnlyPropertiesIfNoMetaClass =
                creationMode.HasFlag(CreationMode.OnlyPropertiesIfNoMetaClass);

            if ((isByProperties
                 || (isOnlyPropertiesIfNoMetaClass && !wasInMetaClass))
                && itemAsAllProperties != null)
            {
                AddToFormByPropertyValues(form, item, creationMode, cache);
                form.set(_FormAndFields._DetailForm.allowNewProperties, true);
            }

            // Third phase: Add metaclass element itself
            var isMetaClass = creationMode.HasFlag(CreationMode.AddMetaClass);
            if (!cache.MetaClassAlreadyAdded &&
                isMetaClass &&
                !form
                    .get<IReflectiveCollection>(_FormAndFields._DetailForm.field)
                    .OfType<IElement>()
                    .Any(x => x.getMetaClass()?.@equals(_formAndFields.__MetaClassElementFieldData) ?? false))
            {
                // Sets the information in cache, that the element was already added
                cache.MetaClassAlreadyAdded = true;

                // Add the element itself
                var metaClassField = _factory.create(_formAndFields.__MetaClassElementFieldData);
                metaClassField.set(_FormAndFields._MetaClassElementFieldData.name, "Metaclass");

                form.get<IReflectiveCollection>(_FormAndFields._DetailForm.field).add(metaClassField);
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
                    .get<IReflectiveCollection>(_FormAndFields._DetailForm.field)
                    .OfType<IObject>()
                    .FirstOrDefault(x => x.getOrDefault<string>(_FormAndFields._FieldData.name) == propertyName);

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

                    column.set(_FormAndFields._FieldData.name, propertyName);
                    column.set(_FormAndFields._FieldData.title, propertyName);
                    column.set(_FormAndFields._FieldData.isReadOnly, isReadOnly);

                    form.get<IReflectiveCollection>(_FormAndFields._DetailForm.field).add(column);
                }

                // Makes the field to an enumeration, if explicitly requested or the type behind is an enumeration
                column.set(
                    _FormAndFields._FieldData.isEnumeration,
                    column.getOrDefault<bool>(_FormAndFields._FieldData.isEnumeration) | DotNetHelper.IsEnumeration(propertyValue?.GetType()));
            }
        }

        /// <summary>
        /// Adds the fields for the form by going through the properties of the metaclass.
        /// It only adds fields, when they are not already added to the given list or detail form
        /// </summary>
        /// <param name="form">Form that will be extended. Must be list or detail form.</param>
        /// <param name="metaClass">Metaclass to be used</param>
        /// <param name="creationMode">Creation Mode to be used</param>
        /// <returns>true, if the metaclass is not null and if the metaclass contains at least on</returns>
        private bool AddToFormByMetaclass(IObject form, IElement metaClass, CreationMode creationMode, FormCreatorCache? cache = null)
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

            if (!cache.MetaClassAlreadyAdded && creationMode.HasFlagFast(CreationMode.AddMetaClass))
            {
                var metaClassField = _factory.create(_formAndFields.__MetaClassElementFieldData);
                metaClassField.set(_FormAndFields._MetaClassElementFieldData.name, "Metaclass");
                form.get<IReflectiveSequence>(_FormAndFields._ListForm.field).add(0, metaClassField);

                cache.MetaClassAlreadyAdded = true;
            }
            
            foreach (var property in classifierMethods)
            {
                wasInMetaClass = true;
                var propertyName = property.get("name")!.ToString();

                if (focusOnPropertyNames && !cache.FocusOnPropertyNames.Contains(propertyName))
                {
                    // Skip the property name, when we would like to have focus on certain property names
                    continue;
                }

                var isAlreadyIn = form
                    .get<IReflectiveCollection>(_FormAndFields._DetailForm.field)
                    .OfType<IObject>()
                    .Any(x => x.getOrDefault<string>(_FormAndFields._FieldData.name) == propertyName);

                if (isAlreadyIn)
                {
                    continue;
                }

                var column = GetFieldForProperty(property, creationMode);
                form.get<IReflectiveCollection>(_FormAndFields._DetailForm.field).add(column);
            }

            return wasInMetaClass;
        }

        /// <summary>
        /// Takes the given uml item and includes it into the form.
        /// The element can be of type enumeration, class or property.
        ///
        /// For the creation rules, see user_formmanager.adoc
        /// </summary>
        /// <param name="form">Form that will be enriched</param>
        /// <param name="umlElement">The uml element, property, class or type that will be added</param>
        /// <param name="creationMode">The creation mode</param>
        /// <returns>true, if an alement was created</returns>
        public bool AddToFormByUmlElement(IElement form, IElement umlElement, CreationMode creationMode)
        {
            if (form == null) throw new ArgumentNullException(nameof(form));
            if (umlElement == null) throw new ArgumentNullException(nameof(umlElement));

            var noDuplicate = creationMode.HasFlagFast(CreationMode.NoDuplicate);
            
            // First, select the type of the form
            var isDetailForm = 
                ClassifierMethods.IsSpecializedClassifierOf(form.getMetaClass(), _formAndFields.__DetailForm);
            var isListForm = 
                ClassifierMethods.IsSpecializedClassifierOf(form.getMetaClass(), _formAndFields.__ListForm);
            var isExtentForm = 
                ClassifierMethods.IsSpecializedClassifierOf(form.getMetaClass(), _formAndFields.__ExtentForm);
            var isNoneOfTheForms = !(isDetailForm || isListForm || isExtentForm);
            if (isNoneOfTheForms)
            {
                throw new InvalidOperationException("Given element is not a detail, a list or an extent form");
            }

            // Second, select the type of the umlElement
            var uml = GiveMe.Scope.GetUmlData();
            var isPropertyUml =
                ClassifierMethods.IsSpecializedClassifierOf(umlElement.getMetaClass(), uml.Classification.__Property);
            var isClassUml =
                ClassifierMethods.IsSpecializedClassifierOf(umlElement.getMetaClass(), uml.StructuredClassifiers.__Class);
            var isEnumerationUml =
                ClassifierMethods.IsSpecializedClassifierOf(umlElement.getMetaClass(), uml.SimpleClassifiers.__Enumeration);
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
                if (noDuplicate && FormHelper.GetField(form, NamedElementMethods.GetName(umlElement)) != null)
                {
                    // Field is already existing
                    return false;
                }

                var column = GetFieldForProperty(umlElement, creationMode);
                form.get<IReflectiveCollection>(_FormAndFields._DetailForm.field).add(column);
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
                    if (noDuplicate && FormHelper.GetListTabForPropertyName(form, propertyName) != null )
                    {
                        // List form is already existing
                        return false;
                    }
                    
                    // Property is a collection, so a list form is created for the property
                    var tabs = form.get<IReflectiveCollection>(_FormAndFields._ExtentForm.tab);
                    
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
                form.get<IReflectiveCollection>(_FormAndFields._DetailForm.field).add(column);
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
        private IElement GetFieldForProperty(IObject property, CreationMode creationMode)
        {
            var propertyType = PropertyMethods.GetPropertyType(property);
            var isForListForm = creationMode.HasFlagFast(CreationMode.ForListForms);
            var propertyName = property.get<string>("name");
            var propertyIsEnumeration = PropertyMethods.IsCollection(property);
            var isReadOnly = creationMode.HasFlagFast(CreationMode.ReadOnly);

            // Check, if field property is an enumeration
            _uml ??= _workspaceLogic.GetUmlData();
            _primitiveTypes ??= _workspaceLogic.GetPrimitiveData();
            _uriResolver ??= _workspaceLogic.GetTypesWorkspace();

            _stringType ??= _primitiveTypes.__String;
            _integerType ??= _primitiveTypes.__Integer;
            _booleanType ??= _primitiveTypes.__Boolean;
            _realType ??= _primitiveTypes.__Real;
            _dateTimeType ??= _uriResolver.Resolve(CoreTypeNames.DateTimeType, ResolveType.Default, false);

            // Checks, if the property is an enumeration.
            if (propertyType?.metaclass != null)
            {
                if (propertyType.metaclass.@equals(_uml.SimpleClassifiers.__Enumeration) && !isForListForm)
                {
                    return CreateFieldForEnumeration(propertyName, propertyType, creationMode);
                }

                if (propertyType.@equals(_booleanType) && !isForListForm)
                {
                    // If we have a boolean and the field is not for a list form
                    var checkbox = _factory.create(_formAndFields.__CheckboxFieldData);
                    checkbox.set(_FormAndFields._CheckboxFieldData.name, propertyName);
                    checkbox.set(_FormAndFields._CheckboxFieldData.title, propertyName);
                    checkbox.set(_FormAndFields._CheckboxFieldData.isReadOnly, isReadOnly);
                    return checkbox;
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
                        var elements = _factory.create(_formAndFields.__SubElementFieldData);
                        elements.set(_FormAndFields._SubElementFieldData.name, propertyName);
                        elements.set(_FormAndFields._SubElementFieldData.title, propertyName);
                        elements.set(_FormAndFields._SubElementFieldData.defaultTypesForNewElements,
                            ClassifierMethods.GetSpecializations(propertyType).ToList());
                        elements.set(_FormAndFields._SubElementFieldData.isReadOnly, isReadOnly);
                        
                        // Create the internal form out of the metaclass
                        var enumerationListForm = CreateListFormForMetaClass(propertyType, CreationMode.All);
                        elements.set(_FormAndFields._SubElementFieldData.form, enumerationListForm);

                        return elements;
                    }

                    // It can just contain one element
                    var reference = _factory.create(_formAndFields.__ReferenceFieldData);
                    reference.set(_FormAndFields._ReferenceFieldData.name, propertyName);
                    reference.set(_FormAndFields._ReferenceFieldData.title, propertyName);
                    reference.set(_FormAndFields._ReferenceFieldData.isReadOnly, isReadOnly);

                    return reference;
                }
            }

            if (propertyType == null)
            {
                // If we have something else than a primitive type and it is not for a list form
                var element = _factory.create(propertyIsEnumeration
                    ? _formAndFields.__SubElementFieldData
                    : _formAndFields.__TextFieldData);

                // It can just contain one element
                element.set(_FormAndFields._SubElementFieldData.name, propertyName);
                element.set(_FormAndFields._SubElementFieldData.title, propertyName);
                element.set(_FormAndFields._SubElementFieldData.isReadOnly, isReadOnly);
                return element;
            }

            // Per default, assume some kind of text
            var column = _factory.create(_formAndFields.__TextFieldData);
            column.set(_FormAndFields._TextFieldData.name, propertyName);
            column.set(_FormAndFields._TextFieldData.title, propertyName);
            column.set(_FormAndFields._TextFieldData.isReadOnly, isReadOnly);

            // If propertyType is an integer, the field can be smaller
            if (propertyType.@equals(_integerType))
            {
                column.set(_FormAndFields._TextFieldData.width, 10);
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
            var comboBox = _factory.create(_formAndFields.__DropDownFieldData);
            comboBox.set(_FormAndFields._DropDownFieldData.name, propertyName);
            comboBox.set(_FormAndFields._DropDownFieldData.title, propertyName);
            comboBox.set(_FormAndFields._DropDownFieldData.isReadOnly, isReadOnly);

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