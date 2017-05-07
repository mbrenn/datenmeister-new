using DatenMeister.Core.Plugins;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage;

namespace DatenMeister.Provider.XMI
{
    /// <summary>
    /// This plugin is loaded during the bootup
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once InconsistentNaming
    public class XmiPlugin : IDatenMeisterPlugin
    {
        private readonly ExtentConfigurationLoader _extentLoader;

        public XmiPlugin(ExtentConfigurationLoader extentLoader)
        {
            _extentLoader = extentLoader;
        }

        public void Start()
        {
            _extentLoader.AddAdditionalType(typeof(XmiStorageConfiguration));
        }
    }
}