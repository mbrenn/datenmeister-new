using System.Net;

namespace DatenMeister.Modules.HtmlReporter.HtmlEngine
{
    public class HtmlParagraph : HtmlElement
    {
        /// <summary>
        /// Defines the css class being used for that paragraph
        /// </summary>
        private string? CssClass { get; set; }

        /// <summary>
        /// Gets or sets the paragraph to be stored
        /// </summary>
        private HtmlElement Paragraph { get; }

        public HtmlParagraph(HtmlElement paragraph)
        {
            Paragraph = paragraph;
        }

        public override string ToString()
        {
            if (CssClass == null || string.IsNullOrEmpty(CssClass))
                return $"<p>{Paragraph}</p>";

            return $"<p class=\"{WebUtility.HtmlEncode(CssClass)}\">{Paragraph}</p>";
        }
    }
}