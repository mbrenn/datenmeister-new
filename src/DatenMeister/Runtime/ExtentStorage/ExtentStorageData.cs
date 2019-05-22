using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Runtime.ExtentStorage.Configuration;

namespace DatenMeister.Runtime.ExtentStorage
{
    /// <summary>
    /// Stores the configuration for the loaded extents during the runtime
    /// </summary>
    public class ExtentStorageData
    {
        /// <summary>
        /// Gets or sets the information whether the loading has failed
        /// </summary>
        public bool FailedLoading { get; set; }

        /// <summary>
        /// Stores the loaded extents including the configuration of the storage for the extent
        /// </summary>
        internal List<LoadedExtentInformation> LoadedExtents { get; } = new List<LoadedExtentInformation>();

        /// <summary>
        /// Gets or sets the path in which the extent loading info is stored
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Defines the class which stores the mapping between the extent and the configuration
        /// </summary>
        internal class LoadedExtentInformation
        {
            public IUriExtent Extent { get; set; }

            public ExtentLoaderConfig Configuration { get; set; }
        }
    }
}