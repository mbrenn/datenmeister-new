using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Provider;

namespace DatenMeister.Core.Provider.Interfaces;

/// <summary>
/// Defines the interface that the provider also supports the reloading of the content. 
/// 
/// </summary>
public interface IProviderLoaderSupportsReload
{
    /// <summary>
    /// Performs the reloading. The reloading must be done in a way that the switch from the old 
    /// data to the new data is performed atomically. No or creation of data shall be done with old 
    /// data or even lead to corrupted information. 
    /// </summary>
    /// <param name="provider">The extent which shall be reloaded</param>
    /// <param name="configuration">Configuration from whith the data has been loaded</param>
    /// <returns>Task for asynchronous operation</returns>
    Task Reload(IProvider provider, IElement configuration);
}