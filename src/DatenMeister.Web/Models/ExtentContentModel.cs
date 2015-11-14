using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Web.Models
{
    public class ExtentContentModel
    {
        public string url
        {
            get;
            set;
        }

        public IEnumerable<DataTableColumn> columns
        {
            get;
            set;
        }

        public IEnumerable<DataTableItem> items
        {
            get;
            set;
        }
    }
}
