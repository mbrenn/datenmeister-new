using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.Core.TypeIndexAssembly;
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
    /// <param name="workspaceLogic">Defines the workspacelogic to be used</param>
    /// <param name="rowOrObjectForm">Form that will be extended. Must be list or detail form.</param>
    /// <param name="metaClass">Metaclass to be used</param>
    /// <param name="parameter">Being </param>
    /// <param name="context">Creation Mode to be used</param>
    /// <returns>true, if the metaclass is not null and if the metaclass contains at least on</returns>
    public static bool AddFieldsToRowOrTableFormByMetaClass(
        IWorkspaceLogic workspaceLogic,
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
        
        var typeIndexLogic = new TypeIndexLogic (workspaceLogic);
        var foundClassModel = typeIndexLogic.FindClassModelByMetaClass(metaClass);
        if (foundClassModel == null)
        {
            return false;
        }

        var classifierMethods = foundClassModel.Attributes;

        foreach (var property in classifierMethods)
        {
            wasInMetaClass = true;
            var propertyName = property.Name;

            if (!cache.CoveredPropertyNames.Add(propertyName))
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
                    Property = property.MetaAttribute,
                    IsInTable = isTableForm,
                    MetaClass = metaClass
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
    /// <param name="parameter">Parameter which describe the parameter's under
    /// which the parant element was created. This value is used to
    /// set the Extent and ExtentTypes for the field.</param>
    /// <param name="context">Context in which the form is created</param>
    public static void AddFieldsToFormByPropertyValues(
        IObject form,
        object item,
        FormFactoryParameterBase parameter,
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

        // Creates the form out of the properties of the item
        var properties = itemAsAllProperties.getPropertiesBeingSet();

        var focusOnPropertyNames = cache.FocusOnPropertyNames.Any();

        foreach (var propertyName in properties
                     .Where(property => !cache.CoveredPropertyNames.Contains(property)))
        {
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
                var clonedContext = context.Clone();
                var resultingField = FormCreation.CreateField(
                    new FieldFactoryParameter
                    {
                        Extent = parameter.Extent,
                        ExtentTypes = parameter.ExtentTypes,
                        PropertyName = propertyName,
                        PropertyValue = propertyValue
                    },
                    clonedContext);

                if (resultingField.Form != null)
                {
                    form.get<IReflectiveCollection>(_Forms._RowForm.field).add(resultingField.Form);
                    cache.CoveredPropertyNames.Add(propertyName);
                    
                    FormCreationResult.AddToFormCreationProtocol(
                        form,
                        "[FieldCreationHelper.AddFieldsToFormByPropertyValues]: Field created for "
                        + NamedElementMethods.GetName(column));
                }
                else
                {
                    FormCreationResult.AddToFormCreationProtocol(
                        form,
                        "[FieldCreationHelper.AddFieldsToFormByPropertyValues]: Field was NOT created for " +
                        "" + NamedElementMethods.GetName(column));
                }
            }
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