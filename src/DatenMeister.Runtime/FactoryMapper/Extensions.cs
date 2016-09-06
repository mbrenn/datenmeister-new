using Autofac;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

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