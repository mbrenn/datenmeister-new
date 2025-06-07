using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms;
using DatenMeister.Forms.FormModifications;
using DatenMeister.Plugins;

namespace DatenMeister.Actions.Forms;

/// <summary>
/// Defines the plugin which adds the Execute Action Button for all items belonging to actions 
/// </summary>
// ReSharper disable once UnusedType.Global
// ReSharper disable once ClassNeverInstantiated.Global
public class ActionFormPlugin(IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    public Task Start(PluginLoadingPosition position)
    {
        switch (position)
        {
            case PluginLoadingPosition.AfterLoadingOfExtents:

                var formsPluginState = scopeStorage.Get<FormsState>();
                formsPluginState.FormModificationPlugins.Add(
                    new ActionFormModificationPlugin());
                break;
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// The helper class which includes the action button into the forms for actions
    /// </summary>
    public class ActionFormModificationPlugin : IFormModificationPlugin
    {
        public bool ModifyForm(FormCreationContext context, IElement form)
        {
            var isAction = ClassifierMethods.IsSpecializedClassifierOf(
                context.MetaClass, _Actions.TheOne.__Action);

            if (isAction
                && context.FormType == _Forms.___FormType.Row
                && context.ParentPropertyName == string.Empty
                && context.DetailElement != null)
            {
                // Fitting, create the field
                var fields = form.get<IReflectiveSequence>(_Forms._RowForm.field);
                var actionField = MofFactory.CreateElement(form, _Forms.TheOne.__ActionFieldData);
                actionField.set(_Forms._ActionFieldData.actionName, "Action.Execute");
                actionField.set(_Forms._ActionFieldData.title, "Execute Action");
                actionField.set(_Forms._ActionFieldData.name, "Execute");
                fields.add(actionField);

                return true;
            }

            return false;
        }
    }

}