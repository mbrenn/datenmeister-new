using System.Globalization;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms.FormFactory;

namespace DatenMeister.Forms.FormCreator;

public class FieldCreator : FormCreator
{
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
    
    public FieldCreator(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        : base(workspaceLogic, scopeStorage)
    {

    }
    
    

    /// <summary>
    ///     Gets the field data, depending upon the given property
    /// </summary>
    /// <param name="property">Uml-Property which is requesting a field</param>
    /// <param name="propertyName">Name of the property wo whose values the list form shall be created.</param>
    /// <param name="context">Defines the mode how to create the fields</param>
    /// <returns>The field data</returns>
    public IElement CreateFieldForProperty(IObject? property,
        string? propertyName,
        FormFactoryContext context)
    {
        if (property == null && propertyName == null)
            throw new InvalidOperationException("property == null && propertyName == null");

        var factory = GetMofFactory(context);

        var propertyType = property == null ? null : PropertyMethods.GetPropertyType(property);

        propertyName ??= property.get<string>("name");
        var propertyIsCollection = property != null && PropertyMethods.IsCollection(property);
        var isReadOnly = context.IsReadOnly;

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
                return CreateFieldForEnumeration(propertyName, propertyType, context);
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
                        context.IsForTableForm || context.IsReadOnly);

                    if (!context.IsForTableForm)
                    {
                        FormMethods.AddDefaultTypeForNewElement(elementsField, propertyType);
                    }

                    elementsField.set(
                        _Forms._SubElementFieldData.includeSpecializationsForDefaultTypes, true);
                    elementsField.set(_Forms._SubElementFieldData.isReadOnly, isReadOnly);

                    IElement? enumerationListForm = null;
                    if (!context.IsForTableForm)
                    {
                        if (FormLogic != null)
                            enumerationListForm =
                                new TableFormFactory(WorkspaceLogic, ScopeStorage)
                                    .CreateTableFormForMetaClass(propertyType, context);

                        // Create the internal form out of the metaclass
                        if (enumerationListForm == null
                            && context.CreateByMetaClass)
                        {
                            var tableFormCreator = new TableFormCreator(WorkspaceLogic, ScopeStorage);
                            enumerationListForm =
                                tableFormCreator.CreateTableFormForMetaClass(
                                    propertyType,
                                    context with { IsForTableForm = true },
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
    public IElement CreateFieldForEnumeration(
        string propertyName,
        IElement propertyType,
        FormFactoryContext creationMode)
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
    public bool AddFieldToFormByMetaClassProperty(
        IElement form,
        IElement umlClassOrProperty,
        FormFactoryContext creationMode,
        FormUmlElementType umlElementType = FormUmlElementType.Unknown)
    {
        ArgumentNullException.ThrowIfNull(form);
        ArgumentNullException.ThrowIfNull(umlClassOrProperty);

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

            var fieldFactory = new FieldCreator(WorkspaceLogic, ScopeStorage);
            var column = fieldFactory.CreateFieldForProperty(umlClassOrProperty,
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
                var result = AddFieldToFormByMetaClassProperty(detailForm, umlClassOrProperty, creationMode);

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
                FormFactoryContext.CreateByMetaClassOnly);

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

            var fieldFactory = new FieldCreator(WorkspaceLogic, ScopeStorage);
            var column = fieldFactory.CreateFieldForEnumeration(propertyName, umlClassOrProperty, creationMode);
            form.get<IReflectiveCollection>(_Forms._RowForm.field).add(column);

            FormMethods.AddToFormCreationProtocol(form,
                "[FormCreator.AddFieldsToFormByMetaClassProperty]: Added Enumeration to row/table form: " +
                NamedElementMethods.GetName(column));

            return true;
        }

        if (isCollectionForm && isEnumerationUml || isObjectForm && isEnumerationUml)
        {
            var detailForm = FormMethods.GetOrCreateRowFormIntoForm(form);
            var result = AddFieldToFormByMetaClassProperty(detailForm, umlClassOrProperty, creationMode);

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
                added &= AddFieldToFormByMetaClassProperty(form, property, creationMode);

                FormMethods.AddToFormCreationProtocol(form,
                    "[FormCreator.AddFieldsToFormByMetaClassProperty]: Added Enumeration to Detail: " +
                    NamedElementMethods.GetName(property));
            }

            return added;
        }

        return false;
    }
}