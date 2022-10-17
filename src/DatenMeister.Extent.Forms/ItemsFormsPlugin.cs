using DatenMeister.Core;
using DatenMeister.Core.Models;
using DatenMeister.Forms;
using DatenMeister.Forms.Helper;
using DatenMeister.Plugins;

namespace DatenMeister.Extent.Forms
{
    // ReSharper disable once UnusedType.Global
    /// <summary>
    /// Defines the default form extensions which are used to navigate through the
    /// items also offers the simple creation and deletion of items. 
    /// </summary>
    public class ItemsFormsPlugin : IDatenMeisterPlugin
    {
        public const string NavigationItemDelete = "Item.Delete";
        public const string NavigationExtentsListViewItem = "ExtentsList.ViewItem";
        public const string NavigationExtentsListDeleteItem = "ExtentsList.DeleteItem";
        public const string NavigationExtentsListMoveUpItem = "ExtentsList.MoveUpItem";
        public const string NavigationExtentsListMoveDownItem = "ExtentsList.MoveDownItem";
        public const string NavigationItemMoveOrCopyNavigate = "Item.MoveOrCopy.Navigate";

        private readonly IScopeStorage _scopeStorage;

        public ItemsFormsPlugin(IScopeStorage scopeStorage)
        {
            _scopeStorage = scopeStorage;
        }

        public void Start(PluginLoadingPosition position)
        {
            switch (position)
            {
                case PluginLoadingPosition.AfterLoadingOfExtents:
                    
                    var formsPlugin = _scopeStorage.Get<FormsPluginState>();

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

                    formsPlugin.FormModificationPlugins.Add(
                        new CreateInstanceButtonsForTableForms());
                    break;
            }
        }
    }
}