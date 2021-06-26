﻿using System;
using DatenMeister.Core;
using DatenMeister.Core.Models;
using DatenMeister.Forms;
using DatenMeister.Forms.Helper;
using DatenMeister.Modules.Forms;
using DatenMeister.Plugins;

namespace DatenMeister.Extent.Forms
{
    // ReSharper disable once UnusedType.Global
    public class ExtentFormExtensionPlugin  : IDatenMeisterPlugin
    {
        public const string NavigationExtentNavigateTo = "Extent.NavigateTo";
        public const string NavigationItemDelete = "Item.Delete";
        public const string NavigationItemNew = "Item.New";
        public const string NavigationExtentsListViewItem = "ExtentsList.ViewItem";
        public const string NavigationExtentsListDeleteItem = "ExtentsList.DeleteItem";
        
        private readonly IScopeStorage _scopeStorage;

        public ExtentFormExtensionPlugin(IScopeStorage scopeStorage)
        {
            _scopeStorage = scopeStorage;
        }

        public void Start(PluginLoadingPosition position)
        {
            var formsPlugin = _scopeStorage.Get<FormsPluginState>();
            
            ActionButtonToFormAdder.AddActionButton(
                formsPlugin, new ActionButtonAdderParameter(NavigationExtentNavigateTo, "View Extent")
                    { MetaClass = _DatenMeister.TheOne.Management.__Extent}
                );

            ActionButtonToFormAdder.AddActionButton(
                formsPlugin, new ActionButtonAdderParameter(NavigationItemDelete, "Delete Item")
            {
                FormType = _DatenMeister._Forms.___FormType.TreeItemDetail
            });
            
            
            ActionButtonToFormAdder.AddActionButton(
                formsPlugin, new ActionButtonAdderParameter(NavigationItemNew, "New Item")
                {
                    MetaClass = _DatenMeister.TheOne.Management.__Extent,
                    FormType = _DatenMeister._Forms.___FormType.TreeItemExtent
                }
            );
            
            ActionButtonToFormAdder.AddActionButton(
                formsPlugin, new ActionButtonAdderParameter(NavigationExtentsListViewItem, "View Item")
                {
                    FormType = _DatenMeister._Forms.___FormType.TreeItemExtent
                });
            
            ActionButtonToFormAdder.AddActionButton(
                formsPlugin, new ActionButtonAdderParameter(NavigationExtentsListDeleteItem, "Delete Item")
                {
                    FormType = _DatenMeister._Forms.___FormType.TreeItemExtent
                });
        }
    }
}