using System.Collections.Generic;
using System.Linq;

namespace DatenMeister.Web.Models.PostModels
{
    /// <summary>
    /// Used to create a new extent
    /// </summary>
    public class ExtentCreateModel : ExtentAddModel
    {
        /// <summary>
        /// Gets or sets the columns as comma separated values
        /// </summary>
        public string columns { get; set; }

        /// <summary>
        /// Reads all columns as an enumeration
        /// </summary>
        public IEnumerable<string> ColumnsAsEnumerable => columns?.Split(',').Select(x => x.Trim());
    }
}