namespace DatenMeister.HtmlEngine
{
    /// <summary>
    /// Defines a single html cell which is used in  a table row
    /// </summary>
    public class HtmlTableCell : HtmlElement
    {
        private readonly HtmlElement _content;

        public bool IsHeading { get; set; }

        public HtmlTableCell(HtmlElement content, string cssClass = "")
        {
            _content = content;
            CssClass = cssClass;
        }

        public override string ToString()
        {
            var htmlTab = IsHeading ? "th" : "td";
            return $"<{htmlTab}{AttributeString}>{_content}</{htmlTab}>";
        }
    }
}