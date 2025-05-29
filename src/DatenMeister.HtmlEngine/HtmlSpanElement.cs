namespace DatenMeister.HtmlEngine;

public class HtmlSpanElement : HtmlElement
{
    /// <summary>
    /// Gets or sets the paragraph to be stored
    /// </summary>
    private HtmlElement Content { get; }
        
    public HtmlSpanElement(HtmlElement content)
    {
        Content = content;
    }

    public override string ToString()
    {
        return $"<span{AttributeString}>{Content}</span>";
    }
}