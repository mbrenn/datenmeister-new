using System.Collections.Generic;
using System.Text;

namespace DatenMeister.CSV
{
    public class CSVSettings
    {
        public CSVSettings()
        {
            Encoding = Encoding.UTF8;
            HasHeader = true;
            Separator = ',';
        }

        public Encoding Encoding { get; set; }

        public bool HasHeader { get; set; }

        public char Separator { get; set; }

        /// <summary>
        /// Gets or sets the columns to be used for loading
        /// If this value is null for loading, the columns will be auto-generated
        /// and stored within the instance (this means that the instance gets changed)
        /// </summary>
        public List<object> Columns { get; set; }
    }
}