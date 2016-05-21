using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.EMOF.Proxy
{
    public interface IHasProxiedObject
    {
        /// <summary>
        /// Gets the proxied element which can be used to dereference the 
        /// content
        /// </summary>
        /// <returns>Returns the proxied element</returns>
        IObject GetProxiedElement();
    }
}