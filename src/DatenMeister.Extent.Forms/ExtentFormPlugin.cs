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
    /// items, extens and also offers the simple creation and deletion of items. 
    /// </summary>
    public class ExtentFormPlugin : IDatenMeisterPlugin
    {
        public const string NavigationExtentNavigateTo = "Extent.NavigateTo";
        public const string NavigationExtentDeleteExtent = "Extent.DeleteExtent";
        public const string NavigationExtentProperties = "Extent.Properties";
        public const string NavigationItemNew = "Item.New";

        private readonly IScopeStorage _scopeStorage;

        public ExtentFormPlugin(IScopeStorage scopeStorage)
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
                        formsPlugin, new ActionButtonAdderParameter(NavigationExtentNavigateTo, "View Items in Extent")
                        {
                            FormType = _DatenMeister._Forms.___FormType.Detail,
                            MetaClass = _DatenMeister.TheOne.Management.__Extent
                        });

                    ActionButtonToFormAdder.AddActionButton(
                        formsPlugin,
                        new ActionButtonAdderParameter(NavigationExtentProperties, "View Extent Properties")
                        {
                            FormType = _DatenMeister._Forms.___FormType.Detail,
                            MetaClass = _DatenMeister.TheOne.Management.__Extent
                        });

                    ActionButtonToFormAdder.AddActionButton(
                        formsPlugin, new ActionButtonAdderParameter(NavigationExtentDeleteExtent, "Delete Extent")
                        {
                            FormType = _DatenMeister._Forms.___FormType.Detail,
                            MetaClass = _DatenMeister.TheOne.Management.__Extent
                        });

                    ActionButtonToFormAdder.AddActionButton(
                        formsPlugin, new ActionButtonAdderParameter(NavigationItemNew, "New Item")
                        {
                            MetaClass = _DatenMeister.TheOne.Management.__Extent,
                            FormType = _DatenMeister._Forms.___FormType.TreeItemExtent
                        }
                    );

                    ActionButtonToFormAdder.AddActionButton(
                        formsPlugin,
                        new ActionButtonAdderParameter(NavigationExtentNavigateTo, "Items")
                        {
                            ParentMetaClass = _DatenMeister.TheOne.Management.__Workspace,
                            FormType = _DatenMeister._Forms.___FormType.ObjectList,
                            ParentPropertyName = _DatenMeister._Management._Workspace.extents,
                            ActionButtonPosition = 0
                        });
                    break;
            }
        }
    }
}