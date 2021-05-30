namespace DatenMeister.HtmlEngine
{
    /// <summary>
    /// Describes the anchor element
    /// </summary>
    public class HtmlLinkElement : HtmlElement
    {
        public string Href { get; set; }
        
        public HtmlElement Content { get; set; }
        
        public override string ToString()
        {
            return $"<a{AttributeString} href=\"{Href}\">{Content}</a>";
        }
    }
}