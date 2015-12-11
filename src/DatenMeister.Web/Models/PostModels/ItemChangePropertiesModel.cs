using System.Collections.Generic;

namespace DatenMeister.Web.Models.PostModels
{
    public class ItemChangePropertiesModel : ItemReferenceModel
    {
        public ItemChangePropertiesModel()
        {
            v = new Dictionary<string, string>();
        }

        public Dictionary<string, string> v { get; private set; }
    }
}