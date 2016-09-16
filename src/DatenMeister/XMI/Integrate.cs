using Autofac;
using DatenMeister.Core.EMOF.InMemory;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.FactoryMapper;
using DatenMeister.XMI.EMOF;
using DatenMeister.XMI.ExtentStorage;

namespace DatenMeister.XMI
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