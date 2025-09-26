using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Forms.FormFactory;

namespace DatenMeister.Forms.Helper;

/// <summary>
///     This class allows the adding of an action button to an existing form.
///     The action button is only added when the form is created for a certain metaclass.
/// </summary>
public static class ActionButtonToFormAdder
{
    public static void AddRowActionButton(FormsState formsState, ActionButtonAdderParameterForRow adder)
    {
        formsState.FormModificationPlugins.Add(
            new FormModificationPlugin
            {
                CreateContext = context =>
                    context.Global.RowFormFactories.Add(
                        new RowAndTableFormModification(adder)
                        {
                            Priority = adder.Priority
                        }),
                Name = adder.Title
            });
    }

    public static void AddTableActionButton(FormsState formsState, ActionButtonAdderParameterForTable adder)
    {
        formsState.FormModificationPlugins.Add(
            new FormModificationPlugin
            {
                CreateContext = context =>
                    context.Global.TableFormFactories.Add(
                        new RowAndTableFormModification(adder)
                        {
                            Priority = adder.Priority
                        }),
                Name = adder.Title
            });
    }

    private class RowAndTableFormModification(ActionButtonAdderParameter parameter) : 
        FormFactoryBase, IRowFormFactory, ITableFormFactory
    {
        private void ManageActionButton(
            FormFactoryParameterBase factoryParameter,
            FormCreationContext context,
            FormCreationResultMultipleForms result)
        {
            if (parameter is ActionButtonAdderParameterForRow forRow
                && factoryParameter is RowFormFactoryParameter rowParameter)
            {
                forRow.OnCall?.Invoke(rowParameter);
                var element = rowParameter.Element;

                if (element != null && parameter.PredicateForElement != null &&
                    parameter.PredicateForElement(element) == false)
                {
                    // Not predicated
                    return;
                }

                if (!forRow.PredicateForParameter(rowParameter))
                {
                    // Skip it
                    return;
                }
            }
            else if (parameter is ActionButtonAdderParameterForTable forTable
                     && factoryParameter is TableFormFactoryParameter tableParameter)
            {
                forTable.OnCall?.Invoke(tableParameter);
                
                if (!forTable.PredicateForParameter(tableParameter))
                {
                    // Skip it
                    return;
                }
            }
            else
            {
                throw new InvalidOperationException("Unknown parameter combination: "
                                                    + parameter.GetType()
                                                    + " " + factoryParameter.GetType());
            }

            var form = result.Forms.FirstOrDefault();
            if (form == null)
                throw new InvalidOperationException("Form is null");


            var fields = form.get<IReflectiveSequence>(_Forms._RowForm.field);
            var actionField = context.Global.Factory.create(_Forms.TheOne.__ActionFieldData);
            actionField.set(_Forms._ActionFieldData.actionName, parameter.ActionName);
            actionField.set(_Forms._ActionFieldData.title, parameter.Title);
            actionField.set(_Forms._ActionFieldData.name, parameter.ActionName);

            if (!string.IsNullOrEmpty(parameter.ButtonText))
            {
                actionField.set(_Forms._ActionFieldData.buttonText, parameter.ButtonText);
            }

            if (parameter.Parameter.Count > 0)
            {
                var parameter1 = context.Global.Factory.create(null);
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

            (parameter as ActionButtonAdderParameterForRow)?.OnCallSuccess?.Invoke(
                factoryParameter as RowFormFactoryParameter
                ?? throw new InvalidOperationException("RowFormFactoryParameter is null"));

            (parameter as ActionButtonAdderParameterForTable)?.OnCallSuccess?.Invoke(
                factoryParameter as TableFormFactoryParameter
                ?? throw new InvalidOperationException("TableFormFactoryParameter is null"));
            
            result.AddToFormCreationProtocol(
                $"[ActionButtonToFormAdder]: Added Button{parameter.Title}");

            result.IsManaged = true;
        }

        /// <summary>
        /// Modifies the form by checking whether the action button can be applied
        /// </summary>
        /// <param name="factoryParameter">The factory parameter being used</param>
        /// <param name="context">The form creation context in which the application shall be checked. This is
        /// the specific instance of the requesting form context</param>
        /// <param name="result">The form to which the changes shall be applied</param>
        /// <returns>true, if the form has been modified</returns>
        public void CreateRowForm(RowFormFactoryParameter factoryParameter, FormCreationContext context, FormCreationResultMultipleForms result)
        {
            if (context.IsInExtensionCreationMode())
            {
                ManageActionButton(factoryParameter, context, result);
            }
        }

        public void CreateTableForm(TableFormFactoryParameter factoryParameter, FormCreationContext context,
            FormCreationResultMultipleForms result)
        {
            if (context.IsInExtensionCreationMode())
            {
                ManageActionButton(factoryParameter, context, result);
            }
        }
    }
}