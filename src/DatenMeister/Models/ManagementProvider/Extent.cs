using System.Collections.Generic;

namespace DatenMeister.Models.ManagementProvider
{
    public class Extent
    {
        public string uri { get; set; }

        public int count { get; set; }

        public string type { get; set; }

        public string extentType { get; set; }
        
        public bool isModified { get; set; }

        /// <summary>
        /// Gets the alternative uris of the extent
        /// </summary>
        public IEnumerable<string> alternativeUris { get; set; }
    }
}