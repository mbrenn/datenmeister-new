using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Plugins;

namespace DatenMeister.Excel.Integration
{

    public class ExcelPlugin : IDatenMeisterPlugin
    {
        private readonly ExtentManager _extentManager;

        public ExcelPlugin(ExtentManager extentManager)
        {
            _extentManager = extentManager;
        }

        public void Start(PluginLoadingPosition loadingPosition)
        {

        }
    }
}