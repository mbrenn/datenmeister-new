using System.Globalization;
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
using DatenMeister.Forms.FormFactory;
using _PrimitiveTypes = DatenMeister.Core.Models.EMOF._PrimitiveTypes;

namespace DatenMeister.Forms.FormCreator;

/// <summary>
///     Creates a view out of the given extent, elements (collection) or element).
/// </summary>
public abstract class FormCreator
{
    /// <summary>
    ///     Defines a small helper enumeration which can be used to define the type of the
    ///     umlElement for the method AddToFormByUmlElement.
    /// </summary>
    public enum FormUmlElementType
    {
        /// <summary>
        ///     Definition is not given
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

    internal readonly ExtentSettings ExtentSettings;

    /// <summary>
    ///     Stores the reference to the view logic which is required to get the views
    ///     for the tabs of the extent form
    /// </summary>
    protected FormMethods? FormLogic { get; }

    protected IScopeStorage ScopeStorage { get; }

    /// <summary>
    ///     Stores the associated workspace logic
    /// </summary>
    protected IWorkspaceLogic WorkspaceLogic { get; }
    
    #region CachedTypes

    class CachedTypesDefinition
    {
        /// <summary>
        /// Caches the boolean type
        /// </summary>
        internal IElement? BooleanType;

        /// <summary>
        /// Caches the datatime type
        /// </summary>
        internal IElement? DateTimeType;

        /// <summary>
        ///     Just intermediate memory
        /// </summary>
        internal IFactory? Factory;

        /// <summary>
        /// The cached real type
        /// </summary>
        internal IElement? RealType;

        /// <summary>
        /// The cached integer type
        /// </summary>
        internal IElement? IntegerType;

        /// <summary>
        /// The cached string type
        /// </summary>
        internal IElement? StringType;

        /// <summary>
        /// The cached unlimited natural type
        /// </summary>
        internal IElement? UnlimitedNaturalType;
    }

    CachedTypesDefinition CachedTypes { get; } = new CachedTypesDefinition();
    
    #endregion

    private Workspace? _uriResolver;

    /// <summary>
    ///     Initializes a new instance of the FormCreator class
    /// </summary>
    /// <param name="workspaceLogic">The workspace logic to be used</param>
    /// <param name="scopeStorage">The scope storage</param>
    protected FormCreator(
        IWorkspaceLogic workspaceLogic,
        IScopeStorage scopeStorage)
    {
        FormLogic = new FormMethods(workspaceLogic, scopeStorage);
        ScopeStorage = scopeStorage;
        ExtentSettings = scopeStorage.Get<ExtentSettings>();
        WorkspaceLogic = workspaceLogic;
    }

    /// <summary>
    /// Gets the factory to create the fields and forms
    /// </summary>
    internal IFactory GetMofFactory(FormFactoryConfiguration? configuration)
    {
        if (configuration?.Factory != null)
        {
            return configuration.Factory;
        }

        if (CachedTypes.Factory != null) return CachedTypes.Factory;

        var userExtent = FormLogic?.GetUserFormExtent(true);
        CachedTypes.Factory = userExtent != null
            ? new MofFactory(userExtent)
            : InMemoryObject.TemporaryFactory;

        return CachedTypes.Factory;
    }

