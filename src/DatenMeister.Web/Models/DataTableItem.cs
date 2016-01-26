using System.Collections.Generic;

namespace DatenMeister.Web.Models
{
    public class DataTableItem
    {
        public string uri { get; set; }

        public Dictionary<string, object> v { get; set; }
    }
}