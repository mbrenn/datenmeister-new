using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Forms.FormFactory;

namespace DatenMeister.Forms.CollectionForms;

/// <summary>
/// In case there is no specific item given, remove the property values from the tabs
/// This is required since we would like to use a tab for an Object Form, but also for a Collection Form.
/// The Collection Form is not working on item's property value level, so the setting of the property does
/// not make any sense at all
/// </summary>
public class RemovePropertyFromCollectionFormTabs : FormFactoryBase, ICollectionFormFactory
{
    /// <summary>
    /// Updates the provided collection form by removing the "property" attribute from the tabs.
    /// Ensures that tabs within the form no longer have the specified property set.
    /// </summary>
    /// <param name="parameter">
    /// The parameter object containing configuration data for the creation of the collection form.
    /// </param>
    /// <param name="context">
    /// The creation context within which the collection form is being generated.
    /// It provides additional environmental details or constraints.
    /// </param>
    /// <param name="result">
    /// The result object where the outcome of the form creation process is stored.
    /// This includes the generated form, which will be updated in this method.
    /// </param>
    public void CreateCollectionForm(CollectionFormFactoryParameter parameter, FormCreationContext context,
        FormCreationResultOneForm result)
    {
        if (result.Form == null) return;
        // Go through the tabs and unset the property
        foreach (var tab in result.Form.getOrDefault<IReflectiveCollection>(_Forms._CollectionForm.tab)
                     .OfType<IElement>())
        {
            if (tab.isSet(_Forms._TableForm.property))
            {
                tab.unset(_Forms._TableForm.property);
            }
                
            result.IsManaged = true;
        }
    }
}