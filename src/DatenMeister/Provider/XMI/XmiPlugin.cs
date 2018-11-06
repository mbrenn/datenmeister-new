using DatenMeister.Core.Plugins;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Provider.XMI
{
    /// <summary>
    /// This plugin is loaded during the bootup
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once InconsistentNaming
    [PluginLoading(PluginLoadingPosition.BeforeBootstrapping)]
    public class XmiPlugin : IDatenMeisterPlugin
    {
        private readonly IExtentManager _extentManager;
        private readonly IConfigurationToExtentStorageMapper _storageMapper;

        public XmiPlugin(IExtentManager extentManager, IConfigurationToExtentStorageMapper storageMapper)
        {
            _extentManager = extentManager;
            _storageMapper = storageMapper;
        }

        public void Start(PluginLoadingPosition position)
        {
            ManualConfigurationToExtentStorageMapper.MapExtentLoaderType(_storageMapper, typeof(XmiStorage));
            
            _extentManager.AddAdditionalType(typeof(ExtentLoaderConfig));
            _extentManager.AddAdditionalType(typeof(XmiStorageConfiguration));
        }
    }
}