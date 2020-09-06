using System.Collections.Generic;
using System.Text;

namespace DatenMeister.Modules.HtmlReporter.HtmlEngine
{
    public class HtmlTable : HtmlElement
    {
        // Option 1: Create one table class and no rows and cells
        // Option 2: Create a row and cell class being capable to host content
        // Decision: Use Option 1 to create one table class

        /// <summary>
        /// Stores the list of cells
        /// </summary>
        private readonly List<HtmlTableRow> _rows = new List<HtmlTableRow>();

        /// <summary>
        /// Gets or sets the css class for the table
        /// </summary>
        public string CssClass { get; set; } = string.Empty;

        public HtmlTableRow AddRow(params HtmlElement[] cells)
        {
            var result = new HtmlTableRow(cells);
            _rows.Add(result);
            return result;
        }

        public void AddRow(HtmlTableRow tableRow)
        {
            _rows.Add(tableRow);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine("<table>");

            foreach (var row in _rows)
            {
                builder.AppendLine(row.ToString());
            }

            builder.AppendLine("</table>");

            return builder.ToString();
        }
    }
}