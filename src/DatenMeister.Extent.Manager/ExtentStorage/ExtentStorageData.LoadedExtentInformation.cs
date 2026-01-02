using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Extent.Manager.ExtentStorage;

public partial class ExtentStorageData
{
    /// <summary>
    /// Defines the class which stores the mapping between the extent and the configuration
    /// </summary>
    public class LoadedExtentInformation
    {
        public IUriExtent? Extent { get; set; }
            
        public IElement Configuration { get; set; }

        public ExtentLoadingState LoadingState { get; set; } = ExtentLoadingState.Unloaded;

        /// <summary>
        /// Gets or sets the message why the loading of the extent has failed
        /// </summary>
        public string FailLoadingMessage { get; set; } = string.Empty;

        public bool IsExtentAddedToWorkspace { get; set; }

        /// <summary>
        /// Defines the value whether this extent shall be regarded as a persistent extent
        /// which will be stored via 'StoreConfiguration', so the extent will be reloaded
        /// when the ExtentManagers is requested to load all configuration.
        /// This configuration is especially useful when an extent is added dynamically by a plugin. 
        /// </summary>
        public bool IsExtentPersistent { get; set; } = true;

        /// <summary>
        /// Initializes a new instance of the  LoadedExtentInformation class
        /// </summary>
        /// <param name="configuration">Configuration of the loading extent</param>
        public LoadedExtentInformation(IElement configuration)
        {
            Configuration = configuration;
        }

        public override string ToString()
        {
            if (Extent == null)
            {
                return "(no extent): " + Configuration;
            }

            return $"({Extent}): " + Configuration;
        }

        /// <summary>
        /// Stores a shadow configuration for non persistent extents which can be used to check
        /// whether an extent is added to extent manager non-persistently. 
        /// </summary>
        public static IElement ShadowConfigurationForNonPersisten =
            new MofObjectShadow("dm:///internal/shadow_configuration_for_non_persistent");
    }
}