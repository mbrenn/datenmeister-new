using System.Collections.Generic;
using System.Text;

namespace DatenMeister.HtmlEngine
{
    public class HtmlTableRow : HtmlElement
    {
        private List<HtmlElement> Cells { get; } = new();

        public HtmlTableRow()
        {
        }
        
        public HtmlTableRow(IEnumerable<HtmlElement> cells)
        {
            Cells.AddRange(cells);
        }

        public void Add(HtmlElement cell)
        {
            Cells.Add(cell);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine($"<tr{AttributeString}>");
            foreach (var cell in Cells)
            {
                builder.AppendLine(cell.ToString());
            }

            builder.AppendLine("</tr>");

            return builder.ToString();
        }
    }
}