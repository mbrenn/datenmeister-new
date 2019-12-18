namespace DatenMeister.Modules.DataViews.Model
{
    public class SelectPathNode : ViewNode
    {
        public ViewNode input { get; set; }

        public string path { get; set; }
    }
}