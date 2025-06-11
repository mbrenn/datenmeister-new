using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms.FormCreator;

namespace DatenMeister.Forms.FieldFactory;

public static class FieldCreationHelper
{
    /// <summary>
    ///     Adds the fields for the form by going through the properties of the metaclass.
    ///     It only adds fields, when they are not already added to the given list or detail form
    /// </summary>
    /// <param name="rowOrObjectForm">Form that will be extended. Must be list or detail form.</param>
    /// <param name="metaClass">Metaclass to be used</param>
    /// <param name="context">Creation Mode to be used</param>
    /// <returns>true, if the metaclass is not null and if the metaclass contains at least on</returns>
    public static bool AddFieldsToRowOrTableFormByMetaClass(
        IObject rowOrObjectForm,
        IElement? metaClass,
        NewFormCreationContext context)
    {
        if (metaClass == null) return false;
        var cache = context.ScopeStorage.Get<FormCreatorCache>();
        if (!cache.CoveredMetaClasses.Add(metaClass))
        {
            // Already covered so we don't to manage it
            return false;
        }

        var wasInMetaClass = false;

        var classifierMethods =
            ClassifierMethods.GetPropertiesOfClassifier(metaClass)
                .Where(x => x.isSet("name")).ToList();

        foreach (var property in classifierMethods)
        {
            wasInMetaClass = true;
            var propertyName = property.get<string?>("name");
            
            var isAlreadyIn = rowOrObjectForm
                .get<IReflectiveCollection>(_Forms._RowForm.field)
                .OfType<IObject>()
                .Any(x => x.getOrDefault<string>(_Forms._FieldData.name) == propertyName);

            if (isAlreadyIn) continue;

            var column = FormCreation.CreateFieldForProperty(
                property,
                context);
            if (column.Result != null)
            {
                rowOrObjectForm.get<IReflectiveCollection>(_Forms._RowForm.field).add(column.Result);
            }
            
            FormMethods.AddToFormCreationProtocol(rowOrObjectForm,
                "[FormCreator.AddFieldsToRowOrObjectFormByMetaClass]: Added field by Metaclass: " +
                NamedElementMethods.GetName(column));
        }

#if DEBUG
        if (!FormMethods.ValidateForm(rowOrObjectForm))
            throw new InvalidOperationException("Something went wrong during creation of form");
#endif

        return wasInMetaClass;
    }
}