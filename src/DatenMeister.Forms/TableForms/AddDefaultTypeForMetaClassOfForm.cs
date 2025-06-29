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
public class AddDefaultTypeForMetaClassOfForm : INewTableFormFactory
{
    public void CreateTableForm(TableFormFactoryParameter parameter, NewFormCreationContext context,
        FormCreationResult result)
    {
        var form = result.Form;
        if (form == null)
            return;

        var defaultType = form.getOrDefault<IElement>(_Forms._TableForm.metaClass);
        if (defaultType == null)
            return;

        var currentDefaultPackages =
            form.get<IReflectiveCollection>(_Forms._TableForm.defaultTypesForNewElements);
        if (currentDefaultPackages.OfType<IElement>().Any(x =>
                x.getOrDefault<IElement>(
                        _Forms._DefaultTypeForNewElement.metaClass)
                    ?.@equals(defaultType) == true))
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