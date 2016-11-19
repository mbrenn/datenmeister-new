namespace DatenMeister.Models.PostModels
{
    public class ItemCreateModel : ExtentReferenceModel
    {
        /// <summary>
        /// Gets the metaclass
        /// </summary>
        public string metaclass { get; set; }
        
        /// <summary>
        /// Defines the url of the item to which the item shall be added
        /// </summary>
        public string parentItem { get; set; }

        /// <summary>
        /// Defines the property to which the item will be added 
        /// </summary>
        public string parentProperty { get; set; }
    }
}