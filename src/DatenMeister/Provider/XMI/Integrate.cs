using Autofac;
using DatenMeister.Provider.XMI.EMOF;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.FactoryMapper;

namespace DatenMeister.Provider.XMI
{
    public class Integrate
    {

        public static void Into(ILifetimeScope scope)
        {
            var factoryMapper = scope.Resolve<IFactoryMapper>();
            var storageMap = scope.Resolve<IConfigurationToExtentStorageMapper>();
            DefaultFactoryMapper.MapFactoryType(factoryMapper, typeof(XmlUriExtent));
            ManualConfigurationToExtentStorageMapper.MapExtentLoaderType(storageMap, typeof(XmiStorage));
        }
    }
}