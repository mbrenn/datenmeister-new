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
using DatenMeister.Forms.Helper;
using _PrimitiveTypes = DatenMeister.Core.Models.EMOF._PrimitiveTypes;

namespace DatenMeister.Forms.FormCreator;

[Obsolete]
/// <summary>
///     Creates a view out of the given extent, elements (collection) or element).
/// </summary>
public abstract class FormCreator
{
    /// <summary>
    /// Stores the cached factory which is used to create instances
    /// </summary>
    protected IFactory? CachedFactory { get; set; }
    
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

    /// <summary>
    ///     Initializes a new instance of the FormCreator class
    /// </summary>
    /// <param name="workspaceLogic">The workspace logic to be used</param>
    /// <param name="scopeStorage">The scope storage</param>
    protected FormCreator(
        IWorkspaceLogic workspaceLogic,
        IScopeStorage scopeStorage)
    {
        FormLogic = new FormMethods(workspaceLogic);
        ScopeStorage = scopeStorage;
        ExtentSettings = scopeStorage.Get<ExtentSettings>();
        WorkspaceLogic = workspaceLogic;
    }

    /// <summary>
    /// Gets the factory to create the fields and forms
    /// </summary>
    internal IFactory GetMofFactory(FormFactoryContext? configuration)
    {
        if (configuration?.Factory != null)
        {
            return configuration.Factory;
        }

        if (CachedFactory != null) return CachedFactory;

        var userExtent = FormLogic?.GetUserFormExtent(true);
        CachedFactory = userExtent != null
            ? new MofFactory(userExtent)
            : InMemoryObject.TemporaryFactory;

        return CachedFactory;
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
        FormFactoryContext creationMode,
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
        FormFactoryContext creationMode,
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
    /// <param name="context">Creation Mode to be used</param>
    /// <param name="cache">Cache of reportCreator cache</param>
    /// <returns>true, if the metaclass is not null and if the metaclass contains at least on</returns>
    protected bool AddFieldsToRowOrTableFormByMetaClass(
        IObject rowOrObjectForm,
        IObject? metaClass,
        FormFactoryContext context,
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

            var fieldFactory = new FieldCreator(WorkspaceLogic, ScopeStorage);
            var column = fieldFactory.CreateFieldForProperty(property,
                propertyName,
                context);

            rowOrObjectForm.get<IReflectiveCollection>(_Forms._RowForm.field).add(column);

            FormMethods.AddToFormCreationProtocol(rowOrObjectForm,
                "[FormCreator.AddFieldsToRowOrObjectFormByMetaClass]: Added field by Metaclass: " +
                NamedElementMethods.GetName(column));
        }

        // After having created all the properties, add the meta class information at the end
        if (!cache.MetaClassAlreadyAdded
            && context.AutomaticMetaClassField
            && !FormMethods.HasMetaClassFieldInForm(rowOrObjectForm))
        {
            var metaClassField = GetMofFactory(context).create(_Forms.TheOne.__MetaClassElementFieldData);
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

}