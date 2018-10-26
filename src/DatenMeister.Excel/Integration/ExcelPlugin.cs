using System.Diagnostics;
using DatenMeister.Core.Plugins;

namespace DatenMeister.Excel.Integration
{

    public class ExcelPlugin : IDatenMeisterPlugin
    {
        public void Start()
        {
            Debug.WriteLine("Excel is now also in");
        }
    }
}