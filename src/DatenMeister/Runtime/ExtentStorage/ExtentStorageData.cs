﻿#nullable enable

using System;
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
        /// Gets or sets the flag whether the extent is opened
        /// </summary>
        public bool IsOpened { get; set; }
        
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
        internal List<LoadedExtentInformation> LoadedExtents { get; } = new List<LoadedExtentInformation>();

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
        internal class LoadedExtentInformation
        {
            public IUriExtent Extent { get; set; }
            
            public ExtentLoaderConfig Configuration { get; set; }

            /// <summary>
            /// Initializes a new instance of the  LoadedExtentInformation class
            /// </summary>
            /// <param name="extent">Extent to be connected to</param>
            /// <param name="configuration">Configuration of the loading extent</param>
            public LoadedExtentInformation(IUriExtent extent, ExtentLoaderConfig configuration)
            {
                Extent = extent;
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
}