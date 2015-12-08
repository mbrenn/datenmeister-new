namespace DatenMeister.Web.Models.PostModels
{
    /// <summary>
    /// This class is used to reference a single object within the database
    /// </summary>
    public class ItemReferenceModel
    {
        public string ws { get; set; }

        public string extent { get; set; }

        public string item { get; set; }
    }
}