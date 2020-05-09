using System.Collections.Generic;

namespace DatenMeister.Models.Forms
{
    /// <summary>
    /// Defines the field data for a list of checkbox which the user can click
    /// </summary>
    public class CheckboxListTaggingFieldData : FieldData
    {
        /// <summary>
        /// Gets or sets the options 
        /// </summary>
        public List<ValuePair> values { get; set; } = new List<ValuePair>();

        /// <summary>
        /// Stores the separator between the chosen values
        /// </summary>
        public string separator { get; set; } = " ";
    }
}