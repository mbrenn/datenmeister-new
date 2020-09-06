using System.Collections.Generic;
using System.Text;

namespace DatenMeister.Modules.HtmlExporter.HtmlEngine
{
    public class HtmlTableRow
    {
        private List<HtmlElement> Cells { get; } = new List<HtmlElement>();

        public HtmlTableRow(IEnumerable<HtmlElement> cells)
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