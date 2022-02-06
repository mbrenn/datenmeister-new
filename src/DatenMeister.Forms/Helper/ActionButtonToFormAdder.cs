using System.Collections.Generic;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Forms.FormModifications;

namespace DatenMeister.Forms.Helper
{
    /// <summary>
    ///     This class allows the adding of an action button to an existing form.
    ///     The action button is only added when the form is created for a certain metaclass.
    /// </summary>
    public static class ActionButtonToFormAdder
    {
        /// <summary>
        ///     Initializes a new instance of the ActionButtonToFormAdder
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

            /// <summary>
            /// Modifies the form by checking whether the action button can be applied
            /// </summary>
            /// <param name="context">The form creation context in which the application shall be checked. This is
            /// the specific instance of the requesting form context</param>
            /// <param name="form">The form to which the changes shall be applied</param>
            /// <returns>true, if the form has been modified</returns>
            public bool ModifyForm(FormCreationContext context, IElement form)
            {
                if (
                    FormCreationContext.EvaluateMatching(_parameter, context) && 
                    (_parameter.PredicateForElement == null || _parameter.PredicateForElement(context.DetailElement)))
                {
                    // Calls the OnCall method to allow property debugging
                    _parameter.OnCallSuccess?.Invoke();
                    var formMetaClass = form.getMetaClass();

                    var forms = new List<IObject>();
                    if (formMetaClass?.equals(_DatenMeister.TheOne.Forms.__ExtentForm) == true)
                    {
                        if (context.FormType != _DatenMeister._Forms.___FormType.ObjectList)
                            forms.AddRange(FormMethods.GetDetailForms(form));

                        if (context.FormType != _DatenMeister._Forms.___FormType.TreeItemDetail &&
                            context.FormType != _DatenMeister._Forms.___FormType.Detail &&
                            context.FormType != _DatenMeister._Forms.___FormType.TreeItemDetailExtension)
                            forms.AddRange(FormMethods.GetListForms(form));
                    }
                    else
                    {
                        forms.Add(form);
                    }

                    foreach (var formWithFields in forms)
                    {
                        var fields = formWithFields.get<IReflectiveSequence>(_DatenMeister._Forms._DetailForm.field);
                        var actionField = MofFactory.Create(form, _DatenMeister.TheOne.Forms.__ActionFieldData);
                        actionField.set(_DatenMeister._Forms._ActionFieldData.actionName, _parameter.ActionName);
                        actionField.set(_DatenMeister._Forms._ActionFieldData.title, _parameter.Title);
                        actionField.set(_DatenMeister._Forms._ActionFieldData.name, _parameter.ActionName);

                        if (_parameter.ActionButtonPosition == -1)
                        {
                            fields.add(actionField);
                        }
                        else
                        {
                            fields.add(_parameter.ActionButtonPosition, actionField);
                        }
                    }

                    FormMethods.AddToFormCreationProtocol(
                        form,
                        $"[ActionButtonToFormAdder]: Added Button{_parameter.Title}");

                    return true;
                }

                return false;
            }

            public override string ToString()
            {
                return "ActionButton: " + _parameter.ActionName;
            }
        }
    }
}