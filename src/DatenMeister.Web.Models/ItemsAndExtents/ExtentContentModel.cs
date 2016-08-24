﻿using System.Collections.Generic;

namespace DatenMeister.Models.ItemsAndExtents
{
    public class ExtentContentModel
    {
        public string url { get; set; }

        /// <summary>
        ///     Gets or sets the number of total items being in the given extent
        /// </summary>
        public int totalItemCount { get; set; }

        /// <summary>
        ///     Gets or sets the number of total items being in scope of the filter
        ///     of the extent.
        /// </summary>
        public int filteredItemCount { get; set; }

        public object columns { get; set; }

        public IEnumerable<DataTableItem> items { get; set; }

        /// <summary>
        /// Gets or sets the search string, which is used to validate that the correct 
        /// request is evaluated. 
        /// </summary>
        public string search { get; set; }
    }
}