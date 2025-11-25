using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Common;
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
public class ActionFormPlugin(IScopeStorage scopeStorage) : IDatenMeisterPlugin
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
                            context => context.Global.RowFormFactories.Add(new ActionFormModificationPlugin())
                    });

                break;
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// The helper class which includes the action button into the forms for actions
    /// </summary>
    public class ActionFormModificationPlugin : FormFactoryBase, IRowFormFactory
    {
        public void CreateRowForm(RowFormFactoryParameter parameter, FormCreationContext context, FormCreationResultMultipleForms result)
        {
            var metaClass = parameter.MetaClass;
            if (metaClass == null)
                return;
            
            var isAction = ClassifierMethods.IsSpecializedClassifierOf(
                metaClass, _Actions.TheOne.__Action);

            var form = result.Forms.FirstOrDefault();
            if (form == null)
            {
                throw new InvalidOperationException("No rowform has been created");
            }

            if (isAction)
            {
                // Fitting, create the field
                var fields = form.get<IReflectiveSequence>(_Forms._RowForm.field);
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