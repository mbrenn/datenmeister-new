using DatenMeister.Core;
using DatenMeister.Core.Models;
using DatenMeister.Forms;
using DatenMeister.Forms.Helper;
using DatenMeister.Plugins;

namespace DatenMeister.Extent.Forms;

// ReSharper disable once UnusedType.Global
/// <summary>
/// Defines the default form extensions which are used to navigate through the
/// items also offers the simple creation and deletion of items. 
/// </summary>
public class ItemsFormsPlugin(IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    public const string NavigationItemDelete = "Item.Delete";
    public const string NavigationItemPropertyMoveUpItem = "Item.MoveUpItem";
    public const string NavigationItemPropertyMoveDownItem = "Item.MoveDownItem";
    public const string NavigationExtentsListViewItem = "ExtentsList.ViewItem";
    public const string NavigationExtentsListDeleteItem = "ExtentsList.DeleteItem";
    public const string NavigationExtentsListMoveUpItem = "ExtentsList.MoveUpItem";
    public const string NavigationExtentsListMoveDownItem = "ExtentsList.MoveDownItem";
    public const string NavigationItemMoveOrCopyNavigate = "Item.MoveOrCopy.Navigate";
    public const string NavigationExportXmi = "Item.ExportXmi.Navigate";
    public const string NavigationImportXmi = "Item.ImportXmi.Navigate";

    public Task Start(PluginLoadingPosition position)
    {
        switch (position)
        {
            case PluginLoadingPosition.AfterLoadingOfExtents:
                    
                var formsPlugin = scopeStorage.Get<FormsPluginState>();

                ActionButtonToFormAdder.AddActionButton(
                    formsPlugin, new ActionButtonAdderParameter(NavigationItemDelete, "Delete Item")
                    {
                        FormType = _DatenMeister._Forms.___FormType.Row
                    });
                    

                ActionButtonToFormAdder.AddActionButton(
                    formsPlugin, new ActionButtonAdderParameter(NavigationItemMoveOrCopyNavigate, "Move/Copy")
                    {
                        FormType = _DatenMeister._Forms.___FormType.Row
                    });

                ActionButtonToFormAdder.AddActionButton(
                    formsPlugin,
                    new ActionButtonAdderParameter(NavigationExportXmi, "Export", "Export Item as Xmi")
                    {
                        FormType = _DatenMeister._Forms.___FormType.Row
                    });

                ActionButtonToFormAdder.AddActionButton(
                    formsPlugin,
                    new ActionButtonAdderParameter(NavigationImportXmi, "Import", "Import Item as Xmi")
                    {
                        FormType = _DatenMeister._Forms.___FormType.Row
                    });

                ActionButtonToFormAdder.AddActionButton(
                    formsPlugin, new ActionButtonAdderParameter(NavigationExtentsListViewItem, "View Item")
                    {
                        ActionButtonPosition = 0, 
                        FormType = _DatenMeister._Forms.___FormType.Table
                    });
                    

                ActionButtonToFormAdder.AddActionButton(
                    formsPlugin, new ActionButtonAdderParameter(NavigationExtentsListMoveUpItem, "⬆️")
                    {
                        FormType = _DatenMeister._Forms.___FormType.Table,
                        ParentMetaClass = _DatenMeister.TheOne.Management.__Extent
                    });

                ActionButtonToFormAdder.AddActionButton(
                    formsPlugin, new ActionButtonAdderParameter(NavigationExtentsListMoveDownItem, "⬇️")
                    {
                        FormType = _DatenMeister._Forms.___FormType.Table,
                        ParentMetaClass = _DatenMeister.TheOne.Management.__Extent
                    });

                ActionButtonToFormAdder.AddActionButton(
                    formsPlugin, new ActionButtonAdderParameter(NavigationExtentsListDeleteItem, "❌")
                    {
                        FormType = _DatenMeister._Forms.___FormType.Table,
                        ParentMetaClass = _DatenMeister.TheOne.Management.__Extent
                    });

                ActionButtonToFormAdder.AddActionButton(
                    formsPlugin, new ActionButtonAdderParameter(NavigationItemPropertyMoveUpItem, "⬆️I")
                    {
                        FormType = _DatenMeister._Forms.___FormType.Table,
                        PredicateForContext = x => !string.IsNullOrEmpty(x.ParentPropertyName),
                        IsReadOnly = false
                    });

                ActionButtonToFormAdder.AddActionButton(
                    formsPlugin, new ActionButtonAdderParameter(NavigationItemPropertyMoveDownItem, "⬇️I")
                    {
                        FormType = _DatenMeister._Forms.___FormType.Table,
                        PredicateForContext = x => !string.IsNullOrEmpty(x.ParentPropertyName),
                        IsReadOnly = false
                    });

                ActionButtonToFormAdder.AddActionButton(
                    formsPlugin, new ActionButtonAdderParameter(NavigationItemDelete, "❌I")
                    {
                        FormType = _DatenMeister._Forms.___FormType.Table,
                        PredicateForContext = x => !string.IsNullOrEmpty(x.ParentPropertyName),
                        IsReadOnly = false
                    });

                formsPlugin.FormModificationPlugins.Add(
                    new CreateInstanceButtonsForTableForms());
                break;
        }

        return Task.CompletedTask;
    }
}