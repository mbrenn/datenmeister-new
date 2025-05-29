using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Forms.FormModifications;

/// <summary>
///     Defines the interface to modify the form, depending on the context
/// </summary>
public interface IFormModificationPlugin
{
    /// <summary>
    ///     Allows the plugin to modify the form. This method is called by the FormsPlugin for each added plugin
    /// </summary>
    /// <param name="context">
    ///     Form Context defining the elements for which the form was created
    ///     and the purpose
    /// </param>
    /// <param name="form">
    ///     The form that had been created. This form can be directly be modified since
    ///     it is a copy.
    /// </param>
    /// <returns>true, if the form has been modified</returns>
    public bool ModifyForm(FormCreationContext context, IElement form);
}