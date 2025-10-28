using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms.FormFactory;

namespace DatenMeister.Forms.Fields;

public class FieldAnyFieldData : FormFactoryBase, IFieldFactory
{
    public void CreateField(FieldFactoryParameter parameter, FormCreationContext context,
        FormCreationResultOneForm result)
    {
        // We do not need to create the field twice
        if (result.IsMainContentCreated)
            return;

        var property = parameter.PropertyType;
        var propertyName = string.IsNullOrEmpty(parameter.PropertyName)
            ? property?.getOrDefault<string>(_UML._Classification._Property.name) ?? string.Empty
            : parameter.PropertyName;
        var propertyValue = parameter.PropertyValue;
        var propertyIsCollection =
            property != null && PropertyMethods.IsCollection(property)
            || propertyValue != null && DotNetHelper.IsOfEnumeration(propertyValue);

        // If we have something else than a primitive type and it is not for a list form
        var element = context.Global.Factory.create(propertyIsCollection
            ? _Forms.TheOne.__SubElementFieldData
            : _Forms.TheOne.__AnyDataFieldData);

        // It can just contain one element
        element.set(_Forms._SubElementFieldData.name, propertyName);
        element.set(_Forms._SubElementFieldData.title, propertyName);
        element.set(_Forms._SubElementFieldData.isReadOnly, context.IsReadOnly);
        element.set(_Forms._SubElementFieldData.isEnumeration, propertyIsCollection);

        result.Form = element;

        result.IsManaged = true;
        result.IsMainContentCreated = true;
    }
}