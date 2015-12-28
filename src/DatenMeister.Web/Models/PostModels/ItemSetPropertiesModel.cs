using System.Collections.Generic;

namespace DatenMeister.Web.Models.PostModels
{
    public class ItemSetPropertiesModel : ItemReferenceModel
    {
        public ItemSetPropertiesModel()
        {
            v = new Dictionary<string, string>();
        }

        public Dictionary<string, string> v { get; private set; }
    }
}