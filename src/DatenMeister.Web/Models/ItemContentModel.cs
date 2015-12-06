using System.Collections.Generic;

namespace DatenMeister.Web.Models
{
    public class ItemContentModel
    {
        public string uri { get; set; }

        public Dictionary<string, string> v
        {
            get; private set;
        }

        public ItemContentModel()
        {
            v = new Dictionary<string, string>();
        }
    }
}