using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Web.Models
{
    public class DataTableItem
    {
        public string url
        {
            get;
            set;
        }

        public Dictionary<string, string> v
        {
            get;
            set;
        }

        public DataTableItem()
        {
        }
    }
}
