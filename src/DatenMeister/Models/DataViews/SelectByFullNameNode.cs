namespace DatenMeister.Models.DataViews
{
    public class SelectByFullNameNode : ViewNode
    {
        public ViewNode? input { get; set; }

        public string? path { get; set; }
    }
}