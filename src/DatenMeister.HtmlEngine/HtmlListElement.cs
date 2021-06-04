using System.Collections.Generic;
using System.Text;

namespace DatenMeister.HtmlEngine
{
    /// <summary>
    /// Describes an html list element describing a list.
    /// An ordered and an non-ordered list is supported by this implementation
    /// </summary>
    public class HtmlListElement: HtmlElement
    {
        public bool OrderedList { get; set; }

        public List<HtmlElement> Items { get; } = new(); 

        private string HtmlTag =>
            OrderedList ? "ol" : "ul";

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append($"<{HtmlTag}>");
            
            foreach (var item in Items)
            {
                builder.Append($"<li {AttributeString}>{item}</li>");
            }

            builder.Append($"</{HtmlTag}");

            return builder.ToString();
        }
    }
}