using Autofac;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.FactoryMapper
{
    public static class Extensions
    {
        public static IFactory FindFactoryFor(this IFactoryMapper mapper, ILifetimeScope scope, IUriExtent extent)
        {
            return mapper.FindFactoryFor(scope, extent.GetType());
        }
    }
}