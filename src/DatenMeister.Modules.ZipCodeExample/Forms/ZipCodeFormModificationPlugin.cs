using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Forms.FormModifications;

namespace DatenMeister.Modules.ZipCodeExample.Forms;

/// <summary>
/// Defines the plugin which modifies the zip
/// </summary>
public class ZipCodeFormModificationPlugin : IFormModificationPlugin
{
    /// <summary>
    /// Modifies the form of a workspace that the user can create directly a new zip extent
    /// </summary>
    /// <param name="context">Context of the form</param>
    /// <param name="form">Form to behandled</param>
    /// <returns></returns>
    public bool ModifyForm(FormCreationContext context, IElement form)
    {
        if (context.FormType == _Forms.___FormType.Row
            && context.MetaClass?.equals(_Management.TheOne.__Workspace) == true)
        {
            // Ok, I got it

            var fields = form.get<IReflectiveSequence>(_Forms._RowForm.field);
            var actionField = MofFactory.CreateElement(form, _Forms.TheOne.__ActionFieldData);
            actionField.set(_Forms._ActionFieldData.actionName, "ZipExample.CreateExample");
            actionField.set(_Forms._ActionFieldData.title, "Create Zip Model");
            actionField.set(_Forms._ActionFieldData.name, "CreateZipModel");
            fields.add(actionField);
            return true;
        }

        return false;
    }
}