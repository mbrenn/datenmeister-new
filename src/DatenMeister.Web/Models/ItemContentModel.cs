using System.Collections.Generic;

namespace DatenMeister.Web.Models
{
    public class ItemContentModel
    {
        public string uri { get; set; }

        public string layer { get; set; }

        public Dictionary<string, object> v { get; set; }

        public List<DataTableColumn> c { get; set; }

        public ItemModel metaclass { get; set; }
    }
}