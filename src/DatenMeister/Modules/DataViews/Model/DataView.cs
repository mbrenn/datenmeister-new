namespace DatenMeister.Modules.DataViews.Model
{
    public class DataView
    {
        public string name { get; set; }

        public string workspace { get; set; }

        public string uri { get; set; }

        public ViewNode viewNode { get; set; }
    }
}