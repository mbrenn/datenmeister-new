namespace DatenMeister.HtmlEngine;

/// <summary>
/// Describes the anchor element
/// </summary>
public class HtmlLinkElement : HtmlElement
{
    public HtmlLinkElement()
    {
    }

    public HtmlLinkElement(HtmlElement content)
    {
        Content = content;
    }
        
    public string Href { get; set; } = string.Empty;

    public HtmlElement Content { get; set; } = string.Empty;
        
    public override string ToString()
    {
        return $"<a{AttributeString} href=\"{Href}\">{Content}</a>";
    }
}