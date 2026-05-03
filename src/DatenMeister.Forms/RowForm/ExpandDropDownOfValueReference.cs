using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms.FormFactory;

namespace DatenMeister.Forms.RowForm;

public class ExpandDropDownOfValueReference : FormFactoryBase, IRowFormFactory, ITableFormFactory
{
    public void CreateRowForm(RowFormFactoryParameter parameter, 
        FormCreationContext context,
        FormCreationResultMultipleForms result)
    {
        ExpandDropDownValuesOfValueReference(context, result);
    }

    public void CreateTableForm(TableFormFactoryParameter parameter, 
        FormCreationContext context, 
        FormCreationResultMultipleForms result)
    {
        ExpandDropDownValuesOfValueReference(context, result);
    }
    
    /// <summary>
    ///     Expands the dropdown values of the the DropDownField.
    ///     The DropDownField supports a reference field which is not resolved by every Form Client.
    ///     So, the DropDownField can already be resolved on server side
    /// </summary>
    public static void ExpandDropDownValuesOfValueReference(
        FormCreationContext context, 
        FormCreationResultMultipleForms result)
    {
        foreach (var listOrDetailForm in result.Forms)
        {
            var factory = context.Global.Factory;
            var fields = listOrDetailForm.get<IReflectiveCollection>(_Forms._FormTypes._TableForm.field);
            foreach (var field in fields.OfType<IElement>())
            {
                if (field.getMetaClass()?.@equals(_Forms.TheOne.FieldTypes.__DropDownFieldData) != true) continue;

                var byEnumeration =
                    field.getOrDefault<IElement>(_Forms._FieldTypes._DropDownFieldData.valuesByEnumeration);
                var byValues =
                    field.getOrDefault<IReflectiveCollection>(_Forms._FieldTypes._DropDownFieldData.values);
                if (byValues == null && byEnumeration != null)
                {
                    var enumeration = EnumerationMethods.GetEnumValues(byEnumeration);
                    foreach (var value in enumeration)
                    {
                        var element = factory.create(_Forms.TheOne.FieldTypes.__ValuePair);
                        element.set(_Forms._FieldTypes._ValuePair.name, value);
                        element.set(_Forms._FieldTypes._ValuePair.value, value);
                        field.AddCollectionItem(_Forms._FieldTypes._DropDownFieldData.values, element);
                    }

                    result.IsManaged = true;
                    result.AddToFormCreationProtocol(
                        $"[ExpandDropDownValuesOfValueReference] Expanded DropDown-Values for {NamedElementMethods.GetName(field)}");
                }
            }
        }
    }
}