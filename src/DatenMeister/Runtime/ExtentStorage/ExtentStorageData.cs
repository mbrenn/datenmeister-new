﻿using System;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Runtime.ExtentStorage.Configuration;

namespace DatenMeister.Runtime.ExtentStorage
{
    /// <summary>
    /// Stores the configuration during the run-time. 
    /// </summary>
    public class ExtentStorageData
    {
        /// <summary>
        /// Stores the loaded extents 
        /// </summary>
        internal List<LoadedExtentInformation> LoadedExtents { get; } = new List<LoadedExtentInformation>();

        public List<Type> AdditionalTypes { get; } = new List<Type>();

        /// <summary>
        /// Defines the class which stores the mapping between the extent and the configuration
        /// </summary>
        internal class LoadedExtentInformation
        {
            public IUriExtent Extent { get; set; }

            public ExtentStorageConfiguration Configuration { get; set; }
        }
    }
}