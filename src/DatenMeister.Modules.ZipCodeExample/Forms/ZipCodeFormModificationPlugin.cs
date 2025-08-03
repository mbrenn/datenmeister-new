using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Forms;
using DatenMeister.Forms.FormFactory;
using DatenMeister.Forms.FormModifications;
using FormCreationContext = DatenMeister.Forms.FormCreationContext;

namespace DatenMeister.Modules.ZipCodeExample.Forms;

/// <summary>
/// Defines the plugin which modifies the zip
/// </summary>
public class ZipCodeFormModificationPlugin : IRowFormFactory
{
    public void CreateRowForm(RowFormFactoryParameter parameter, FormCreationContext context, FormCreationResultMultipleForms result)
    {
            var form = result.Forms.FirstOrDefault();
        var metaClass = parameter.MetaClass;
        if (metaClass != null && metaClass.equals(_Management.TheOne.__Workspace) == true && form != null)
        {
            // Ok, I got it
            var fields = form.get<IReflectiveSequence>(_Forms._RowForm.field);
            var actionField = context.Global.Factory.create(_Forms.TheOne.__ActionFieldData);
            actionField.set(_Forms._ActionFieldData.actionName, "ZipExample.CreateExample");
            actionField.set(_Forms._ActionFieldData.title, "Create Zip Model");
            actionField.set(_Forms._ActionFieldData.name, "CreateZipModel");
            fields.add(actionField);
            result.IsManaged = true;
        }
    }
}