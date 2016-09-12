using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Excel.Models
{
    public class Workbook
    {
        public IEnumerable<Table> tables { get; set; }
    }
}
