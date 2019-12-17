using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Plugins;

namespace DatenMeister.Excel.Integration
{

    public class ExcelPlugin : IDatenMeisterPlugin
    {
        private readonly IExtentManager _extentManager;

        public ExcelPlugin(IExtentManager extentManager)
        {
            _extentManager = extentManager;
        }

        public void Start(PluginLoadingPosition loadingPosition)
        {

        }
    }
}