    /// <summary>
    ///     Creates the fields of the form by evaluation of the given object.
    ///     Depending on the creation mode, the evaluation will be done by metaclass
    ///     or by evaluation of the properties.
    ///     This method is independent whether it is used in a list or extent form.
    /// </summary>
    /// <param name="rowOrTableForm">Form which will be extended by the given object</param>
    /// <param name="item">Item being used</param>
    /// <param name="creationMode">Creation mode for the form. Whether by metaclass or ByProperties</param>
    /// <param name="cache">Cache being used to store intermediate items</param>
    protected void AddFieldsToForm(
        IObject rowOrTableForm,
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
            if (cache.CoveredMetaClasses.Add(metaClass))
            {
                wasInMetaClass = AddFieldsToRowOrTableFormByMetaClass(
                    rowOrTableForm,
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
            AddFieldsToFormByPropertyValues(rowOrTableForm, item, creationMode, cache);
            rowOrTableForm.set(_Forms._RowForm.allowNewProperties, true);
        }

        // Third phase: Add metaclass element itself
        var addMetaClass = creationMode.AutomaticMetaClassField;
        if (!cache.MetaClassAlreadyAdded
            && addMetaClass
            && !FormMethods.HasMetaClassFieldInForm(rowOrTableForm))
        {
            // Add the element itself
            var metaClassField = GetMofFactory(creationMode).create(_Forms.TheOne.__MetaClassElementFieldData);
            metaClassField.set(_Forms._MetaClassElementFieldData.name, "Metaclass");

            rowOrTableForm.get<IReflectiveCollection>(_Forms._RowForm.field).add(metaClassField);

            // Sets the information in cache, that the element was already added
            cache.MetaClassAlreadyAdded = true;
        }

#if DEBUG
        if (!FormMethods.ValidateForm(rowOrTableForm))
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
    protected void AddFieldsToFormByPropertyValues(
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
        var factory = GetMofFactory(creationMode);

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
                .get<IReflectiveCollection>(_Forms._RowForm.field)
                .OfType<IObject>()
                .FirstOrDefault(x => x.getOrDefault<string>(_Forms._FieldData.name) == propertyName);

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
                    column = factory.create(_Forms.TheOne.__SubElementFieldData);
                }
                else if (propertyValue is IObject)
                {
                    column = factory.create(_Forms.TheOne.__ReferenceFieldData);
                    column.set(_Forms._ReferenceFieldData.isSelectionInline, false);
                }
                else
                {
                    column = factory.create(_Forms.TheOne.__TextFieldData);
                }

                column.set(_Forms._FieldData.name, propertyName);
                column.set(_Forms._FieldData.title, propertyName);
                column.set(_Forms._FieldData.isReadOnly, isReadOnly);

                form.get<IReflectiveCollection>(_Forms._RowForm.field).add(column);

                FormMethods.AddToFormCreationProtocol(
                    form,
                    "[FormCreator.AddFieldsToFormByPropertyValues]: " + NamedElementMethods.GetName(column));
            }

            // Makes the field to an enumeration, if explicitly requested or the type behind is an enumeration
            column.set(
                _Forms._FieldData.isEnumeration,
                column.getOrDefault<bool>(_Forms._FieldData.isEnumeration) |
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
    /// <param name="rowOrObjectForm">Form that will be extended. Must be list or detail form.</param>
    /// <param name="metaClass">Metaclass to be used</param>
    /// <param name="configuration">Creation Mode to be used</param>
    /// <param name="cache">Cache of reportCreator cache</param>
    /// <returns>true, if the metaclass is not null and if the metaclass contains at least on</returns>
    protected bool AddFieldsToRowOrTableFormByMetaClass(
        IObject rowOrObjectForm,
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

            var isAlreadyIn = rowOrObjectForm
                .get<IReflectiveCollection>(_Forms._RowForm.field)
                .OfType<IObject>()
                .Any(x => x.getOrDefault<string>(_Forms._FieldData.name) == propertyName);

            if (isAlreadyIn) continue;

            var column = CreateFieldForProperty(property,
                propertyName,
                configuration);

            rowOrObjectForm.get<IReflectiveCollection>(_Forms._RowForm.field).add(column);

            FormMethods.AddToFormCreationProtocol(rowOrObjectForm,
                "[FormCreator.AddFieldsToRowOrObjectFormByMetaClass]: Added field by Metaclass: " +
                NamedElementMethods.GetName(column));
        }

        // After having created all the properties, add the meta class information at the end
        if (!cache.MetaClassAlreadyAdded
            && configuration.AutomaticMetaClassField
            && !FormMethods.HasMetaClassFieldInForm(rowOrObjectForm))
        {
            var metaClassField = GetMofFactory(configuration).create(_Forms.TheOne.__MetaClassElementFieldData);
            metaClassField.set(_Forms._MetaClassElementFieldData.name, "Metaclass");
            metaClassField.set(_Forms._MetaClassElementFieldData.title, "Metaclass");
            rowOrObjectForm.get<IReflectiveSequence>(_Forms._TableForm.field).add(metaClassField);

            cache.MetaClassAlreadyAdded = true;

            FormMethods.AddToFormCreationProtocol(rowOrObjectForm,
                "[FormCreator.AddFieldsToRowOrObjectFormByMetaClass]: Added metaclass information");
        }

        // Sorts the field by important properties
        SortFieldsByImportantProperties(rowOrObjectForm);

#if DEBUG
        if (!FormMethods.ValidateForm(rowOrObjectForm))
            throw new InvalidOperationException("Something went wrong during creation of form");
#endif

        return wasInMetaClass;
    }

    /// <summary>
    ///     Takes the given uml item and includes it into the form.
    ///     The element can be of type enumeration, class or property.
    ///     For the creation rules, see chapter "FormManager" in the Documentation
    /// </summary>
    /// <param name="form">Form that will be enriched. It may be an object, collection, row oder table form</param>
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
        var isRowForm =
            ClassifierMethods.IsSpecializedClassifierOf(form.getMetaClass(),
                _Forms.TheOne.__RowForm);
        var isTableForm =
            ClassifierMethods.IsSpecializedClassifierOf(form.getMetaClass(),
                _Forms.TheOne.__TableForm);
        var isCollectionForm =
            ClassifierMethods.IsSpecializedClassifierOf(form.getMetaClass(),
                _Forms.TheOne.__CollectionForm);
        var isObjectForm =
            ClassifierMethods.IsSpecializedClassifierOf(form.getMetaClass(),
                _Forms.TheOne.__ObjectForm);
        var isNoneOfTheForms = !(isRowForm || isTableForm || isCollectionForm || isObjectForm);
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
        if (isRowForm && isPropertyUml || isTableForm && isPropertyUml)
        {
            if (noDuplicate && FormMethods.GetField(form, NamedElementMethods.GetName(umlClassOrProperty)) != null)
                // Field is already existing
                return false;

            var column = CreateFieldForProperty(umlClassOrProperty,
                null,
                creationMode);
            form.get<IReflectiveCollection>(_Forms._RowForm.field).add(column);

            FormMethods.AddToFormCreationProtocol(form,
                "[FormCreator.AddFieldsToFormByMetaClassProperty]: Added Property to row or table form: " +
                NamedElementMethods.GetName(column));
            return true;
        }

        if (isCollectionForm && isPropertyUml || isObjectForm && isPropertyUml)
        {
            var isPropertyACollection = PropertyMethods.IsCollection(umlClassOrProperty);

            if (!isPropertyACollection)
            {
                // Property is a single element, so a field is added to the detail form, if not already existing
                var detailForm = FormMethods.GetOrCreateRowFormIntoForm(form);
                var result = AddFieldsToFormByMetaClassProperty(detailForm, umlClassOrProperty, creationMode);

                FormMethods.AddToFormCreationProtocol(form,
                    "[FormCreator.AddFieldsToFormByMetaClassProperty]: Added Property to Collection Form: " +
                    NamedElementMethods.GetName(umlClassOrProperty));

                return result;
            }

            var propertyName = umlClassOrProperty.getOrDefault<string>(_UML._CommonStructure._NamedElement.name);
            if (noDuplicate && FormMethods.GetTableFormForPropertyName(form, propertyName) != null)
            {
                // List form is already existing
                return false;
            }

            // Property is a collection, so a list form is created for the property
            var tabs = form.get<IReflectiveCollection>(_Forms._CollectionForm.tab);

            // Now try to figure out the metaclass
            var tableFormCreator = new TableFormCreator(WorkspaceLogic, ScopeStorage);
            var listForm = tableFormCreator.CreateTableFormForProperty(
                umlClassOrProperty,
                FormFactoryConfiguration.CreateByMetaClassOnly);

            FormMethods.AddToFormCreationProtocol(form,
                "[FormCreator.AddFieldsToFormByMetaClassProperty]: Added Table Form to Collection Form: " +
                NamedElementMethods.GetName(umlClassOrProperty));

            tabs.add(listForm);
            return true;
        }

        // Now, let's parse the enumerations
        if (isRowForm && isEnumerationUml || isTableForm && isEnumerationUml)
        {
            var propertyName = NamedElementMethods.GetName(umlClassOrProperty)
                .ToLower(CultureInfo.InvariantCulture);
            var column = CreateFieldForEnumeration(propertyName, umlClassOrProperty, creationMode);
            form.get<IReflectiveCollection>(_Forms._RowForm.field).add(column);

            FormMethods.AddToFormCreationProtocol(form,
                "[FormCreator.AddFieldsToFormByMetaClassProperty]: Added Enumeration to row/table form: " +
                NamedElementMethods.GetName(column));

            return true;
        }

        if (isCollectionForm && isEnumerationUml || isObjectForm && isEnumerationUml)
        {
            var detailForm = FormMethods.GetOrCreateRowFormIntoForm(form);
            var result = AddFieldsToFormByMetaClassProperty(detailForm, umlClassOrProperty, creationMode);

            FormMethods.AddToFormCreationProtocol(form,
                "[FormCreator.AddFieldsToFormByMetaClassProperty]: Added Enumeration to Collection Form: " +
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
    /// <param name="property">Uml-Property which is requesting a field</param>
    /// <param name="propertyName">Name of the property wo whose values the list form shall be created.</param>
    /// <param name="configuration">Defines the mode how to create the fields</param>
    /// <returns>The field data</returns>
    protected IElement CreateFieldForProperty(IObject? property,
        string? propertyName,
        FormFactoryConfiguration configuration)
    {
        if (property == null && propertyName == null)
            throw new InvalidOperationException("property == null && propertyName == null");

        var factory = GetMofFactory(configuration);

        var propertyType = property == null ? null : PropertyMethods.GetPropertyType(property);

        propertyName ??= property.get<string>("name");
        var propertyIsCollection = property != null && PropertyMethods.IsCollection(property);
        var isReadOnly = configuration.IsReadOnly;

        // Check, if field property is an enumeration
        _uriResolver ??= WorkspaceLogic.GetTypesWorkspace();

        CachedTypes.StringType ??= _PrimitiveTypes.TheOne.__String;
        CachedTypes.IntegerType ??= _PrimitiveTypes.TheOne.__Integer;
        CachedTypes.BooleanType ??= _PrimitiveTypes.TheOne.__Boolean;
        CachedTypes.RealType ??= _PrimitiveTypes.TheOne.__Real;
        CachedTypes.UnlimitedNaturalType ??= _PrimitiveTypes.TheOne.__UnlimitedNatural;
        CachedTypes.DateTimeType ??= _uriResolver?.ResolveElement(CoreTypeNames.DateTimeType, ResolveType.Default, false);

        // Checks, if the property is an enumeration.
        var propertyTypeMetaClass = propertyType?.metaclass; // The type of the type (enum, class, struct, etc)
        if (propertyTypeMetaClass != null && propertyType != null)
        {
            if (propertyTypeMetaClass.equals(_UML.TheOne.SimpleClassifiers.__Enumeration))
            {
                return CreateFieldForEnumeration(propertyName, propertyType, configuration);
            }

            if (propertyType.equals(CachedTypes.BooleanType))
            {
                // If we have a boolean and the field is not for a list form
                var checkbox = factory.create(_Forms.TheOne.__CheckboxFieldData);
                checkbox.set(_Forms._CheckboxFieldData.name, propertyName);
                checkbox.set(_Forms._CheckboxFieldData.title, propertyName);
                checkbox.set(_Forms._CheckboxFieldData.isReadOnly, isReadOnly);
                return checkbox;
            }

            if (propertyType.equals(CachedTypes.DateTimeType))
            {
                var dateTimeField = factory.create(_Forms.TheOne.__DateTimeFieldData);
                dateTimeField.set(_Forms._CheckboxFieldData.name, propertyName);
                dateTimeField.set(_Forms._CheckboxFieldData.title, propertyName);
                dateTimeField.set(_Forms._CheckboxFieldData.isReadOnly, isReadOnly);
                return dateTimeField;
            }

            if (
                !propertyType.equals(CachedTypes.StringType) &&
                !propertyType.equals(CachedTypes.IntegerType) &&
                !propertyType.equals(CachedTypes.RealType) &&
                !propertyType.equals(CachedTypes.UnlimitedNaturalType) && 
                !propertyType.equals(CachedTypes.DateTimeType))
            {
                // If we have something else than a primitive type and it is not for a list form
                if (propertyIsCollection)
                {
                    // It can contain multiple factory
                    var elementsField = factory.create(_Forms.TheOne.__SubElementFieldData);
                    elementsField.set(_Forms._SubElementFieldData.name, propertyName);
                    elementsField.set(_Forms._SubElementFieldData.title, propertyName);
                    elementsField.set(_Forms._SubElementFieldData.isReadOnly,
                        configuration.IsForTableForm || configuration.IsReadOnly);

                    if (!configuration.IsForTableForm)
                    {
                        FormMethods.AddDefaultTypeForNewElement(elementsField, propertyType);
                    }

                    elementsField.set(
                        _Forms._SubElementFieldData.includeSpecializationsForDefaultTypes, true);
                    elementsField.set(_Forms._SubElementFieldData.isReadOnly, isReadOnly);

                    IElement? enumerationListForm = null;
                    if (!configuration.IsForTableForm)
                    {
                        if (FormLogic != null)
                            enumerationListForm =
                                new TableFormFactory(WorkspaceLogic, ScopeStorage)
                                    .CreateTableFormForMetaClass(propertyType, configuration);

                        // Create the internal form out of the metaclass
                        if (enumerationListForm == null
                            && configuration.CreateByMetaClass)
                        {
                            var tableFormCreator = new TableFormCreator(WorkspaceLogic, ScopeStorage);
                            enumerationListForm =
                                tableFormCreator.CreateTableFormForMetaClass(
                                    propertyType,
                                    configuration with { IsForTableForm = true },
                                    property as IElement);
                        }

                        if (enumerationListForm != null)
                            elementsField.set(_Forms._SubElementFieldData.form, enumerationListForm);
                    }

                    return elementsField;
                }

                if (Configuration.CreateDropDownForReferences)
                {
                    var dropDownByQueryData = factory.create(_Forms.TheOne.__DropDownByQueryData);
                    var queryStatement = factory.create(_DataViews.TheOne.__QueryStatement);
                    var queryByExtent = factory.create(_DataViews.TheOne.__SelectFromAllWorkspacesNode);
                    var queryFlatten = factory.create(_DataViews.TheOne.__FlattenNode);
                    var queryByMetaClass = factory.create(_DataViews.TheOne.__FilterByMetaclassNode);
                        
                    queryStatement.AddCollectionItem(_DataViews._QueryStatement.nodes, queryByExtent);
                    queryStatement.AddCollectionItem(_DataViews._QueryStatement.nodes, queryByMetaClass);
                    queryStatement.AddCollectionItem(_DataViews._QueryStatement.nodes, queryFlatten);
                    queryStatement.set(_DataViews._QueryStatement.resultNode, queryByMetaClass);
                        
                    dropDownByQueryData.set(_Forms._DropDownByQueryData.query, queryStatement);
                        
                    queryFlatten.set(_DataViews._FlattenNode.input, queryByExtent);
                        
                    queryByMetaClass.set(_DataViews._FilterByMetaclassNode.input, queryFlatten);
                    queryByMetaClass.set(_DataViews._FilterByMetaclassNode.metaClass, propertyType);
                        
                    dropDownByQueryData.set(_Forms._SubElementFieldData.name, propertyName);
                    dropDownByQueryData.set(_Forms._SubElementFieldData.title, propertyName);

                    return dropDownByQueryData;
                }
                // ReSharper disable HeuristicUnreachableCode
#pragma warning disable CS0162 // Unreachable code detected
                else
                {
                    // It can just contain one element (or is in list view)
                    var reference = factory.create(_Forms.TheOne.__ReferenceFieldData);
                    reference.set(_Forms._ReferenceFieldData.name, propertyName);
                    reference.set(_Forms._ReferenceFieldData.title, propertyName);
                    reference.set(_Forms._ReferenceFieldData.isReadOnly, isReadOnly);
                    reference.set(_Forms._ReferenceFieldData.isEnumeration, propertyIsCollection);
                    reference.set(
                        _Forms._ReferenceFieldData.metaClassFilter,
                        new[] { propertyType });

                    return reference;
                }   
#pragma warning restore CS0162 // Unreachable code detected
                // ReSharper restore HeuristicUnreachableCode
            }
        }

        if (propertyType == null)
        {
            // If we have something else than a primitive type and it is not for a list form
            var element = factory.create(propertyIsCollection
                ? _Forms.TheOne.__SubElementFieldData
                : _Forms.TheOne.__AnyDataFieldData);

            // It can just contain one element
            element.set(_Forms._SubElementFieldData.name, propertyName);
            element.set(_Forms._SubElementFieldData.title, propertyName);
            element.set(_Forms._SubElementFieldData.isReadOnly, isReadOnly);
            element.set(_Forms._SubElementFieldData.isEnumeration, propertyIsCollection);

            if (propertyType != null)
            {
                FormMethods.AddDefaultTypeForNewElement(element, propertyType);
            }

            return element;
        }

        // Per default, assume some kind of text
        var column = factory.create(_Forms.TheOne.__TextFieldData);
        column.set(_Forms._TextFieldData.name, propertyName);
        column.set(_Forms._TextFieldData.title, propertyName);
        column.set(_Forms._TextFieldData.isReadOnly, isReadOnly);

        // If propertyType is an integer, the field can be smaller
        if (propertyType.equals(CachedTypes.IntegerType)
            || propertyType.equals(CachedTypes.UnlimitedNaturalType))
        {
            column.set(_Forms._TextFieldData.width, 10);
        }

        return column;
    }

    /// <summary>
    ///     Creates a field for the given enumeration
    /// </summary>
    /// <param name="propertyName">Name of the property being used to add title and name</param>
    /// <param name="propertyType">Type of the enumeration</param>
    /// <param name="creationMode">The used creation mode</param>
    /// <returns>The created element of the enumeration</returns>
    protected IElement CreateFieldForEnumeration(
        string propertyName,
        IElement propertyType,
        FormFactoryConfiguration creationMode)
    {
        var factory = GetMofFactory(creationMode);
        var isReadOnly = creationMode.IsReadOnly;
        // If we have an enumeration (C#: Enum) and the field is not for a list form
        var comboBox = factory.create(_Forms.TheOne.__DropDownFieldData);
        comboBox.set(_Forms._DropDownFieldData.name, propertyName);
        comboBox.set(_Forms._DropDownFieldData.title, propertyName);
        comboBox.set(_Forms._DropDownFieldData.isReadOnly, isReadOnly);

        var values = EnumerationMethods.GetEnumValues(propertyType);
        comboBox.set(
            _Forms._DropDownFieldData.values,
            values.Select(x =>
            {
                var data = factory.create(_Forms.TheOne.__ValuePair);
                data.set(_Forms._ValuePair.name, x);
                data.set(_Forms._ValuePair.value, x);
                return data;
            }).ToList());
        return comboBox;
    }
        
    protected static void SortFieldsByImportantProperties(IObject form)
    {
        var fields = form.getOrDefault<IReflectiveSequence>(_Forms._TableForm.field);
        if (fields == null) return;
        var fieldsAsList = fields.OfType<IElement>().ToList();

        // Check if the name is within the list, if yes, push it to the front
        var fieldName = fieldsAsList.FirstOrDefault(x =>
            x.getOrDefault<string>(_UML._CommonStructure._NamedElement.name) ==
            _UML._CommonStructure._NamedElement.name);

        if (fieldName != null)
        {
            fields.remove(fieldName);
            fields.add(0, fieldName);

            FormMethods.AddToFormCreationProtocol(
                form,
                "[FormCreator.SortFieldsByImportantProperties]: Field 'name' was put up-front");
        }

        // Performs a resetting of all properties
        // form.set(_Forms._TableForm.field, fieldsAsList);
    }

    /// <summary>
    ///     Checks whether at least one field is given.
    ///     If no field is given, then the one text field for the name will be added
    /// </summary>
    /// <param name="form">Form to be checked</param>
    protected static void AddTextFieldForNameIfNoFieldAvailable(IObject form)
    {
        // If the field is empty, create an empty textfield with 'name' as a placeholder
        var fieldLength =
            form.getOrDefault<IReflectiveCollection>(_Forms._TableForm.field)?.Count() ?? 0;
        if (fieldLength == 0)
        {
            var factory = new MofFactory(form);
            var textFieldData = factory.create(_Forms.TheOne.__TextFieldData);
            textFieldData.set(_Forms._TextFieldData.name, "name");
            textFieldData.set(_Forms._TextFieldData.title, "name");

            form.AddCollectionItem(_Forms._TableForm.field, textFieldData);

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
    protected class FormCreatorCache
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