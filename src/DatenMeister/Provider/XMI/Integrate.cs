using Autofac;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Provider.XMI
{
    public class Integrate
    {
        public static void Into(ILifetimeScope scope)
        {
            var storageMap = scope.Resolve<IConfigurationToExtentStorageMapper>();
            ManualConfigurationToExtentStorageMapper.MapExtentLoaderType(storageMap, typeof(XmiStorage));

            var data = scope.Resolve<ExtentStorageData>();
            data.AdditionalTypes.Add(typeof(ExtentLoaderConfig));
            data.AdditionalTypes.Add(typeof(XmiStorageConfiguration));
        }
    }
}