using System.Collections.Generic;
using System.Text;

namespace DatenMeister.Modules.HtmlReporter.HtmlEngine
{
    public class HtmlTable
    {
        // Option 1: Create one table class and no rows and cells
        // Option 2: Create a row and cell class being capable to host content
        // Decision: Use Option 1 to create one table class

        /// <summary>
        /// Stores the list of cells
        /// </summary>
        private List<HtmlTableRow> Rows = new List<HtmlTableRow>();
        
        /// <summary>
        /// Initializes a new instance of the HtmlTable class
        /// </summary>
        public HtmlTable()
        {
            
        }

        public HtmlTableRow AddRow(params object[] cells)
        {
            var result = new HtmlTableRow();
            Rows.Add(result);
            return result;
        }

        public void AddRow(HtmlTableRow tableRow)
        {
            Rows.Add(tableRow);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine("<table>");

            foreach (var row in Rows)
            {
                builder.AppendLine(row.ToString());
            }
            
            builder.AppendLine("</table>");

            return builder.ToString();
        }
    }
}