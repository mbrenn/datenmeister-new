using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms;
using DatenMeister.Forms.FormModifications;

namespace DatenMeister.Extent.Forms;

/// <summary>
///     Creates the buttons which allows the user to create new instances by evaluating the extent type to which
///     the element is currently belonging to.
///     Here, the property 'ExtentConfiguration.ExtentDefaultTypes' is being used to retrieve the values
/// </summary>
public class CreateInstanceButtonsForTableForms : IFormModificationPlugin
{
    public bool ModifyForm(FormCreationContext context, IElement form)
    {
        var extent = context.DetailElement?.GetExtentOf();
        if (context.FormType == _Forms.___FormType.Table &&
            extent != null)
        {
            var added = false;
            // Adds the default types as defined in the extent
            var defaultTypes = extent.getOrDefault<IReflectiveCollection>(ExtentConfiguration.ExtentDefaultTypes);
            if (defaultTypes != null)
            {
                foreach (var defaultType in defaultTypes.OfType<IElement>())
                {
                    FormMethods.AddToFormCreationProtocol(
                        form,
                        "[CreateInstanceButtonsForTableForms]: Add DefaultType per ExtentDefaultTypes property " +
                        NamedElementMethods.GetName(defaultType));
                        
                    FormMethods.AddDefaultTypeForNewElement(form, defaultType);
                    added = true;
                }
            }

            return added;
        }

        return false;
    }
}