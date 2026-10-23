using System.Diagnostics;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms;
using DatenMeister.Forms.FormFactory;
using DatenMeister.Plugins;
using FormCreationContext = DatenMeister.Forms.FormCreationContext;

namespace DatenMeister.Actions.Forms;

/// <summary>
/// Defines the plugin which adds the Execute Action Button for all items belonging to actions 
/// </summary>
// ReSharper disable once UnusedType.Global
// ReSharper disable once ClassNeverInstantiated.Global
public class ActionFormPlugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    public Task Start(PluginLoadingPosition position)
    {
        switch (position)
        {
            case PluginLoadingPosition.AfterLoadingOfExtents:

                var formsPluginState = scopeStorage.Get<FormsState>();
                formsPluginState.FormModificationPlugins.Add(
                    new FormModificationPlugin
                    {
                        Name = "ActionFormPlugin",
                        CreateContext =
                            context => context.Global.RowFormFactories.Add(new ActionFormModification(workspaceLogic, scopeStorage))
                    });

                break;
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// The helper class which includes the action button into the forms for actions
    /// </summary>
    public class ActionFormModification (IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage): FormFactoryBase, IRowFormFactory
    {
        public void CreateRowForm(RowFormFactoryParameter parameter, FormCreationContext context, FormCreationResultMultipleForms result)
        {
            if (!context.IsInExtensionCreationMode())
                return;
            
            var metaClass = parameter.MetaClass;
            if (metaClass == null)
                return;
            
            var isAction = ClassifierMethods.IsSpecializedClassifierOf(
                metaClass, 
                _Actions.TheOne.__Action);
            if (!isAction)
            {
                return;
            }

            var actionLogic = new ActionLogic(workspaceLogic, scopeStorage);
            if (parameter.Element == null)
            {
                // There is no action to be added
                return;
            }

            string[] verbs;
            if (actionLogic.FindActionHandler(parameter.Element) is IActionHandlerWithVerbs actionHandler)
            {
                verbs = actionHandler.SupportedVerbs;
            }
            else
            {
                verbs = [ActionVerbs.Execute];
            }

            var form = result.Forms.FirstOrDefault();
            if (form == null)
            {
                throw new InvalidOperationException("No rowform has been created");
            }

            foreach(var verb in verbs)
            {
                // Fitting, create the field
                var fields = form.get<IReflectiveSequence>(_Forms._FormTypes._RowForm.field);
                var actionField = context.Global.Factory.create(_Forms.TheOne.FieldTypes.__ActionFieldData);
                actionField.set(_Forms._FieldTypes._ActionFieldData.actionName, "Action.Execute");
                actionField.set(_Forms._FieldTypes._ActionFieldData.title, "Execute Action");
                actionField.set(_Forms._FieldTypes._ActionFieldData.name, verb);
                var parameter1 = context.Global.Factory.create(null);
                parameter1.set("verb", verb);
                actionField.set(_Forms._FieldTypes._ActionFieldData.parameter, parameter1);
                fields.add(actionField);

                result.IsManaged = true;
            }
        }
    }

}