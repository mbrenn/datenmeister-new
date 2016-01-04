﻿namespace DatenMeister.Runtime.ExtentStorage.Interfaces
{
    /// <summary>
    /// Defines the storage configuration, which allows the extent getting loaded and saved. 
    /// The configuration has to be maintained during the runtime for loading and storing
    /// </summary>
    public class ExtentStorageConfiguration
    {
        /// <summary>
        /// Gets or sets the extent uri
        /// </summary>
        public string ExtentUri { get; set; }
    }
}