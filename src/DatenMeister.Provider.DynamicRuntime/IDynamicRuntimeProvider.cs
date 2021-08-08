using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Provider;

namespace DatenMeister.Provider.DynamicRuntime
{
    /// <summary>
    /// Defines the interface to support the dynamic runtime providers
    /// </summary>
    public interface IDynamicRuntimeProvider
    {
        /// <summary>
        /// Gets the enumeration of provider objects of the dynamic runtime provider
        /// </summary>
        /// <param name="configuration">Configuration element to be used to define the properties</param>
        /// <returns>Enumeration of provider objects</returns>
        IEnumerable<IProviderObject> Get(DynamicRuntimeProviderWrapper wrapper, IElement? configuration);
    }
}
