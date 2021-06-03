#nullable enable

using System;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Extent.Manager.ExtentStorage
{
    /// <summary>
    /// Stores the configuration for the loaded extents during the runtime
    /// </summary>
    public class ExtentStorageData
    {
        /// <summary>
        /// Gets or sets the flag whether the extent is opened
        /// </summary>
        public bool IsOpened { get; set; }

        /// <summary>
        /// Gets or sets the value whether the extent manager uses a registration file. 
        /// If the Extent Manager is just opened via OpenDecoupled, then all the changes are not registered
        /// in the global configuration file
        /// </summary>
        public bool IsRegistrationOpen { get; set; }
        
        /// <summary>
        /// Gets or sets the information whether the loading has failed
        /// </summary>
        public bool FailedLoading { get; set; }
        
        /// <summary>
        /// Gets or sets the exception that has occured during the loading
        /// </summary>
        public Exception? FailedLoadingException { get; set; }

        /// <summary>
        /// Gets the enumeration of extents which failed to load
        /// </summary>
        public IEnumerable<string> FailedLoadingExtents { get; set; } = new List<string>();
        
        /// <summary>
        /// Stores the loaded extents including the configuration of the storage for the extent
        /// </summary>
        internal List<LoadedExtentInformation> LoadedExtents { get; } = new();

        /// <summary>
        /// Gets or sets the path in which the extent loading info is stored
        /// </summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// Gets the locking path to lock the extent registration
        /// </summary>
        public string GetLockPath()
        {

            if (string.IsNullOrEmpty(FilePath))
            {
                throw new InvalidOperationException(
                    "The locking path could not be retrieved because the configuration is empty. ");
            }

            return FilePath + ".lock";
        }

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
        }
    }

    /// <summary>
    /// Defines the possible states of loading of the extent
    /// </summary>
    public enum ExtentLoadingState
    {
        /// <summary>
        /// The loading state is unknown. Should never be set
        /// </summary>
        Unknown,

        /// <summary>
        /// The extent is not loaded
        /// </summary>
        Unloaded,
            
        /// <summary>
        /// The extent is loaded
        /// </summary>
        Loaded,
            
        /// <summary>
        /// The loading of the extent has failed and saving will not be performed
        /// </summary>
        Failed,
        
        /// <summary>
        /// The loading succeeded but we are in read-only mode
        /// </summary>
        LoadedReadOnly
    }
}