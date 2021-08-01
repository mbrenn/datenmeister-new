namespace DatenMeister.HtmlEngine
{
    public class HtmlParagraph : HtmlElement
    {
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
            return $"<p{AttributeString}>{Paragraph}</p>\r\n";
        }
    }
}