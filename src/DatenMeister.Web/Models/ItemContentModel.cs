using System.Collections.Generic;

namespace DatenMeister.Web.Models
{
    public class ItemContentModel
    {
        public string uri { get; set; }

        public Dictionary<string, string> values
        {
            get; private set;
        }

        public ItemContentModel()
        {
            values = new Dictionary<string, string>();
        }
    }
}