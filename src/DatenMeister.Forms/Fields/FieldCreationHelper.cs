using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms.FormFactory;
using DatenMeister.Forms.Helper;
using DatenMeister.Forms.TableForms;

namespace DatenMeister.Forms.Fields;

public static class FieldCreationHelper
{
    /// <summary>
    ///     Adds the fields for the form by going through the properties of the metaclass.
    ///     It only adds fields, when they are not already added to the given list or detail form
    /// </summary>
    /// <param name="rowOrObjectForm">Form that will be extended. Must be list or detail form.</param>
    /// <param name="metaClass">Metaclass to be used</param>
    /// <param name="parameter">Being </param>
    /// <param name="context">Creation Mode to be used</param>
    /// <returns>true, if the metaclass is not null and if the metaclass contains at least on</returns>
    public static bool AddFieldsToRowOrTableFormByMetaClass(
        IElement rowOrObjectForm,
        IElement? metaClass,
        FormFactoryParameterBase parameter,
        FormCreationContext context)
    {
        if (metaClass == null)
            return false;
        var cache = context.LocalScopeStorage.Get<FormCreatorCache>();

        if (!cache.CoveredMetaClasses.Add(metaClass))
        {
            // Already covered so we don't to manage it
            return false;
        }

        var isTableForm = rowOrObjectForm.metaclass?.equals(_Forms.TheOne.__TableForm) == true;

        var wasInMetaClass = false;

        var classifierMethods =
            ClassifierMethods.GetPropertiesOfClassifier(metaClass)
                .Where(x => x.isSet("name")).ToList();

        foreach (var property in classifierMethods)
        {
            wasInMetaClass = true;
            var propertyName = property.get<string?>("name");

            if (propertyName == null || !cache.CoveredPropertyNames.Add(propertyName))
            {
                // We already managed this property
                continue;
            }

            var isAlreadyIn = rowOrObjectForm
                .get<IReflectiveCollection>(_Forms._RowForm.field)
                .OfType<IObject>()
                .Any(x => x.getOrDefault<string>(_Forms._FieldData.name) == propertyName);

            if (isAlreadyIn)
                continue;

            var column = FormCreation.CreateField(
                new FieldFactoryParameter
                {
                    Extent = parameter.Extent,
                    ExtentTypes = parameter.ExtentTypes,
                    PropertyName = propertyName ?? string.Empty,
                    PropertyType = property,
                    IsInTable = isTableForm
                },
                context.Clone());
            if (column.Form != null)
            {
                rowOrObjectForm.get<IReflectiveCollection>(_Forms._RowForm.field).add(column.Form);
            }

            FormCreationResult.AddToFormCreationProtocol(
                rowOrObjectForm,
                "[FormCreator.AddFieldsToRowOrObjectFormByMetaClass]: Added field by Metaclass for property: " +
                NamedElementMethods.GetName(propertyName));
        }

#if DEBUG
        if (!ValidateTableOrRowForm.ValidateForm(rowOrObjectForm))
            throw new InvalidOperationException("Something went wrong during creation of form");
#endif

        return wasInMetaClass;
    }

    /// <summary>
    ///     Adds the fields to the properties as given in the object itself.
    ///     The properties are retrieved by reading the available property types
    ///     from the object itself via the interface IObjectAllProperties
    /// </summary>
    /// <param name="form">Form to be extended</param>
    /// <param name="item">Item to be evaluated</param>
    /// <param name="context">Context in which the form is created</param>
    public static void AddFieldsToFormByPropertyValues(
        IObject form,
        object item,
        FormCreationContext context)
    {
        var cache = context.LocalScopeStorage.Get<FormCreatorCache>();
        if (item is not IObjectAllProperties itemAsAllProperties)
            // The object does not allow the retrieving of properties
            return;

        if (item is not IObject itemAsObject)
            // The object cannot be converted and FormCreator does not support
            // non MOF Objects
            return;

        var isReadOnly = context.IsReadOnly;
        var factory = context.Global.Factory;

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

                FormCreationResult.AddToFormCreationProtocol(
                    form,
                    "[FormCreator.AddFieldsToFormByPropertyValues]: " + NamedElementMethods.GetName(column));
            }

            // Makes the field to an enumeration, if explicitly requested or the type behind is an enumeration
            column.set(
                _Forms._FieldData.isEnumeration,
                column.getOrDefault<bool>(_Forms._FieldData.isEnumeration) |
                DotNetHelper.IsEnumeration(propertyValue?.GetType()));
        }
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

                return x.PropertyName.Equals(y.PropertyName);
            }

            public int GetHashCode(P obj)
            {
                return obj.PropertyName.GetHashCode();
            }
        }

        public class MofObjectComparer : IEqualityComparer<IElement?>
        {
            public bool Equals(IElement? x, IElement? y)
            {
                if (x == null || y == null) return false;

                return MofObject.AreEqual(x, y);
            }

            public int GetHashCode(IElement obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}