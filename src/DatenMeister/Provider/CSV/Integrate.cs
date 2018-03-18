using Autofac;
using DatenMeister.Modules.ZipExample;
using DatenMeister.Provider.CSV.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.UserInteractions;

namespace DatenMeister.Provider.CSV
{
    /// <summary>
    /// Performs the integration into the Datenmeister
    /// </summary>
    public static class Integrate
    {
        public static void Into(ILifetimeScope scope)
        {
            var storageMap = scope.Resolve<IConfigurationToExtentStorageMapper>();
            ManualConfigurationToExtentStorageMapper.MapExtentLoaderType(storageMap, typeof(CSVExtentLoader));

            var data = scope.Resolve<ExtentStorageData>();
            data.AdditionalTypes.Add(typeof(CSVExtentLoaderConfig));
        }
    }
}