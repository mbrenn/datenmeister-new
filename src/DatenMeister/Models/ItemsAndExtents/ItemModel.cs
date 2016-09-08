namespace DatenMeister.Models.ItemsAndExtents
{
    public class ItemModel
    {
        public ItemModel()
        {
        }

        public ItemModel(string name, string uri)
        {
            this.name = name;
            this.uri = uri;
        }

        public string name { get; set; }
        public string uri { get; set; }
        public string ext { get; set; }
        public string ws { get; set; }
        public string layer { get; set; }
    }
}