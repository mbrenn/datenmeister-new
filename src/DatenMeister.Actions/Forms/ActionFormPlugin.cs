using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms;
using DatenMeister.Forms.FormFactory;
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
                formsPluginState.NewFormModificationPlugins.Add(
                    new NewFormModificationPlugin
                    {
                        Name = "ActionFormPlugin",
                        CreateContext =
                            context => context.Global.RowFormFactories.Add(new ActionFormModificationPlugin())
                    });

                break;
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// The helper class which includes the action button into the forms for actions
    /// </summary>
    public class ActionFormModificationPlugin : INewRowFormFactory
    {
        public void CreateRowFormForItem(IObject element, NewFormCreationContext context, FormCreationResult result)
        {
        }

        public void CreateRowFormForMetaClass(IElement metaClass, NewFormCreationContext context, FormCreationResult result)
        {
            var isAction = ClassifierMethods.IsSpecializedClassifierOf(
                metaClass, _Actions.TheOne.__Action);

            if (result.Form == null)
            {
                throw new InvalidOperationException("Form is null");
            }

            if (isAction)
            {
                // Fitting, create the field
                var fields = result.Form.get<IReflectiveSequence>(_Forms._RowForm.field);
                var actionField = context.Global.Factory.create(_Forms.TheOne.__ActionFieldData);
                actionField.set(_Forms._ActionFieldData.actionName, "Action.Execute");
                actionField.set(_Forms._ActionFieldData.title, "Execute Action");
                actionField.set(_Forms._ActionFieldData.name, "Execute");
                fields.add(actionField);

                result.IsManaged = true;
            }
        }
    }

}