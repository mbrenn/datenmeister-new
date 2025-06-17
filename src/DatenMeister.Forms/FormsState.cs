using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Forms.FormModifications;
using DatenMeister.Forms.Helper;

namespace DatenMeister.Forms;


public class FormsState
{
    /// <summary>
    /// Stores the list of form modification plugins which may modify the factory 
    /// </summary>
    public List<Action<NewFormCreationContext>> NewFormModificationPlugins { get; } = [];
    
    /// <summary>
    /// Lists all form modificationPlugins
    /// </summary>
    [Obsolete]
    public List<IFormModificationPlugin> FormModificationPlugins { get; } = [];
        

    /// <summary>
    ///     Calls all the form modification plugins, if allowed.
    /// </summary>
    /// <param name="formCreationContext">The creation context used by the plugins</param>
    /// <param name="form">The form that is evaluated</param>
    [Obsolete]
    public void CallFormsModificationPlugins(
        FormFactoryContext context,
        FormCreationContext formCreationContext, 
        ref IElement form)
    {
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