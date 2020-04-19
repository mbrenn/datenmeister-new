using System.Collections.Generic;

namespace DatenMeister.Models.ManagementProvider
{
    public class Workspace
    {
        public string? id { get; set; }
        
        public string? annotation { get; set; }

        public IEnumerable<Extent>? extents { get; set; }

        /// <summary>
        /// Converts the given workspace to a string with id and annotation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"({id ?? "no id"}) {annotation ?? ""}";
        }
    }
}