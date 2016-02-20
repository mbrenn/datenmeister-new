using System.Collections.Generic;

namespace DatenMeister.Web.Models
{
    public class ItemContentModel
    {
        public ItemContentModel()
        {
            v = new Dictionary<string, string>();
            c = new List<DataFormRow>();
        }

        public string uri { get; set; }

        public Dictionary<string, string> v { get; private set; }

        public List<DataFormRow> c { get; private set; }

        public ItemModel metaclass { get; set; }
    }
}