using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Forms;
using DatenMeister.Forms.FormModifications;
using DatenMeister.Modules.Forms;

namespace DatenMeister.Extent.Forms
{
    public class ExtentFormExtension : IFormModificationPlugin
    {
        public const string ViewNavigationActionType = "Extent.NavigateTo";

        public void ModifyForm(FormCreationContext context, IElement form)
        {
            if (context.MetaClass?.@equals(_DatenMeister.TheOne.Management.__Extent) == true
                && context.ViewMode == ViewModes.Default)
            {
                var detailForm = FormMethods.GetDetailForms(form).FirstOrDefault();
                if (detailForm == null)
                {
                    return;
                }

                var fields = detailForm.get<IReflectiveCollection>(_DatenMeister._Forms._DetailForm.field);
                var actionField = MofFactory.Create(form, _DatenMeister.TheOne.Forms.__ActionFieldData);
                actionField.set(_DatenMeister._Forms._ActionFieldData.actionName, ViewNavigationActionType);
                actionField.set(_DatenMeister._Forms._ActionFieldData.title, "View Extent");
                actionField.set(_DatenMeister._Forms._ActionFieldData.name, ViewNavigationActionType);
                fields.add(actionField);
            }
        }
    }
}