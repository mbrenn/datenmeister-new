using System.Diagnostics;
using Autofac;
using DatenMeister.Core.Plugins;
using DatenMeister.Excel.Helper;
using DatenMeister.Integration;
using DatenMeister.Provider.CSV.Runtime;
using DatenMeister.Runtime.ExtentStorage;

namespace DatenMeister.Excel.Integration
{

    public class ExcelPlugin : IDatenMeisterPlugin
    {
        private readonly ExtentStorageData _storageData;

        public ExcelPlugin(ExtentStorageData storageData)
        {
            _storageData = storageData;
        }

        public void Start()
        {
            _storageData.AdditionalTypes.Add(typeof(ExcelReferenceSettings));
            _storageData.AdditionalTypes.Add(typeof(ExcelImportSettings));
            _storageData.AdditionalTypes.Add(typeof(ExcelExtentSettings));
        }
    }
}