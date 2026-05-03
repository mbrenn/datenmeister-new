using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Forms.FormFactory;

namespace DatenMeister.Forms.Fields;

public class FieldFromPropertyValue : FormFactoryBase, IFieldFactory
{
    public void CreateField(FieldFactoryParameter parameter, FormCreationContext context,
        FormCreationResultOneForm result)
    {
        // We do not need to create the field twice
        if (result.IsMainContentCreated)
            return;

        IElement column;
        var propertyValue = parameter.PropertyValue;
        var propertyName = parameter.PropertyName;
        var factory = context.Global.Factory;
        if (propertyValue == null || string.IsNullOrEmpty(parameter.PropertyName))
            return;

        // Guess by content, which type of field shall be created
        var propertyType = propertyValue.GetType();

        if (DotNetHelper.IsEnumeration(propertyType))
        {
            column = factory.create(_Forms.TheOne.FieldTypes.__SubElementFieldData);
        }
        else if (propertyValue is IObject)
        {
            column = factory.create(_Forms.TheOne.FieldTypes.__ReferenceFieldData);
            column.set(_Forms._FieldTypes._ReferenceFieldData.isSelectionInline, false);
        }
        else
        {
            column = factory.create(_Forms.TheOne.FieldTypes.__TextFieldData);
        }

        column.set(_Forms._FieldTypes._FieldData.name, propertyName);
        column.set(_Forms._FieldTypes._FieldData.title, propertyName);
        column.set(_Forms._FieldTypes._FieldData.isReadOnly, context.IsReadOnly);
        

        // Makes the field to an enumeration, if explicitly requested or the type behind is an enumeration
        column.set(
            _Forms._FieldTypes._FieldData.isEnumeration,
            column.getOrDefault<bool>(_Forms._FieldTypes._FieldData.isEnumeration) |
            DotNetHelper.IsEnumeration(propertyValue?.GetType()));

        result.Form = column;

        result.IsMainContentCreated = true;
        result.IsManaged = true;
    }
}