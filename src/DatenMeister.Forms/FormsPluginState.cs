using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Forms.FormModifications;

namespace DatenMeister.Forms;

public class FormsPluginState
{
    /// <summary>
    /// Lists all form modificationPlugins
    /// </summary>
    public List<IFormModificationPlugin> FormModificationPlugins { get; } = new();
        

    /// <summary>
    ///     Calls all the form modification plugins, if allowed.
    /// </summary>
    /// <param name="configuration">The configuration under which the plugins shall be checked</param>
    /// <param name="formCreationContext">The creation context used by the plugins</param>
    /// <param name="form">The form that is evaluated</param>
    public void CallFormsModificationPlugins(
        FormFactoryConfiguration configuration,
        FormCreationContext formCreationContext, 
        ref IElement form)
    {
        if (configuration?.AllowFormModifications != true)
        {
            // Nothing to do
            return;
        }

        foreach (var plugin in FormModificationPlugins)
        {
            if (plugin.ModifyForm(formCreationContext, form))
            {
                FormMethods.AddToFormCreationProtocol(form,
                    $"[FormFactory.CallFormsModificationPlugins] Modified via plugin: {plugin} in context {formCreationContext}");
            }
        }
    }
}