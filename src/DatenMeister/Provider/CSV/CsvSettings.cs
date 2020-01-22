using System.Collections.Generic;

namespace DatenMeister.Provider.CSV
{
    public class CsvSettings
    {
        public string? Encoding { get; set; } = "UTF-8";

        public bool HasHeader { get; set; } = true;

        public char Separator { get; set; } = ',';

        /// <summary>
        /// Gets or sets the type that is used within the associated csv extent.
        /// If the type is not set at loading of the instance, a new type will be automatically created
        /// </summary>
        public List<string> Columns { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the uri to the metaclass to be used to load the given extent
        /// </summary>
        public string MetaclassUri { get; set; } = string.Empty;
    }
}