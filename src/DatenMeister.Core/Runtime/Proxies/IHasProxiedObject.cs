using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Core.Runtime.Proxies;

public interface IHasProxiedObject
{
    /// <summary>
    /// Gets the proxied element which can be used to dereference the
    /// content
    /// </summary>
    /// <returns>Returns the proxied element</returns>
    IObject GetProxiedElement();
}