namespace DatenMeister.Modules.HtmlExporter.HtmlEngine
{
    /// <summary>
    /// Defines a single html cell which is used in  a table row
    /// </summary>
    public class HtmlTableCell : HtmlElement
    {
        private readonly HtmlElement _content;

        private readonly string _cssClass;

        public bool IsHeading { get; set; }

        public HtmlTableCell(HtmlElement content, string cssClass = "")
        {
            _content = content;
            _cssClass = cssClass;
        }

        public override string ToString()
        {
            var htmlTab = IsHeading ? "th" : "td";
            if (_cssClass == null || string.IsNullOrEmpty(_cssClass))
            {
                return $"<{htmlTab}>{_content}</{htmlTab}>";
            }
            else
            {
                return $"<{htmlTab} class=\"{_cssClass}>{_content}</{htmlTab}>";
            }
        }
    }
}