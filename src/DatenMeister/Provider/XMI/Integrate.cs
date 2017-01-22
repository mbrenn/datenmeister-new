using Autofac;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Provider.XMI
{
    public class Integrate
    {
        public static void Into(ILifetimeScope scope)
        {
            var storageMap = scope.Resolve<IConfigurationToExtentStorageMapper>();
            ManualConfigurationToExtentStorageMapper.MapExtentLoaderType(storageMap, typeof(XmiStorage));
        }
    }
}