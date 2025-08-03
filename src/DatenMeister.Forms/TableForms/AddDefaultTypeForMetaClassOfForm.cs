using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms.FormFactory;

namespace DatenMeister.Forms.TableForms;

/// <summary>
/// Adds the button for a default type according to the metaclass of the TableForm 
/// </summary>
public class AddDefaultTypeForMetaClassOfForm : ITableFormFactory
{
    public void CreateTableForm(TableFormFactoryParameter parameter, FormCreationContext context,
        FormCreationResultMultipleForms result)
    {
        foreach (var form in result.Forms)
        {
            var defaultType = form.getOrDefault<IElement>(_Forms._TableForm.metaClass);
            if (defaultType == null)
                return;

            AddDefaultTypeIfNotExists(result, form, defaultType);
        }
    }

    public static void AddDefaultTypeIfNotExists(FormCreationResultOneForm result, IElement defaultType)
    {
        if (result.Form == null) return;
        
        AddDefaultTypeIfNotExists(result, result.Form, defaultType);
    }
    
    public static void AddDefaultTypeIfNotExists(
        FormCreationResult result,
        IElement form,
        IElement defaultType)
    {
        var currentDefaultPackages =
            form.get<IReflectiveCollection>(_Forms._TableForm.defaultTypesForNewElements);
        if (currentDefaultPackages.OfType<IElement>().Any(x =>
                x.getOrDefault<IElement>(
                        _Forms._DefaultTypeForNewElement.metaClass)
                    ?.equals(defaultType) == true))
        {
            result.AddToFormCreationProtocol(
                $"[FormMethods.AddDefaultTypeForNewElement] Not added because default type is already existing: {NamedElementMethods.GetName(defaultType)}");
            // No adding, because it already exists
            return;
        }

        var defaultTypeInstance =
            new MofFactory(form).create(_Forms.TheOne.__DefaultTypeForNewElement);
        defaultTypeInstance.set(_Forms._DefaultTypeForNewElement.metaClass, defaultType);
        defaultTypeInstance.set(_Forms._DefaultTypeForNewElement.name,
            NamedElementMethods.GetName(defaultType));
        currentDefaultPackages.add(defaultTypeInstance);
        result.AddToFormCreationProtocol(
            $"[FormMethods.AddDefaultTypeForNewElement] Added defaulttype: {NamedElementMethods.GetName(defaultType)}");

        result.IsManaged = true;
    }
}