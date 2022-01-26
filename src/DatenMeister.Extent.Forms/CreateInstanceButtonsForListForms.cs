using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Forms.FormModifications;

namespace DatenMeister.Extent.Forms
{
    /// <summary>
    ///     Creates the buttons which allows the user to create new instances by evaluating the extent type to which
    ///     the element is currently belonging to.
    ///     Here, the property 'ExtentConfiguration.ExtentDefaultTypes' is being used to retrieve the values
    /// </summary>
    public class CreateInstanceButtonsForListForms : IFormModificationPlugin
    {
        public void ModifyForm(FormCreationContext context, IElement form)
        {
            var extent = context.DetailElement?.GetExtentOf();
            if (context.FormType == _DatenMeister._Forms.___FormType.ObjectList &&
                extent != null)
            {
                // Adds the default types as defined in the extent
                var defaultTypes = extent.getOrDefault<IReflectiveCollection>(ExtentConfiguration.ExtentDefaultTypes);
                if (defaultTypes != null)
                {
                    var inForm =
                        form.get<IReflectiveCollection>(_DatenMeister._Forms._ListForm.defaultTypesForNewElements);

                    foreach (var defaultType in defaultTypes.OfType<IElement>()) inForm.add(defaultType);
                }
            }
        }
    }
}