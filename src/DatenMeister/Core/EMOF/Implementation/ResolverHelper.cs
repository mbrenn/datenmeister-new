using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.EMOF.Implementation
{
    public static class ResolverHelper
    {
        public static IElement? ResolveElement(this IUriResolver resolver, string uri, ResolveType resolveType, bool traceFailing = true)
        {
            return resolver.Resolve(uri, resolveType, traceFailing) as IElement;
        }
        
        public static IReflectiveCollection? ResolveCollection(this IUriResolver resolver, string uri, ResolveType resolveType, bool traceFailing = true)
        {
            return resolver.Resolve(uri, resolveType, traceFailing) as IReflectiveCollection;
        }
        
        public static IReflectiveSequence? ResolveSequence(this IUriResolver resolver, string uri, ResolveType resolveType, bool traceFailing = true)
        {
            return resolver.Resolve(uri, resolveType, traceFailing) as IReflectiveSequence;
        }
    }
}