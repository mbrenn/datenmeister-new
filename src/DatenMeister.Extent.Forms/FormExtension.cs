using System;
using DatenMeister.Core;
using DatenMeister.Forms;
using DatenMeister.Modules.Forms;
using DatenMeister.Plugins;

namespace DatenMeister.Extent.Forms
{
    public class ExtentFormExtensionPlugin  : IDatenMeisterPlugin
    {
        private readonly IScopeStorage _scopeStorage;

        public ExtentFormExtensionPlugin(IScopeStorage scopeStorage)
        {
            _scopeStorage = scopeStorage;
        }

        public void Start(PluginLoadingPosition position)
        {
            var formsPlugin = _scopeStorage.Get<FormsPluginState>();
            formsPlugin.FormModificationPlugins.Add(new ExtentFormExtension());
        }
    }
}