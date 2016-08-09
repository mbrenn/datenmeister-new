using System.Collections.Generic;

namespace DatenMeister.Web.Models.PostModels
{
    public class ItemSetPropertiesModel : ItemReferenceModel
    {
        public Dictionary<string, string> v { get; set; }
    }
}