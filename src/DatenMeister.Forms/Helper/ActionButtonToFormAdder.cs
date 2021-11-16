using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Forms.FormModifications;

namespace DatenMeister.Forms.Helper
{
    /// <summary>
    /// This class allows the adding of an action button to an existing form.
    /// The action button is only added when the form is created for a certain metaclass. 
    /// </summary>
    public static class ActionButtonToFormAdder
    {
        /// <summary>
        /// Initializes a new instance of the ActionButtonToFormadder
        /// </summary>
        /// <param name="formsPluginState">The plugin interface to the forms</param>
        /// <param name="adder">The parameter of the addition</param>
        public static void AddActionButton(FormsPluginState formsPluginState, ActionButtonAdderParameter adder)
        {
            formsPluginState.FormModificationPlugins.Add(
                new FormModification(adder));
        }

        private class FormModification : IFormModificationPlugin
        {
            private readonly ActionButtonAdderParameter _parameter;

            public FormModification(ActionButtonAdderParameter parameter)
            {
                _parameter = parameter;
            }
            
            public void ModifyForm(FormCreationContext context, IElement form)
            {

                if (
                    (_parameter.MetaClass == null || context.MetaClass?.equals(_parameter.MetaClass) == true) &&
                    (_parameter.FormType == null || context.FormType == _parameter.FormType) &&
                    (string.IsNullOrEmpty(context.Configuration?.ViewModeId) || context.Configuration?.ViewModeId == _parameter.ViewMode))
                {
                    // Calls the OnCall method to allow property debugging
                    _parameter.OnCallSuccess?.Invoke();
                    var formMetaClass = form.getMetaClass();

                    IElement? formWithFields;
                    if (formMetaClass?.equals(_DatenMeister.TheOne.Forms.__ExtentForm) == true)
                    {
                        formWithFields = FormMethods.GetDetailForms(form).FirstOrDefault();
                        if (formWithFields == null)
                        {
                            formWithFields = FormMethods.GetListForms(form).FirstOrDefault();
                            if (formWithFields == null)
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        formWithFields = form;
                    }

                    if (formWithFields is not null)
                    {
                        var fields = formWithFields.get<IReflectiveCollection>(_DatenMeister._Forms._DetailForm.field);
                        var actionField = MofFactory.Create(form, _DatenMeister.TheOne.Forms.__ActionFieldData);
                        actionField.set(_DatenMeister._Forms._ActionFieldData.actionName, _parameter.ActionName);
                        actionField.set(_DatenMeister._Forms._ActionFieldData.title, _parameter.Title);
                        actionField.set(_DatenMeister._Forms._ActionFieldData.name, _parameter.ActionName);
                        fields.add(actionField);
                    }
                }
            }
        }
    }
}