namespace DatenMeister.HtmlEngine
{
    public class HtmlSpanElement : HtmlElement
    {
        /// <summary>
        /// Gets or sets the paragraph to be stored
        /// </summary>
        private HtmlElement DivElement { get; }

        public HtmlSpanElement(HtmlElement divElement)
        {
            DivElement = divElement;
        }

        public override string ToString()
        {
            return $"<span{AttributeString}>{DivElement}</span>\r\n";
        }
    }
}