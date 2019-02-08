using DatenMeister.Provider;
using DatenMeister.Runtime.ExtentStorage.Configuration;

namespace DatenMeister.Runtime.ExtentStorage
{
    /// <summary>
    /// Defines the return information of the loading of a provider.
    /// The loader may add additional information to the
    /// structure to change the loader config or to perform other operation
    /// </summary>
    public class LoadedProviderInfo
    {
        public LoadedProviderInfo(IProvider provider = null, ExtentLoaderConfig config = null)
        {
            Provider = provider;
            UsedConfig = config;
        }

        /// <summary>
        /// Gets or sets the provider being loaded by the provider loader
        /// </summary>
        public IProvider Provider { get; set; }

        /// <summary>
        /// Gets or sets the config that shall be used in the future for the provider.
        /// It may be set by ProviderLoader which support the 'one-time'
        /// transformation of one datatype ot another. 
        /// </summary>
        public ExtentLoaderConfig UsedConfig { get; set; }

        /// <summary>
        /// Gets or sets the information whether the extent is already added to the workspace. If yes, then the ExtentLoader will not separately
        /// add the extent to the workspace, including the UsedConfig
        /// </summary>
        public bool IsExtentAlreadyAddedToWorkspace { get; set; }
    }
}