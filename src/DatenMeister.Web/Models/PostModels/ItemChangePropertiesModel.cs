using System.Collections.Generic;

namespace DatenMeister.Web.Models.PostModels
{
    public class ItemChangePropertiesModel : ItemReferenceModel
    {
        public Dictionary<string, string> v { get; private set; }

        public ItemChangePropertiesModel()
        {
            v = new Dictionary<string, string>();
        }
    }
}