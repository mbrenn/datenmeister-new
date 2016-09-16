using Autofac;
using DatenMeister.Core.EMOF.InMemory;
using DatenMeister.CSV.Runtime.Storage;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.FactoryMapper;

namespace DatenMeister.Core.EMOF
{
    /// <summary>
    /// Performs the integration into the Datenmeister
    /// </summary>
    public static class Integrate
    {
        public static void Into(ILifetimeScope scope)
        {
            var factoryMapper = scope.Resolve<IFactoryMapper>();
            DefaultFactoryMapper.MapFactoryType(factoryMapper, typeof(MofUriExtent));
        }
    }
}