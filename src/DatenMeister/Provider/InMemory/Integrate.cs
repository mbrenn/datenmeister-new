using Autofac;
using DatenMeister.Runtime.FactoryMapper;

namespace DatenMeister.Provider.InMemory
{
    /// <summary>
    /// Performs the integration into the Datenmeister
    /// </summary>
    public static class Integrate
    {
        public static void Into(ILifetimeScope scope)
        {
            var factoryMapper = scope.Resolve<IFactoryMapper>();
            DefaultFactoryMapper.MapFactoryType(factoryMapper, typeof(InMemoryUriExtent));
        }
    }
}