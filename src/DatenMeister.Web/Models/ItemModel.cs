namespace DatenMeister.Web.Models
{
    public class ItemModel
    {
        public ItemModel()
        {
        }

        public ItemModel(ExtentModel extent, string url)
        {
            this.extent = extent;
            this.url = url;
        }

        public ExtentModel extent { get; private set; }

        public string url { get; private set; }
    }
}