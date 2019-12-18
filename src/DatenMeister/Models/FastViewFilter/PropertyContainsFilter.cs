namespace DatenMeister.Models.FastViewFilter
{
    /// <summary>
    /// Defines the filter which verifies that the property contains a certain value
    /// </summary>
    public class PropertyContainsFilter
    {
        /// <summary>
        /// Gets or sets the property being filtered
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Gets or sets the value for the filter
        /// </summary>
        public string Value { get; set; }
    }
}