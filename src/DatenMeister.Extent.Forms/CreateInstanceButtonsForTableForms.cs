using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms;
using DatenMeister.Forms.FormFactory;
using DatenMeister.Forms.FormModifications;
using DatenMeister.Forms.Helper;

namespace DatenMeister.Extent.Forms;

/// <summary>
///     Creates the buttons which allows the user to create new instances by evaluating the extent type to which
///     the element is currently belonging to.
///     Here, the property 'ExtentConfiguration.ExtentDefaultTypes' is being used to retrieve the values
/// </summary>
public class CreateInstanceButtonsForTableForms : INewTableFormFactory
{
    public void CreateTableForm(
        TableFormFactoryParameter parameter,
        NewFormCreationContext context,
        FormCreationResult result)
    {
        var collection = parameter.Collection;
        if (collection == null)
            return;
        
        if (result.Form == null)
            throw new InvalidOperationException("Form is null");

        var extent = collection.GetUriExtentOf();
        if (extent != null)
        {
            var added = false;
            // Adds the default types as defined in the extent
            var defaultTypes = extent.getOrDefault<IReflectiveCollection>(ExtentConfiguration.ExtentDefaultTypes);
            if (defaultTypes != null)
            {
                foreach (var defaultType in defaultTypes.OfType<IElement>())
                {
                    result.AddToFormCreationProtocol(
                        "[CreateInstanceButtonsForTableForms]: Add DefaultType per ExtentDefaultTypes property " +
                        NamedElementMethods.GetName(defaultType));
                        
                    FormMethods.AddDefaultTypeForNewElement(result.Form, defaultType);
                    added = true;
                }
            }

            result.IsManaged = added;
        }
    }

    public void CreateTableFormForMetaclass(IElement metaClass, NewFormCreationContext context, FormCreationResult result)
    {
    }
}