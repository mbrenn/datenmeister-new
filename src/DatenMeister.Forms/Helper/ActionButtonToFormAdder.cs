using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Forms.FormModifications;

namespace DatenMeister.Forms.Helper;

/// <summary>
///     This class allows the adding of an action button to an existing form.
///     The action button is only added when the form is created for a certain metaclass.
/// </summary>
public static class ActionButtonToFormAdder
{
    /// <summary>
    ///     Initializes a new instance of the ActionButtonToFormAdder
    /// </summary>
    /// <param name="formsState">The plugin interface to the forms</param>
    /// <param name="adder">The parameter of the addition</param>
    public static void AddActionButton(FormsState formsState, ActionButtonAdderParameter adder)
    {
        formsState.FormModificationPlugins.Add(
            new FormModification(adder));
    }

    private class FormModification(ActionButtonAdderParameter parameter) : IFormModificationPlugin
    {
        /// <summary>
        /// Modifies the form by checking whether the action button can be applied
        /// </summary>
        /// <param name="context">The form creation context in which the application shall be checked. This is
        /// the specific instance of the requesting form context</param>
        /// <param name="form">The form to which the changes shall be applied</param>
        /// <returns>true, if the form has been modified</returns>
        public bool ModifyForm(FormCreationContext context, IElement form)
        {
            parameter.OnCall?.Invoke(context.DetailElement, parameter);

            if (
                FormCreationContext.EvaluateMatching(parameter, context) &&
                (parameter.PredicateForElement == null || context.DetailElement == null || parameter.PredicateForElement(context.DetailElement)) &&
                (parameter.PredicateForContext == null || parameter.PredicateForContext(context))
            )
            {
                // Calls the OnCall method to allow property debugging
                parameter.OnCallSuccess?.Invoke(context.DetailElement, parameter);
                var formMetaClass = form.getMetaClass();

                var forms = new List<IObject>();
                if (formMetaClass?.equals(_Forms.TheOne.__CollectionForm) == true
                    || formMetaClass?.equals(_Forms.TheOne.__ObjectForm) == true)
                {
                    switch (context.FormType)
                    {
                        case _Forms.___FormType.Row:
                        case _Forms.___FormType.RowExtension:
                            forms.AddRange(FormMethods.GetRowForms(form));
                            break;
                        case _Forms.___FormType.Table:
                        case _Forms.___FormType.TableExtension:
                            forms.AddRange(FormMethods.GetTableForms(form));
                            break;
                    }
                }
                else
                {
                    forms.Add(form);
                }

                foreach (var formWithFields in forms)
                {
                    var fields = formWithFields.get<IReflectiveSequence>(_Forms._RowForm.field);
                    var actionField = MofFactory.CreateElement(form, _Forms.TheOne.__ActionFieldData);
                    actionField.set(_Forms._ActionFieldData.actionName, parameter.ActionName);
                    actionField.set(_Forms._ActionFieldData.title, parameter.Title);
                    actionField.set(_Forms._ActionFieldData.name, parameter.ActionName);

                    if (parameter.ButtonText != null && !string.IsNullOrEmpty(parameter.ButtonText))
                    {
                        actionField.set(_Forms._ActionFieldData.buttonText, parameter.ButtonText);
                    }

                    if (parameter.Parameter.Count > 0)
                    {
                        var parameter1 = MofFactory.CreateElement(form, null);
                        foreach (var (key, value) in parameter.Parameter)
                        {
                            parameter1.set(key, value);
                        }

                        actionField.set(_Forms._ActionFieldData.parameter, parameter1);
                    }

                    if (parameter.ActionButtonPosition == -1)
                    {
                        fields.add(actionField);
                    }
                    else
                    {
                        fields.add(parameter.ActionButtonPosition, actionField);
                    }
                }

                FormMethods.AddToFormCreationProtocol(
                    form,
                    $"[ActionButtonToFormAdder]: Added Button{parameter.Title}");

                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return "ActionButton: " + parameter.ActionName;
        }
    }
}