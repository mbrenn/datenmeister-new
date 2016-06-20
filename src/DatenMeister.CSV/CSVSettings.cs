using System.Collections.Generic;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.CSV
{
    public class CSVSettings
    {
        public CSVSettings()
        {
            Encoding = "UTF-8";
            HasHeader = true;
            Separator = ',';
        }

        public string Encoding { get; set; }

        public bool HasHeader { get; set; }

        public char Separator { get; set; }

        /// <summary>
        /// Gets or sets the type that is used within the associated csv extent.
        /// If the type is not set at loading of the instance, a new type will be automatically created
        /// </summary>
        public IElement Type { get; set; }

        /// <summary>
        /// Gets or sets the uri to the metaclass to be used to load the given extent
        /// </summary>
        public string MetaclassUri { get; set; }
    }
}