using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Forms.FormFactory;

namespace DatenMeister.Forms.Helper;

/// <summary>
///     This class allows the adding of an action button to an existing form.
///     The action button is only added when the form is created for a certain metaclass.
/// </summary>
public static class ActionButtonToFormAdder
{
    /// <summary>
    /// Adds an action button to a rowform
    /// </summary>
    /// <param name="formsState">Defines the forms state in which the action button shall be added</param>
    /// <param name="adder">Parameter which contains the information about the action button</param>
    public static void AddRowActionButton(FormsState formsState, ActionButtonAdderParameterForRow adder)
    {
        formsState.FormModificationPlugins.Add(
            new FormModificationPlugin
            {
                CreateContext = context =>
                    context.Global.RowFormFactories.Add(
                        new RowAndTableFormModificationForActionButton(adder)
                        {
                            Priority = adder.Priority
                        }),
                Name = adder.Title
            });
    }
    
    /// <summary>
    /// Adds an action button to a rowform
    /// </summary>
    /// <param name="formsState">Defines the forms state in which the action button shall be added</param>
    /// <param name="multiActionButtonsConfig">Configuration of the action button set</param>
    /// <param name="adders">Parameters which contains the information about the action button</param>
    public static void AddRowActionButtons(
        FormsState formsState,
        MultiActionButtonsConfig multiActionButtonsConfig, 
        IEnumerable<ActionButtonAdderParameterForRow> adders)
    {
        formsState.FormModificationPlugins.Add(
            new FormModificationPlugin
            {
                CreateContext = context =>
                    context.Global.RowFormFactories.Add(
                        new RowAndTableFormModificationForMultipleActionButtons(multiActionButtonsConfig, adders)
                        {
                            Priority = multiActionButtonsConfig.Priority
                        }),
                Name = multiActionButtonsConfig.Title
            });
    }

    /// <summary>
    /// Adds an action button to a tableform
    /// </summary>
    /// <param name="formsState">Defines the forms state in which the action button shall be added</param>
    /// <param name="adder">Parameter which contains the information about the action button</param>
    public static void AddTableActionButton(FormsState formsState, ActionButtonAdderParameterForTable adder)
    {
        formsState.FormModificationPlugins.Add(
            new FormModificationPlugin
            {
                CreateContext = context =>
                    context.Global.TableFormFactories.Add(
                        new RowAndTableFormModificationForActionButton(adder)
                        {
                            Priority = adder.Priority
                        }),
                Name = adder.Title
            });
    }

    public class MultiActionButtonsConfig
    {
        public required string Name { get; set; }
        
        public required string Title { get; set; }
        
        public int Priority { get; set; }
    }

    private class RowAndTableFormModificationForMultipleActionButtons
        (MultiActionButtonsConfig configuration, IEnumerable<ActionButtonAdderParameter> parameters) 
        : FormFactoryBase, IRowFormFactory, ITableFormFactory
    {
        /// <summary>
        /// In case the action but is fitting to the constraints, we create the action button and add it
        /// to the field.  
        /// </summary>
        /// <param name="factoryParameter">Parameter which contain the information when the button shall
        /// be added</param>
        /// <param name="context">Context in which the button will be added</param>
        /// <param name="result">Here-in, the new fields will be added</param>
        private void ManageActionButtons(
            FormFactoryParameterBase factoryParameter,
            FormCreationContext context,
            FormCreationResultMultipleForms result)
        {
            var mergedActionField = context.Global.Factory.create(_Forms.TheOne.FieldTypes.__MergedFieldsInCellData);
            mergedActionField.set(_Forms._FieldTypes._MergedFieldsInCellData.name, configuration.Name);
            mergedActionField.set(_Forms._FieldTypes._MergedFieldsInCellData.title, configuration.Title);

            // Adds the field to the form
            var form = result.Forms.FirstOrDefault();
            if (form == null)
                throw new InvalidOperationException("Form is null");
            var fields = form.get<IReflectiveSequence>(_Forms._FormTypes._RowForm.field);
                fields.add(mergedActionField);
                
            // Now walk through all the parameters
            foreach(var parameter in parameters)
            {
                var actionField = 
                    RowAndTableFormModificationForActionButton.CreateActionFieldInCaseItIsValid(parameter, factoryParameter, context);
                if (actionField == null)
                {
                    continue;
                }
                
                mergedActionField.AddCollectionItem(_Forms._FieldTypes._MergedFieldsInCellData.fields, actionField);
                
                (parameter as ActionButtonAdderParameterForRow)?.OnCallSuccess?.Invoke(
                    factoryParameter as RowFormFactoryParameter
                    ?? throw new InvalidOperationException("RowFormFactoryParameter is null"));

                (parameter as ActionButtonAdderParameterForTable)?.OnCallSuccess?.Invoke(
                    factoryParameter as TableFormFactoryParameter
                    ?? throw new InvalidOperationException("TableFormFactoryParameter is null"));
            
                result.AddToFormCreationProtocol(
                    $"[ActionButtonToFormAdder]: Added Merged Button{parameter.Title}");
                
                result.IsManaged = true;
            }
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
                ManageActionButtons(factoryParameter, context, result);
            }
        }

