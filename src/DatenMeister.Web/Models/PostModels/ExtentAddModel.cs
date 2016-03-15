﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DatenMeister.Web.Models.PostModels
{
    /// <summary>
    /// Used to create a new extent
    /// </summary>
    public class ExtentAddModel
    {
        /// <summary>
        /// Gets or sets the workspace, where extent will be created
        /// </summary>
        public string workspace { get; set; }

        /// <summary>
        /// Gets or sets the contexturi of the new extent
        /// </summary>
        public string contextUri { get; set; }

        /// <summary>
        /// Gets or sets the filename
        /// </summary>
        public string filename { get; set; }

        /// <summary>
        /// Gets or sets the extent type to be created
        /// </summary>
        public string extentType { get; set; }
    }
}