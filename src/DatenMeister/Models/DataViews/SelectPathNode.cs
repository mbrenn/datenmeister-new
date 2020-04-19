namespace DatenMeister.Models.DataViews
{
    public class SelectPathNode : ViewNode
    {
        public ViewNode? input { get; set; }

        public string? path { get; set; }
    }
}