        public void CreateTableForm(TableFormFactoryParameter factoryParameter, FormCreationContext context,
            FormCreationResultMultipleForms result)
        {
            if (context.IsInExtensionCreationMode())
            {
                ManageActionButtons(factoryParameter, context, result);
            }
        }
        
    }

    /// <summary>
    /// This is just a small helper class which adds the fields where necessary
    /// </summary>
    /// <param name="parameter">Parameter in which the buttons will be added</param>
    private class RowAndTableFormModificationForActionButton(ActionButtonAdderParameter parameter) : 
        FormFactoryBase, IRowFormFactory, ITableFormFactory
    {
        /// <summary>
        /// In case the action but is fitting to the constraints, we create the action button and add it
        /// to the field.  
        /// </summary>
        /// <param name="factoryParameter">Parameter which contain the information when the button shall
        /// be added</param>
        /// <param name="context">Context in which the button will be added</param>
        /// <param name="result">Here-in, the new fields will be added</param>
        private void ManageActionButton(
            FormFactoryParameterBase factoryParameter,
            FormCreationContext context,
            FormCreationResultMultipleForms result)
        {
            var actionField = CreateActionFieldInCaseItIsValid(parameter, factoryParameter, context);
            if (actionField == null)
            {
                return;
            }

            // Adds the field to the form
            var form = result.Forms.FirstOrDefault();
            if (form == null)
                throw new InvalidOperationException("Form is null");

            var fields = form.get<IReflectiveSequence>(_Forms._FormTypes._RowForm.field);
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

        internal static IElement? CreateActionFieldInCaseItIsValid(ActionButtonAdderParameter parameter,
            FormFactoryParameterBase factoryParameter, FormCreationContext context)
        {
            if (!ChecksValidity(parameter, factoryParameter))
            {
                // The parameter is not valid, so we would like to skip it
                return null;
            }

            return CreateActionField(parameter, context);
        }

        private static IElement CreateActionField(ActionButtonAdderParameter parameter, FormCreationContext context)
        {
            var actionField = context.Global.Factory.create(_Forms.TheOne.FieldTypes.__ActionFieldData);
            actionField.set(_Forms._FieldTypes._ActionFieldData.actionName, parameter.ActionName);
            actionField.set(_Forms._FieldTypes._ActionFieldData.title, parameter.Title);
            actionField.set(_Forms._FieldTypes._ActionFieldData.name, parameter.ActionName);

            if (!string.IsNullOrEmpty(parameter.ButtonText))
            {
                actionField.set(_Forms._FieldTypes._ActionFieldData.buttonText, parameter.ButtonText);
            }

            if (parameter.Parameter.Count > 0)
            {
                var parameter1 = context.Global.Factory.create(null);
                foreach (var (key, value) in parameter.Parameter)
                {
                    parameter1.set(key, value);
                }

                actionField.set(_Forms._FieldTypes._ActionFieldData.parameter, parameter1);
            }

            return actionField;
        }

        private static bool ChecksValidity(ActionButtonAdderParameter parameter, FormFactoryParameterBase factoryParameter)
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
                    return false;
                }

                if (!forRow.PredicateForParameter(rowParameter))
                {
                    // Skip it
                    return false;
                }
            }
            else if (parameter is ActionButtonAdderParameterForTable forTable
                     && factoryParameter is TableFormFactoryParameter tableParameter)
            {
                forTable.OnCall?.Invoke(tableParameter);
                
                if (!forTable.PredicateForParameter(tableParameter))
                {
                    // Skip it
                    return false;
                }
            }
            else
            {
                throw new InvalidOperationException("Unknown parameter combination: "
                                                    + parameter.GetType()
                                                    + " " + factoryParameter.GetType());
            }

            return true;
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