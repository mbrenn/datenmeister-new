﻿using DatenMeister.Core;
using DatenMeister.Core.Models;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Plugins;

namespace DatenMeister.Provider.DynamicRuntime
{
    /// <summary>
    /// Defines the plugin for the dynamic runtime providers
    /// </summary>
    [PluginLoading(PluginLoadingPosition.AfterInitialization)]
    public class DynamicRuntimeProviderPlugin : IDatenMeisterPlugin
    {
        public DynamicRuntimeProviderPlugin(IScopeStorage scopeStorage)
        {
            ScopeStorage = scopeStorage;
        }

        public IScopeStorage ScopeStorage { get; }

        public void Start(PluginLoadingPosition position)
        {
            switch (position)
            {
                case PluginLoadingPosition.AfterInitialization:
                {
                    var mapper = ScopeStorage.Get<ProviderToProviderLoaderMapper>();
                    mapper.AddMapping(
                        _DatenMeister.TheOne.DynamicRuntimeProvider.__DynamicRuntimeLoaderConfig,
                        manager => new DynamicRuntimeProviderLoader());
                    break;
                }
            }
        }
    }
}