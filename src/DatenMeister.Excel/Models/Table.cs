using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Excel.Models
{
    public class Table
    {
        public string name { get; set; }
        public IEnumerable<object> items { get; set; }
    }
}
