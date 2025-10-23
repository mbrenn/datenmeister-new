using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Core.EMOF.Implementation;

public static class ResolverHelper
{
    public static IElement? ResolveElement(this IUriResolver resolver, string uri, ResolveType resolveType, bool traceFailing = true)
    {
        return resolver.Resolve(uri, resolveType, traceFailing) as IElement;
    }
        
    /// <summary>
    /// Resolves the element if the element is just a mof shadow
    /// </summary>
    /// <param name="resolver">Resolver to be used </param>
    /// <param name="element">Element to be resolved. If this is directly an object, then the element will be
    /// returned directly. If it is a MofObjectShadow, then a full resolving will be performed</param>
    /// <param name="resolveType">Type of the resolving</param>
    /// <param name="traceFailing">true, if failed resolving shall be traced</param>
    /// <returns>Resolved element</returns>
    public static IElement? ResolveElement(this IUriResolver resolver, IObject element, ResolveType resolveType, bool traceFailing = true)
    {
        if (element is MofObjectShadow mofObjectShadow)
        {
            return resolver.Resolve(mofObjectShadow.Uri, resolveType, traceFailing) as IElement;
        }

        return element as IElement;
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