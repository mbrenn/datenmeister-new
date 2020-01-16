namespace DatenMeister.Models.DataViews
{
    public enum ComparisonMode
    {
        Equal,
        NotEqual,
        Contains,
        DoesNotContain,
        GreaterThan,
        GreaterOrEqualThan,
        LighterThan,
        LighterOrEqualThan
    }

    public class FilterPropertyNode : ViewNode
    {
        public ViewNode? input { get; set; }
        public string? property { get; set; }
        public string? value { get;set; }
        public ComparisonMode comparisonMode { get; set; }
    }
}