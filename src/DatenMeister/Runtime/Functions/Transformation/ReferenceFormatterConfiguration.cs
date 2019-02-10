using System.Collections.Generic;

namespace DatenMeister.Runtime.Functions.Transformation
{
    /// <summary>
    /// Defines the configuration for the reference formatter
    /// </summary>
    public class ReferenceFormatterConfiguration
    {
        /// <summary>
        /// This is a list of properties that will be transfered as subitems
        /// </summary>
        public IList<string> FixedProperty { get; } = new List<string>();

        /// <summary>
        /// The property that will be created an which will host the subitems
        /// </summary>
        public string SubItemProperty { get; set; }

        /// <summary>
        /// The name of the property in subitem that will store the reference to the item
        /// </summary>
        public string ReferenceProperty { get; set; }

        /// <summary>
        /// The name of the property in subitem that will store the value of the item
        /// </summary>
        public string ContentProperty { get; set; }

        /// <summary>
        /// Gets or sets the value whether real references shall be created instead of a pure namesetting
        /// </summary>
        public bool CreateReferences { get; set; }
        
    }
}