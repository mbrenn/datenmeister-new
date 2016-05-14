using System.Collections.Generic;
using DatenMeister.Web.Models.Fields;

namespace DatenMeister.Web.Models
{
    public class ItemContentModel
    {
        public string uri { get; set; }

        public string layer { get; set; }

        public Dictionary<string, object> v { get; set; }

        public List<DataField> c { get; set; }

        public ItemModel metaclass { get; set; }
    }
}