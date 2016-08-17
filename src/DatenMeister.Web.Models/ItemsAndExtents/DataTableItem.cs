using System.Collections.Generic;

namespace DatenMeister.Web.Models.ItemsAndExtents
{
    public class DataTableItem
    {
        public string uri { get; set; }

        public Dictionary<string, object> v { get; set; }
    }
}