namespace DatenMeister.Models.FastViewFilter
{
    public enum ComparisonType
    {
        Equal,
        GreaterThan,
        LighterThan,
        GreaterOrEqualThan,
        LighterOrEqualThan
    }

    public class PropertyComparisonFilter
    {
        /// <summary>
        /// Gets or sets the property being filtered
        /// </summary>
        public string Property { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the comparison type
        /// </summary>
        public ComparisonType ComparisonType { get; set; }

        /// <summary>
        /// Gets or sets the value of the Comparison
        /// </summary>
        public string Value { get; set; } = string.Empty;
    }
}