using DatenMeister.Core.Plugins;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

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