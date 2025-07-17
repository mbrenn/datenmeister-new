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

                // TODO: Make that the filters are working
                var formsPlugin = scopeStorage.Get<FormsState>();

                ActionButtonToFormAdder.AddRowActionButton(
                    formsPlugin, new ActionButtonAdderParameter(NavigationItemDelete, "Delete Item"));


                ActionButtonToFormAdder.AddRowActionButton(
                    formsPlugin, new ActionButtonAdderParameter(NavigationItemMoveOrCopyNavigate, "Move/Copy"));

                ActionButtonToFormAdder.AddRowActionButton(
                    formsPlugin,
                    new ActionButtonAdderParameter(NavigationExportXmi, "Export", "Export Item as Xmi"));

                ActionButtonToFormAdder.AddRowActionButton(
                    formsPlugin,
                    new ActionButtonAdderParameter(NavigationImportXmi, "Import", "Import Item as Xmi"));

                ActionButtonToFormAdder.AddTableActionButton(
                    formsPlugin, new ActionButtonAdderParameter(NavigationExtentsListViewItem, "View Item")
                    {
                        ActionButtonPosition = 0
                    });


                ActionButtonToFormAdder.AddTableActionButton(
                    formsPlugin, new ActionButtonAdderParameter(NavigationExtentsListMoveUpItem, "⬆️")
                    {
                        // ParentMetaClass = _Management.TheOne.__Extent
                    });

                ActionButtonToFormAdder.AddTableActionButton(
                    formsPlugin, new ActionButtonAdderParameter(NavigationExtentsListMoveDownItem, "⬇️")
                    {
                        // ParentMetaClass = _Management.TheOne.__Extent
                    });

                ActionButtonToFormAdder.AddTableActionButton(
                    formsPlugin, new ActionButtonAdderParameter(NavigationExtentsListDeleteItem, "❌")
                    {
                        // ParentMetaClass = _Management.TheOne.__Extent
                    });

                ActionButtonToFormAdder.AddTableActionButton(
                    formsPlugin, new ActionButtonAdderParameter(NavigationItemPropertyMoveUpItem, "⬆️I")
                    {
                        // PredicateForContext = x => !string.IsNullOrEmpty(x.ParentPropertyName),
                        // IsReadOnly = false
                    });

                ActionButtonToFormAdder.AddTableActionButton(
                    formsPlugin, new ActionButtonAdderParameter(NavigationItemPropertyMoveDownItem, "⬇️I")
                    {
                        // PredicateForContext = x => !string.IsNullOrEmpty(x.ParentPropertyName),
                        // IsReadOnly = false
                    });

                ActionButtonToFormAdder.AddTableActionButton(
                    formsPlugin, new ActionButtonAdderParameter(NavigationItemDelete, "❌I")
                    {
                        // PredicateForContext = x => !string.IsNullOrEmpty(x.ParentPropertyName),
                        // IsReadOnly = false
                    });

                formsPlugin.FormModificationPlugins.Add(
                    new FormModificationPlugin
                    {
                        CreateContext =context =>
                            context.Global.TableFormFactories.Add(new CreateInstanceButtonsForTableForms()),
                        Name = "CreateInstanceButtonsForTableForms"
                    }
                );
                
                break;
        }

        return Task.CompletedTask;
    }
}