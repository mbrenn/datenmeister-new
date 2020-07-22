﻿using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Plugins;

namespace DatenMeister.Provider.XMI
{
    /// <summary>
    /// This plugin is loaded during the bootup
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once InconsistentNaming
    [PluginLoading(PluginLoadingPosition.BeforeBootstrapping)]
    // ReSharper disable once UnusedMember.Global
    public class XmiPlugin : IDatenMeisterPlugin
    {
        private readonly IConfigurationToExtentStorageMapper _storageMapper;

        public XmiPlugin(IConfigurationToExtentStorageMapper storageMapper)
        {
            _storageMapper = storageMapper;
        }

        public void Start(PluginLoadingPosition position)
        {
            _storageMapper.MapExtentLoaderType(typeof(XmiProviderLoader));
        }
    }
}