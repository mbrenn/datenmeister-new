using DatenMeister.Core.Plugins;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.XMI.ExtentStorage;

namespace DatenMeister.XMI
{
    /// <summary>
    /// This plugin is loaded during the bootup
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once InconsistentNaming
    public class XmiPlugin : IDatenMeisterPlugin
    {
        private readonly ExtentStorageConfigurationLoader _extentStorageLoader;

        public XmiPlugin(ExtentStorageConfigurationLoader extentStorageLoader)
        {
            _extentStorageLoader = extentStorageLoader;
        }

        public void Start()
        {
            _extentStorageLoader.AddAdditionalType(typeof(XmiStorageConfiguration));
        }
    }
}