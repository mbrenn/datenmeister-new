﻿using DatenMeister.Core;
using DatenMeister.Core.Models;
using DatenMeister.Forms;
using DatenMeister.Forms.Helper;
using DatenMeister.Plugins;

namespace DatenMeister.Extent.Forms
{
    // ReSharper disable once UnusedType.Global
    /// <summary>
    /// Defines the default form extensions which are used to navigate through the
    /// items, extens and also offers the simple creation and deletion of items. 
    /// </summary>
    public class BasicNavigationForFormsAndItemsPlugin : IDatenMeisterPlugin
    {
        public const string NavigationItemDelete = "Item.Delete";
        public const string NavigationExtentsListViewItem = "ExtentsList.ViewItem";
        public const string NavigationExtentsListDeleteItem = "ExtentsList.DeleteItem";

        private readonly IScopeStorage _scopeStorage;

        public BasicNavigationForFormsAndItemsPlugin(IScopeStorage scopeStorage)
        {
            _scopeStorage = scopeStorage;
        }

        public void Start(PluginLoadingPosition position)
        {
            var formsPlugin = _scopeStorage.Get<FormsPluginState>();

            ActionButtonToFormAdder.AddActionButton(
                formsPlugin, new ActionButtonAdderParameter(NavigationItemDelete, "Delete Item")
                {
                    FormType = _DatenMeister._Forms.___FormType.TreeItemDetail
                });

            ActionButtonToFormAdder.AddActionButton(
                formsPlugin, new ActionButtonAdderParameter(NavigationExtentsListViewItem, "View Item")
                {
                    FormType = _DatenMeister._Forms.___FormType.ObjectList
                });

            ActionButtonToFormAdder.AddActionButton(
                formsPlugin, new ActionButtonAdderParameter(NavigationExtentsListDeleteItem, "Delete Item")
                {
                    FormType = _DatenMeister._Forms.___FormType.ObjectList
                });

            formsPlugin.FormModificationPlugins.Add(
                new CreateInstanceButtonsForListForms());
        }
    }
}