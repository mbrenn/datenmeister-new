using System.Collections.Generic;

namespace DatenMeister.Web.Models.ItemsAndExtents
{
    public class ItemContentModel
    {
        public string uri { get; set; }

        public string layer { get; set; }

        public Dictionary<string, object> v { get; set; }

        public object c { get; set; }

        public ItemModel metaclass { get; set; }
    }
}