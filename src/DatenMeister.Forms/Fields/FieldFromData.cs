using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms.FormFactory;
using DatenMeister.Forms.Helper;

namespace DatenMeister.Forms.Fields;

public class FieldFromData(IWorkspaceLogic workspaceLogic) : IFieldFactory
{
    #region CachedTypes

    private class CachedTypesDefinition
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
    
    public void CreateField(
        FieldFactoryParameter parameter,
        FormCreationContext context,
        FormCreationResultOneForm result)
    {
        // We do not need to create the field twice
        if (result.IsMainContentCreated)
            return;
        
        var property = parameter.PropertyType;
        var propertyName = string.IsNullOrEmpty(parameter.PropertyName) 
            ? property.getOrDefault<string>(_UML._Classification._Property.name)
            : parameter.PropertyName;
        
        if (property == null && propertyName == null)
            throw new InvalidOperationException("property == null && propertyName == null");

        var factory = context.Global.Factory;

        var propertyType = property == null ? null : PropertyMethods.GetPropertyType(property);

        propertyName ??= property.get<string>("name");
        if (propertyName == null)
        {
            throw new InvalidOperationException("propertyName == null");
        }
        // Checks, if the property is an enumeration.
        var propertyIsCollection = property != null && PropertyMethods.IsCollection(property);
        var isReadOnly = context.IsReadOnly;

        // Checks, if field property is an enumeration
        _uriResolver ??= workspaceLogic.GetTypesWorkspace();

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
                result.Form = CreateFieldForEnumeration(propertyName, propertyType, context);
            }

            else if (propertyType.equals(CachedTypes.BooleanType))
            {
                // If we have a boolean and the field is not for a list form
                var checkbox = factory.create(_Forms.TheOne.__CheckboxFieldData);
                checkbox.set(_Forms._CheckboxFieldData.name, propertyName);
                checkbox.set(_Forms._CheckboxFieldData.title, propertyName);
                checkbox.set(_Forms._CheckboxFieldData.isReadOnly, isReadOnly);
                result.Form = checkbox;
            }

            else if (propertyType.equals(CachedTypes.DateTimeType))
            {
                var dateTimeField = factory.create(_Forms.TheOne.__DateTimeFieldData);
                dateTimeField.set(_Forms._CheckboxFieldData.name, propertyName);
                dateTimeField.set(_Forms._CheckboxFieldData.title, propertyName);
                dateTimeField.set(_Forms._CheckboxFieldData.isReadOnly, isReadOnly);
                result.Form = dateTimeField;
            }

            else if (
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
                        parameter.IsInTable || context.IsReadOnly);

                    elementsField.set(
                        _Forms._SubElementFieldData.includeSpecializationsForDefaultTypes, true);
                    elementsField.set(_Forms._SubElementFieldData.isReadOnly, isReadOnly);

                    if (!parameter.IsInTable)
                    {
                        var clonedContext = context.Clone();
                        var enumerationListForm =
                            FormCreation.CreateTableForm(
                                new TableFormFactoryParameter
                                {
                                    Extent = parameter.Extent,
                                    ExtentTypes = parameter.ExtentTypes,
                                    MetaClass = propertyType,
                                    ParentMetaClass = parameter.MetaClass,
                                    ParentPropertyName = parameter.PropertyName
                                }, clonedContext).Forms.FirstOrDefault();

                        if (enumerationListForm != null)
                            elementsField.set(_Forms._SubElementFieldData.form, enumerationListForm);
                    }

                    result.Form = elementsField;
                }

                else if (Configuration.CreateDropDownForReferences)
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

                    result.Form = dropDownByQueryData;
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

                    result.Form = reference;
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

            result.Form = element;
        }

        else if (result.Form == null)
        {
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

            result.Form = column;
        }

        if (result.Form != null)
        {
            result.IsManaged = true;
            result.IsMainContentCreated = true;
        }
    }

    /// <summary>
    /// Creates a field for the given enumeration
    /// </summary>
    /// <param name="propertyName">Name of the property being used to add title and name</param>
    /// <param name="propertyType">Type of the enumeration</param>
    /// <param name="context">The used creation mode</param>
    /// <returns>The created element of the enumeration</returns>
    private IElement CreateFieldForEnumeration(
        string propertyName,
        IElement propertyType,
        FormCreationContext context)
    {
        var factory = context.Global.Factory;
        var isReadOnly = context.IsReadOnly;
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
}