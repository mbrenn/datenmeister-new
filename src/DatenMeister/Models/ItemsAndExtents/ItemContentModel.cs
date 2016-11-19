using System.Collections.Generic;

namespace DatenMeister.Models.ItemsAndExtents
{
    public class ItemContentModel : ItemModel
    {
        public string id { get; set; }
        public Dictionary<string, object> v { get; set; } = new Dictionary<string, object>();
        public object c { get; set; }
        public ItemModel metaclass { get; set; }
    }
}