using System.Collections.Generic;

namespace DatenMeister.Provider.CSV
{
    public class CsvSettings
    {
        public CsvSettings()
        {
            Encoding = "UTF-8";
            HasHeader = true;
            Separator = ',';
            Columns = new List<string>();
        }

        public string Encoding { get; set; }

        public bool HasHeader { get; set; }

        public char Separator { get; set; }

        /// <summary>
        /// Gets or sets the type that is used within the associated csv extent.
        /// If the type is not set at loading of the instance, a new type will be automatically created
        /// </summary>
        public List<string> Columns { get; set; }

        /// <summary>
        /// Gets or sets the uri to the metaclass to be used to load the given extent
        /// </summary>
        public string MetaclassUri { get; set; }
    }
}