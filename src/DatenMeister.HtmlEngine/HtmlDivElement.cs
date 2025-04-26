namespace DatenMeister.HtmlEngine
{
    public class HtmlDivElement : HtmlElement
    {
        /// <summary>
        /// Gets or sets the paragraph to be stored
        /// </summary>
        private HtmlElement DivElement { get; }

        public HtmlDivElement(HtmlElement divElement)
        {
            DivElement = divElement;
        }

        public override string ToString()
        {
            return $"<div{AttributeString}>{DivElement}</div>";
        }
    }
}