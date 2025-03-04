﻿using DatenMeister.Core;
using DatenMeister.Forms;
using DatenMeister.Plugins;
using System.Threading.Tasks;

namespace DatenMeister.Extent.Forms
{
    public class DefaultTypesPlugin : IDatenMeisterPlugin
    {
        private readonly IScopeStorage _scopeStorage;

        public DefaultTypesPlugin(IScopeStorage scopeStorage)
        {
            _scopeStorage = scopeStorage;
        }

        public Task Start(PluginLoadingPosition position)
        {
            switch (position)
            {
                case PluginLoadingPosition.AfterLoadingOfExtents:

                    var formsPluginState = _scopeStorage.Get<FormsPluginState>();
                    formsPluginState.FormModificationPlugins.Add(
                        new PackageFormModificationPlugin());
                    break;
            }

            return Task.CompletedTask;
        }
    }
}