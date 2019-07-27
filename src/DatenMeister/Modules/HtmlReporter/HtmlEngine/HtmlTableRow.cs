using System.Collections.Generic;
using System.Text;

namespace DatenMeister.Modules.HtmlReporter.HtmlEngine
{
    public class HtmlTableRow
    {
        public List<object> Cells { get; } = new List<object>();

        public HtmlTableRow()
        {
            
        }

        public HtmlTableRow(IEnumerable<object> cells)
        {
            Cells.AddRange(cells);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine("<tr>");
            foreach (var cell in Cells)
            {
                builder.AppendLine(cell.ToString());
            }

            builder.AppendLine("</tr>");

            return builder.ToString();
        }
    }
}