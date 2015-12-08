namespace DatenMeister.Web.Models.PostModels
{
    public class ItemChangePropertyModel : ItemReferenceModel
    {
        public string property { get; set; }

        public string value { get; set; }
    }
}